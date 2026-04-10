using cCoder.Scheduling.Api.OData;


namespace cCoder.Scheduling.Services.Foundations;

internal interface ISchedulingMetadataTypeService
{
    IEnumerable<MetadataContainerSet> GetKnownMetadata();
}

