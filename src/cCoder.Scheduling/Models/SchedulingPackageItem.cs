namespace cCoder.Scheduling.Models;

public class SchedulingPackageItem
{
    public Guid Id { get; set; }

    public Guid PackageId { get; set; }

    public string Type { get; set; }

    public string Data { get; set; }

    public virtual SchedulingPackage Package { get; set; }
}

