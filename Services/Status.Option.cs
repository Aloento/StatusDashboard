namespace StatusDashboard.Services;

using System.ComponentModel.DataAnnotations;

public class StatusOption {
    [Url]
    [Required]
    [MinLength(7)]
    public required string Source { get; set; }
}
