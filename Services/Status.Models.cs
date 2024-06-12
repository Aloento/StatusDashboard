#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
namespace StatusDashboard.Services;

using System.ComponentModel.DataAnnotations;
using Components.Event;
using Microsoft.EntityFrameworkCore;

[Index(nameof(Name), IsUnique = true)]
public class Service {
    public int Id { get; set; }

    [StringLength(byte.MaxValue, MinimumLength = 1)]
    public string Name { get; set; }

    [StringLength(10, MinimumLength = 1)]
    public string Abbr { get; set; }

    public Category Category { get; set; }

    public ICollection<Region> Regions { get; set; }
}

[Index(nameof(Name), IsUnique = true)]
public class Category {
    public int Id { get; set; }

    [StringLength(byte.MaxValue, MinimumLength = 1)]
    public string Name { get; set; }

    public ICollection<Service> Services { get; set; }
}

[Index(nameof(Name), IsUnique = true)]
public class Region {
    public int Id { get; set; }

    [StringLength(byte.MaxValue, MinimumLength = 1)]
    public string Name { get; set; }

    public ICollection<Service> Services { get; set; }
}

public class RegionService {
    public int Id { get; set; }

    public int RegionId { get; set; }

    public Region Region { get; set; }

    public int ServiceId { get; set; }

    public Service Service { get; set; }

    public ICollection<Event> Events { get; set; }
}

public class Event {
    public int Id { get; set; }

    [StringLength(200, MinimumLength = 8)]
    public string Title { get; set; }

    public EventType Type { get; set; }

    public DateTime Start { get; set; }

    public DateTime? End { get; set; }

    public ICollection<RegionService> RegionServices { get; set; }

    public ICollection<History> Histories { get; set; }
}

public class EventRegionService {
    public int Id { get; set; }

    public int EventId { get; set; }

    public Event Event { get; set; }

    public int RegionServiceId { get; set; }

    public RegionService RegionService { get; set; }
}

public class History {
    public int Id { get; set; }

    [StringLength(200, MinimumLength = 10)]
    public string Message { get; set; }

    public DateTime Created { get; set; }

    public EventStatus Status { get; set; }

    public Event Event { get; set; }
}
