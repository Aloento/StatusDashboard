namespace StatusDashboard.Components.New;

using System.ComponentModel.DataAnnotations;
using Event;

public class NewModel {
    [Required]
    [StringLength(maximumLength: 200, MinimumLength = 8)]
    public string? Title { get; set; }

    [Required]
    public EventType? Type { get; set; }

    [StringLength(maximumLength: 200, MinimumLength = 10)]
    public string? Description { get; set; }

    public dynamic? Services { get; set; }

    public DateTime? Start { get; set; }

    public DateTime? End { get; set; }
}
