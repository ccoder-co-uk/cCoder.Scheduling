namespace cCoder.Scheduling.Models;

public class SchedulingPackage
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public string Category { get; set; }

    public string SourceApi { get; set; }

    public virtual ICollection<SchedulingPackageItem> Items { get; set; }

    public SchedulingPackage() { }

    public SchedulingPackage(string name)
    {
        Name = name;
    }
}

