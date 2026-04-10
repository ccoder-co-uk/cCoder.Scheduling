using cCoder.Scheduling.Api.OData;
using cCoder.Data.Models.Planning;


namespace cCoder.Scheduling.Services.Foundations;

internal sealed class SchedulingMetadataTypeService : ISchedulingMetadataTypeService
{
    public IEnumerable<MetadataContainerSet> GetKnownMetadata() =>
    [
        new MetadataContainerSet
        {
            Name = "Core",
            UriBase = "Core",
            Types =
            [
                Entity<Calendar>(),
                Entity<CalendarEvent>(),
                Entity<ScheduledTask>(),
            ],
        },
    ];

    private static ExtendedMetadataContainer Entity<T>() =>
        new(typeof(T), isEntity: true, hasEndpoint: true)
        {
            Category = "Core",
        };
}

