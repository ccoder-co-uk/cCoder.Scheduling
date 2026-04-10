using cCoder.Scheduling.Models;
using cCoder.Data.Models.CMS;
using cCoder.Data.Models.Planning;
using cCoder.Data.Models.Security;
using cCoder.Data.Models.Workflow;
using cCoder.Scheduling.Services.Orchestrations;

namespace cCoder.Scheduling.Exposures;

internal class SchedulingAppExposure(IAppOrchestrationService appOrchestrationService)
    : ISchedulingAppExposure
{
    public ValueTask AddAsync(App app) => appOrchestrationService.AddAsync(app);
    public ValueTask UpdateAsync(App app) => appOrchestrationService.UpdateAsync(app);
    public ValueTask DeleteAsync(int appId) => appOrchestrationService.DeleteAsync(appId);
}

