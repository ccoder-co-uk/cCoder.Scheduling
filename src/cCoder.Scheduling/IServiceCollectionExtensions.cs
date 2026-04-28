using cCoder.Data.Models.CMS;
using cCoder.Data.Models.Packaging;
using cCoder.Data.Models.Planning;
using cCoder.Scheduling.Api.OData;
using cCoder.Scheduling.Models;
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

public static partial class IServiceCollectionExtensions
{
    public static void AddSchedulingWeb(
        this IServiceCollection services,
        Action<SchedulingConfiguration> configure = null,
        ODataConventionModelBuilder builder = null) =>
        services.AddConfiguredSchedulingWeb((_, configuration) => configure?.Invoke(configuration), builder);

    public static void AddSchedulingHostedServices(
        this IServiceCollection services,
        Action<SchedulingConfiguration> configure = null) =>
        services.AddConfiguredSchedulingHostedServices((_, configuration) => configure?.Invoke(configuration));

    private static void AddScheduling(this IServiceCollection services)
    {
        services.AddEventingTypes();
        services.AddBrokers();
        services.AddFoundations();
        services.AddProcessings();
        services.AddOrchestrations();
        services.AddCoordinations();
        services.AddEventHandlers();
    }

    private static void AddSchedulingWeb(this IServiceCollection services, ODataConventionModelBuilder builder = null)
    {
        services.AddScheduling();

    }

    private static void AddSchedulingHostedServices(this IServiceCollection services)
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
}
