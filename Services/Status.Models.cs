#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
namespace StatusDashboard.Services;

using System.ComponentModel.DataAnnotations;
using Components.Event;

internal class Service {
    public int Id { get; set; }

    [StringLength(byte.MaxValue, MinimumLength = 1)]
    public string Name { get; set; }

    public Category Category { get; set; }

    public Region Region { get; set; }

    public ICollection<Event> Events { get; set; }
}

internal class Category {
    public int Id { get; set; }

    [StringLength(byte.MaxValue, MinimumLength = 1)]
    public string Name { get; set; }

    public ICollection<Service> Services { get; set; }
}

internal class Region {
    public int Id { get; set; }

    [StringLength(byte.MaxValue, MinimumLength = 1)]
    public string Name { get; set; }

    public ICollection<Service> Services { get; set; }
}

internal class Event {
    public int Id { get; set; }

    [StringLength(200, MinimumLength = 8)]
    public string Title { get; set; }

    public EventType Type { get; set; }

    public DateTime Start { get; set; }

    public DateTime? End { get; set; }

    public ICollection<History> Histories { get; set; }
}

internal class History {
    public int Id { get; set; }

    [StringLength(200, MinimumLength = 10)]
    public string Message { get; set; }

    public DateTime Created { get; set; }

    public Event Event { get; set; }
}
