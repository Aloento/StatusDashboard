namespace StatusDashboard.Components.Event;

using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Helpers;

public class EventForm {
    [NotNull]
    [Required]
    [StringLength(maximumLength: 200, MinimumLength = 8)]
    public string? Title { get; set; }

    [Required]
    public EventType Type { get; set; } = EventType.Maintenance;

    [NotNull]
    [Required]
    [StringLength(maximumLength: 200, MinimumLength = 10)]
    public string? UpdateMsg { get; set; }

    [Required]
    public EventStatus Status { get; set; }

    public DateTime Now => DateTime.Now;

    [DateTimeBind(nameof(Now))]
    public DateTime? End { get; set; }
}
