namespace StatusDashboard.Components.New;

using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Event;
using Helpers;

public class NewModel {
    [NotNull]
    [Required]
    [StringLength(maximumLength: 200, MinimumLength = 8)]
    public string? Title { get; set; }

    [Required]
    public EventType Type { get; set; } = EventType.Maintenance;

    [StringLength(maximumLength: 200, MinimumLength = 10)]
    public string? Description { get; set; }

    [NotNull]
    [Required]
    [DateTimeBind(nameof(End), true)]
    public DateTime? Start { get; set; } = DateTime.Now;

    [DateTimeBind(nameof(Start))]
    public DateTime? End { get; set; }

    [Required(ErrorMessage = "At least one service must be selected.")]
    public object? Services { get; set; }
}
