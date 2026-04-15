using cCoder.Scheduling.Api.OData;
using cCoder.Scheduling.Models;
using EventLibrary;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Batch;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi;

namespace cCoder.Scheduling;

public static class SchedulingServiceCollectionConfigurationExtensions
{
    public static SchedulingConfiguration AddScheduling(
        this IServiceCollection services,
        Action<IServiceCollection, SchedulingConfiguration> configure)
    {
        SchedulingConfiguration configuration = CreateConfiguration(services, configure);
        IServiceCollectionExtensions.AddScheduling(services);
        return configuration;
    }

    public static SchedulingConfiguration AddSchedulingApi(
        this IServiceCollection services,
        Action<IServiceCollection, SchedulingConfiguration> configure,
        ODataConventionModelBuilder builder = null)
    {
        SchedulingConfiguration configuration = CreateConfiguration(services, configure);
        IServiceCollectionExtensions.AddScheduling(services);
        services.AddConfiguredApi(
            configuration,
            "Scheduling",
            static modelBuilder => modelBuilder.ConfigureSchedulingApiModel(),
            builder);

        return configuration;
    }

    public static SchedulingConfiguration AddSchedulingHostedServices(
        this IServiceCollection services,
        Action<IServiceCollection, SchedulingConfiguration> configure)
    {
        SchedulingConfiguration configuration = CreateConfiguration(services, configure);
        IServiceCollectionExtensions.AddSchedulingHostedServices(services);
        return configuration;
    }

    internal static void ConfigureSchedulingApiModel(this ODataConventionModelBuilder builder) =>
        new SchedulingModelBuilder(builder).Configure();

    private static SchedulingConfiguration CreateConfiguration(
        IServiceCollection services,
        Action<IServiceCollection, SchedulingConfiguration> configure)
    {
        SchedulingConfiguration configuration = new();
        configure?.Invoke(services, configuration);
        services.AddSingleton(configuration);
        services.AddEventProviders(configuration.EventProviders);
        return configuration;
    }

    private static void AddConfiguredApi(
        this IServiceCollection services,
        SchedulingConfiguration configuration,
        string documentName,
        Action<ODataConventionModelBuilder> configureModel,
        ODataConventionModelBuilder builder = null,
        bool useFullSchemaIds = false)
    {
        services.AddSingleton<Action<ODataConventionModelBuilder>>(configureModel);

        if (builder is not null)
            configureModel(builder);

        AddAspNet(services);

        if (builder is null)
            AddApiDocumentation(services, documentName, configuration, useFullSchemaIds);

        IEdmModel routeModel = BuildRouteModel(configureModel);
        DefaultODataBatchHandler batchHandler = new();
        string rootPath = string.IsNullOrWhiteSpace(configuration.RootPath)
            ? $"Api/{documentName}"
            : configuration.RootPath;

        services.AddControllers().AddOData(options =>
        {
            options.RouteOptions.EnableQualifiedOperationCall = false;
            options.EnableAttributeRouting = true;
            options.RouteOptions.EnableKeyAsSegment = false;
            options.Expand()
                .Count()
                .Filter()
                .Select()
                .OrderBy()
                .SetMaxTop(1000)
                .AddRouteComponents(rootPath, routeModel, batchHandler);

            if (builder is null
                && configuration.IncludeLegacyCoreContext
                && !string.Equals(rootPath, "Api/Core", StringComparison.OrdinalIgnoreCase))
            {
                _ = options.AddRouteComponents("Api/Core", routeModel, batchHandler);
            }
        });
    }

    private static void AddApiDocumentation(
        IServiceCollection services,
        string documentName,
        SchedulingConfiguration configuration,
        bool useFullSchemaIds)
    {
        services.AddSwaggerGen(options =>
        {
            options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            AddSwaggerDocuments(options, documentName, configuration);
            options.DocInclusionPredicate(
                (swaggerDocumentName, apiDescription) =>
                    ShouldIncludeInDocument(
                        swaggerDocumentName,
                        apiDescription.RelativePath,
                        documentName,
                        configuration));

            if (useFullSchemaIds)
                options.CustomSchemaIds(type => type.FullName?.Replace('+', '.') ?? type.Name);

            options.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
            {
                Description = @"Authorization header using the Bearer scheme.",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "bearer",
            });
        });
    }

    private static void AddSwaggerDocuments(
        Swashbuckle.AspNetCore.SwaggerGen.SwaggerGenOptions options,
        string documentName,
        SchedulingConfiguration configuration)
    {
        options.SwaggerDoc(documentName, new OpenApiInfo
        {
            Title = $"{documentName} API definition",
            Version = documentName,
        });

        if (configuration.IncludeLegacyCoreContext)
        {
            options.SwaggerDoc("Core", new OpenApiInfo
            {
                Title = "Core API definition",
                Version = "Core",
            });
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Core API definition",
                Version = "v1",
            });
        }
    }

    private static bool ShouldIncludeInDocument(
        string swaggerDocumentName,
        string relativePath,
        string documentName,
        SchedulingConfiguration configuration)
    {
        if (string.IsNullOrWhiteSpace(relativePath))
            return false;

        if (string.Equals(swaggerDocumentName, "v1", StringComparison.OrdinalIgnoreCase))
            swaggerDocumentName = "Core";

        string path = NormalizePath(relativePath);
        string rootPath = string.IsNullOrWhiteSpace(configuration.RootPath)
            ? $"Api/{documentName}"
            : configuration.RootPath;

        return string.Equals(swaggerDocumentName, "Core", StringComparison.OrdinalIgnoreCase)
            ? configuration.IncludeLegacyCoreContext && MatchesContextRoute(path, "Api/Core")
            : MatchesContextRoute(path, rootPath);
    }

    private static bool MatchesContextRoute(string path, string rootPath)
    {
        string normalizedPath = NormalizePath(rootPath);
        return path.Equals(normalizedPath, StringComparison.OrdinalIgnoreCase)
            || path.StartsWith($"{normalizedPath}/", StringComparison.OrdinalIgnoreCase);
    }

    private static string NormalizePath(string relativePath) =>
        relativePath.StartsWith('/') ? relativePath : $"/{relativePath}";

    private static IEdmModel BuildRouteModel(Action<ODataConventionModelBuilder> configureModel)
    {
        ODataConventionModelBuilder builder = new();
        configureModel(builder);
        return builder.GetEdmModel();
    }

    private static void AddAspNet(IServiceCollection services)
    {
        services.AddRouting();
        services.AddResponseCompression();
        services.AddHttpClient();
        services.AddHttpContextAccessor();
        services.AddScoped(
            typeof(HttpContext),
            ctx => ctx.GetService<IHttpContextAccessor>()?.HttpContext ?? new DefaultHttpContext());
        services.AddScoped(typeof(HttpRequest), ctx => ctx.GetRequiredService<HttpContext>().Request);
        services.AddSession();
        services.AddHsts(options =>
        {
            options.Preload = true;
            options.IncludeSubDomains = true;
            options.MaxAge = TimeSpan.FromMinutes(60);
        });
        services.AddMvc(options => options.EnableEndpointRouting = false);
        services.AddRazorPages();
        services.Configure<KestrelServerOptions>(options =>
        {
            options.Limits.MaxRequestBodySize = int.MaxValue;
        });
        services.AddEndpointsApiExplorer();
        services.AddSignalR();
    }
}

