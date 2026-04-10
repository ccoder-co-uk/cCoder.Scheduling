using cCoder.Scheduling.Models;
using cCoder.Data.Models.CMS;
using cCoder.Data.Models.Planning;
using cCoder.Data.Models.Security;
using cCoder.Data.Models.Workflow;
using cCoder.Scheduling.Services.Aggregations;


namespace cCoder.Scheduling.Exposures;

internal class SchedulingPackageManager(
    ISchedulingMigrationAggregationService schedulingMigrationAggregationService
) : ISchedulingPackageManager
{
    public ValueTask ImportPackageAsync(int appId, SchedulingPackage package) =>
        schedulingMigrationAggregationService.ImportPackageAsync(appId, package);

    public SchedulingPackage ExportPackage(int appId, string packageName) =>
        schedulingMigrationAggregationService.ExportPackage(appId, packageName);
}


