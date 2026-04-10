using cCoder.Scheduling.Models;
using cCoder.Data.Models.CMS;
using cCoder.Data.Models.Planning;
using cCoder.Data.Models.Security;
using cCoder.Data.Models.Workflow;


namespace cCoder.Scheduling.Exposures;

public interface ISchedulingPackageManager
{
    ValueTask ImportPackageAsync(int appId, SchedulingPackage package);

    SchedulingPackage ExportPackage(int appId, string packageName);
}


