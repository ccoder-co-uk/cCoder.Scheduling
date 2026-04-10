using System;
using System.Text.Json;
using cCoder.Data.Exposures;
using cCoder.Scheduling.Exposures.EventHandlers;
using cCoder.Scheduling.Services.Foundations;


namespace cCoder.Scheduling;

public static class WebApplicationExtensions
{
    private const string MetadataScope = "Scheduling";

    public static WebApplication UseSchedulingExposure(this WebApplication app, ILogger log = null)
    {
        log?.LogInformation("Initialising Scheduling");
        PopulateMetadataTypeCache(app);
        return app;
    }

    public static WebApplication UseSchedulingEventHandlers(this WebApplication app)
    {
        using IServiceScope scope = app.Services.CreateScope();
        IServiceProvider services = scope.ServiceProvider;

        foreach (ISchedulingEventHandlers handlers in services.GetServices<ISchedulingEventHandlers>())
            handlers.ListenToAllEvents();

        return app;
    }

    private static void PopulateMetadataTypeCache(WebApplication app)
    {
        IMetadataTypeCache metadataTypeCache = app.Services.GetRequiredService<IMetadataTypeCache>();

        if (!metadataTypeCache.Contains(MetadataScope))
        {
            metadataTypeCache.Set(
                MetadataScope,
                app.Services
                    .GetRequiredService<ISchedulingMetadataTypeService>()
                    .GetKnownMetadata()
                    .Select(static metadata => JsonSerializer.Serialize(metadata)));
        }
    }
}




