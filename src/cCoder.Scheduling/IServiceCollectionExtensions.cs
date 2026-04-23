using cCoder.Data.Models.CMS;
using cCoder.Data.Models.Packaging;
using cCoder.Data.Models.Planning;
using cCoder.Scheduling.Api.OData;
using cCoder.Scheduling.Brokers.Events;
using cCoder.Scheduling.Brokers.Storage;
using cCoder.Scheduling.Exposures;
using cCoder.Scheduling.Exposures.EventHandlers;
using cCoder.Scheduling.Exposures.HostedServices;
using cCoder.Scheduling.Services.Aggregations;
using cCoder.Scheduling.Services.Coordinations;
using cCoder.Scheduling.Services.Foundations;
using cCoder.Scheduling.Services.Foundations.Events;
using cCoder.Scheduling.Services.Orchestrations;
using cCoder.Scheduling.Services.Processings;
using cCoder.Eventing;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Batch;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi;
using AuthorizationBroker = cCoder.Scheduling.Brokers.AuthorizationBroker;
using IAuthorizationBroker = cCoder.Scheduling.Brokers.IAuthorizationBroker;
using IJsonBroker = cCoder.Scheduling.Brokers.IJsonBroker;
using JsonBroker = cCoder.Scheduling.Brokers.JsonBroker;


namespace cCoder.Scheduling;

public static class IServiceCollectionExtensions
{
    public static void AddScheduling(this IServiceCollection services)
    {
        services.AddEventingTypes();
        services.AddBrokers();
        services.AddFoundations();
        services.AddProcessings();
        services.AddOrchestrations();
        services.AddCoordinations();
        services.AddEventHandlers();
    }

    public static void AddSchedulingApi(this IServiceCollection services, ODataConventionModelBuilder builder = null)
    {
        services.AddScheduling();
        services.AddApi("Scheduling", ConfigureSchedulingApiModel, builder);
    }

    public static void AddSchedulingHostedServices(this IServiceCollection services)
    {
        services.AddEventingTypes();
        services.AddBrokers();
        services.AddFoundations();
        services.AddProcessings();
        services.AddOrchestrations();
        services.AddCoordinations();
        services.AddTransient<ITaskRunnerOrchestrationService, TaskRunnerOrchestrationService>();
        services.AddHostedService<TaskRunnerHostedService>();
    }

    private static void AddEventingTypes(this IServiceCollection services)
    {
        services.AddEventingForType<App>();
        services.AddEventingForType<Calendar>();
        services.AddEventingForType<CalendarEvent>();
        services.AddEventingForType<Package>();
        services.AddEventingForType<PackageItem>();
        services.AddEventingForType<(int, Package)>();
        services.AddEventingForType<ScheduledTask>();
        services.AddEventingForType<cCoder.Data.Models.Planning.Calendar>();
    }

    private static void AddBrokers(this IServiceCollection services)
    {
        services.AddTransient<IEventHubBroker, EventHubBroker>();
        services.AddTransient<ICalendarEntityEventBroker, CalendarEntityEventBroker>();
        services.AddTransient<ICalendarEventEventBroker, CalendarEventEventBroker>();
        services.AddTransient<IScheduledTaskEventBroker, ScheduledTaskEventBroker>();
        services.AddTransient<ICalendarBroker, CalendarBroker>();
        services.AddTransient<ICalendarEventBroker, CalendarEventBroker>();
        services.AddTransient<IScheduledTaskBroker, ScheduledTaskBroker>();
        services.AddTransient<IAuthorizationBroker, AuthorizationBroker>();
        services.AddTransient<IJsonBroker, JsonBroker>();
    }

    private static void AddCoordinations(this IServiceCollection services) =>
        services.AddTransient<ICalendarCoordinationService, CalendarCoordinationService>();

    private static void AddEventHandlers(this IServiceCollection services)
    {
        services.AddTransient<ISchedulingAppExposure, SchedulingAppExposure>();
        services.AddTransient<ISchedulingPackageManager, SchedulingPackageManager>();
        services.AddTransient<ISchedulingEventHandlers, SchedulingEventHandlers>();
    }

