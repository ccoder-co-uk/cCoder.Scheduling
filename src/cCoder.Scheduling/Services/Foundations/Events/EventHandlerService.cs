using cCoder.Data.Models.Packaging;
using cCoder.Scheduling.Brokers.Events;
using cCoder.Scheduling.Models;
using cCoder.Data.Models.CMS;
using cCoder.Data.Models.Planning;
using cCoder.Data.Models.Security;
using cCoder.Data.Models.Workflow;
using cCoder.Scheduling.Services.Aggregations;
using cCoder.Scheduling.Services.Coordinations;
using DataPackageItem = cCoder.Data.Models.Packaging.PackageItem;


namespace cCoder.Scheduling.Services.Foundations.Events;

internal class EventHandlerService(IEventHubBroker eventHubBroker) : IEventHandlerService
{
    public void ListenToAllEvents()
    {
        ListenToAppEvents();
        ListenToCalendarEvents();
        ListenToPackageEvents();
    }

    void ListenToAppEvents()
    {
        ListenToAppAddEvents();
        ListenToAppUpdateEvents();
        ListenToAppDeleteEvents();
    }

    void ListenToCalendarEvents()
    {
        ListenToCalendarAddEvents();
        ListenToCalendarUpdateEvents();
        ListenToCalendarDeleteEvents();
    }

    void ListenToPackageEvents() => ListenToPackageImportEvents();

    void ListenToAppAddEvents() =>
        eventHubBroker.ListenToEvent<App, Services.Orchestrations.IAppOrchestrationService>(
            "app_add",
            (service, app) => service.AddAsync(app));

    void ListenToAppUpdateEvents() =>
        eventHubBroker.ListenToEvent<App, Services.Orchestrations.IAppOrchestrationService>(
            "app_update",
            (service, app) => service.UpdateAsync(app));

    void ListenToAppDeleteEvents() =>
        eventHubBroker.ListenToEvent<App, Services.Orchestrations.IAppOrchestrationService>(
            "app_delete",
            (service, app) => service.DeleteAsync(app.Id));

    void ListenToCalendarAddEvents() =>
        eventHubBroker.ListenToEvent<Calendar, ICalendarCoordinationService>(
            "calendar_add",
            (service, calendar) => service.HandleCalendarAddAsync(calendar));

    void ListenToCalendarUpdateEvents() =>
        eventHubBroker.ListenToEvent<Calendar, ICalendarCoordinationService>(
            "calendar_update",
            (service, calendar) => service.HandleCalendarUpdateAsync(calendar));

    void ListenToCalendarDeleteEvents() =>
        eventHubBroker.ListenToEvent<Calendar, ICalendarCoordinationService>(
            "calendar_delete",
            (service, calendar) => service.HandleCalendarDeleteAsync(calendar));

    void ListenToPackageImportEvents() =>
        eventHubBroker.ListenToEvent<(int appId, Package package), ISchedulingMigrationAggregationService>(
            "package_import",
            (service, args) => service.ImportPackageAsync(args.appId, ToLocalPackage(args.package)));

    static SchedulingPackage ToLocalPackage(Package package) =>
        package == null ? null : new SchedulingPackage(package.Name)
        {
            Id = package.Id,
            Name = package.Name,
            Description = package.Description,
            Category = package.Category,
            SourceApi = package.SourceApi,
            Items = package.Items?.Select(ToLocalPackageItem).ToArray(),
        };

    static SchedulingPackageItem ToLocalPackageItem(DataPackageItem packageItem) =>
        packageItem == null ? null : new SchedulingPackageItem
        {
            Id = packageItem.Id,
            PackageId = packageItem.PackageId,
            Type = packageItem.Type,
            Data = packageItem.Data,
        };
}



