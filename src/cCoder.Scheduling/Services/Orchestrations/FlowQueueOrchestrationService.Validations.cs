using System.ComponentModel.DataAnnotations;

namespace cCoder.Scheduling.Services.Orchestrations;

internal partial class FlowQueueOrchestrationService
{
    private static void ValidateId(Guid id)
    {
        if (id == Guid.Empty)
            throw new ValidationException("Id is required.");
    }

    private static void ValidateUserId(string asUserId)
    {
        if (string.IsNullOrWhiteSpace(asUserId))
            throw new ValidationException("UserId is required.");
    }

    private static void ValidateArgs(string args)
    {
        if (args is null)
            throw new ValidationException("Args are required.");
    }
}