    private static void AddFoundations(this IServiceCollection services)
    {
        services.AddTransient<Services.Foundations.Events.IEventHandlerService, Services.Foundations.Events.EventHandlerService>();
        services.AddTransient<ICalendarEventService, CalendarEventService>();
        services.AddTransient<ICalendarService, CalendarService>();
        services.AddTransient<ISchedulingMetadataTypeService, SchedulingMetadataTypeService>();
        services.AddTransient<IScheduledTaskService, ScheduledTaskService>();
        services.AddTransient<ICalendarEntityEventService, CalendarEntityEventService>();
        services.AddTransient<ICalendarEventEventService, CalendarEventEventService>();
        services.AddTransient<IScheduledTaskEventService, ScheduledTaskEventService>();
    }

    private static void AddOrchestrations(this IServiceCollection services)
    {
        services.AddTransient<IAppOrchestrationService, AppOrchestrationService>();
        services.AddTransient<ISchedulingMigrationAggregationService, SchedulingMigrationAggregationService>();
        services.AddTransient<ICalendarEventOrchestrationService, CalendarEventOrchestrationService>();
        services.AddTransient<ICalendarOrchestrationService, CalendarOrchestrationService>();
        services.AddTransient<IScheduledTaskOrchestrationService, ScheduledTaskOrchestrationService>();
        services.AddTransient<ITaskRunnerOrchestrationService, TaskRunnerOrchestrationService>();
    }

    private static void AddProcessings(this IServiceCollection services)
    {
        services.AddTransient<ICalendarEntityEventProcessingService, CalendarEntityEventProcessingService>();
        services.AddTransient<ICalendarEventEventProcessingService, CalendarEventEventProcessingService>();
        services.AddTransient<ICalendarEventProcessingService, CalendarEventProcessingService>();
        services.AddTransient<ICalendarProcessingService, CalendarProcessingService>();
        services.AddTransient<IScheduledTaskEventProcessingService, ScheduledTaskEventProcessingService>();
        services.AddTransient<IScheduledTaskProcessingService, ScheduledTaskProcessingService>();
    }

    private static void ConfigureSchedulingApiModel(ODataConventionModelBuilder builder) =>
        new SchedulingModelBuilder(builder).Configure();

    private static void AddApi(
        this IServiceCollection services,
        string routePrefix,
        Action<ODataConventionModelBuilder> configureModel,
        ODataConventionModelBuilder builder = null,
        bool useFullSchemaIds = false)
    {
        services.AddSingleton<Action<ODataConventionModelBuilder>>(configureModel);

        if (builder is not null)
            configureModel(builder);

        AddAspNet(services);

        if (builder is null)
            AddApiDocumentation(services, routePrefix, useFullSchemaIds);

        IEdmModel routeModel = BuildRouteModel(configureModel);
        DefaultODataBatchHandler batchHandler = new();

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
                .AddRouteComponents($"Api/{routePrefix}", routeModel, batchHandler);

            if (builder is null)
                _ = options.AddRouteComponents("Api/Core", routeModel, batchHandler);
        });
    }

    private static void AddApiDocumentation(
        IServiceCollection services,
        string routePrefix,
        bool useFullSchemaIds)
    {
        services.AddSwaggerGen(options =>
        {
            options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            AddSwaggerDocuments(options, routePrefix);
            options.DocInclusionPredicate(
                (documentName, apiDescription) =>
                    ShouldIncludeInDocument(documentName, apiDescription.RelativePath, routePrefix));

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
        string routePrefix)
    {
        options.SwaggerDoc(routePrefix, new OpenApiInfo
        {
            Title = $"{routePrefix} API definition",
            Version = routePrefix,
        });
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

    private static bool ShouldIncludeInDocument(
        string documentName,
        string relativePath,
        string routePrefix)
    {
        if (string.IsNullOrWhiteSpace(relativePath))
            return false;

        if (string.Equals(documentName, "v1", StringComparison.OrdinalIgnoreCase))
            documentName = "Core";

        string path = NormalizePath(relativePath);

        return string.Equals(documentName, "Core", StringComparison.OrdinalIgnoreCase)
            ? MatchesContextRoute(path, "Core")
            : MatchesContextRoute(path, routePrefix);
    }

    private static bool MatchesContextRoute(string path, string context)
    {
        string prefix = $"/Api/{context}";
        return path.Equals(prefix, StringComparison.OrdinalIgnoreCase)
            || path.StartsWith($"{prefix}/", StringComparison.OrdinalIgnoreCase);
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







