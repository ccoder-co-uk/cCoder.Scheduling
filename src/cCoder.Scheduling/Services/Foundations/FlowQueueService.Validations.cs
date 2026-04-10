using System.ComponentModel.DataAnnotations;
using cCoder.Scheduling.Models;
using cCoder.Data.Models.CMS;
using cCoder.Data.Models.Planning;
using cCoder.Data.Models.Security;
using cCoder.Data.Models.Workflow;


namespace cCoder.Scheduling.Services.Foundations;

internal partial class FlowQueueService
{
    private static void ValidateId(Guid id)
    {
        if (id == Guid.Empty)
            throw new ValidationException("Id is required.");
    }

    private static void ValidateUserId(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ValidationException("UserId is required.");
    }

    private static void ValidateFlowInstanceData(FlowInstanceData entity)
    {
        if (entity is null)
            throw new ValidationException("Flow instance data is required.");

        if (entity.Id == Guid.Empty)
            throw new ValidationException("Flow instance id is required.");

        if (entity.FlowDefinitionId == Guid.Empty)
            throw new ValidationException("FlowDefinitionId is required.");

        if (string.IsNullOrWhiteSpace(entity.State))
            throw new ValidationException("State is required.");

        if (string.IsNullOrWhiteSpace(entity.Caller))
            throw new ValidationException("Caller is required.");

        if (string.IsNullOrWhiteSpace(entity.ContextString))
            throw new ValidationException("ContextString is required.");
    }
}

