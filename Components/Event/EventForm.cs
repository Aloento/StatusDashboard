﻿namespace StatusDashboard.Components.Event;

using System.ComponentModel.DataAnnotations;

public class EventForm {
    [Required]
    public int EventId { get; set; }

    [Required]
    [StringLength(maximumLength: 200, MinimumLength = 8)]
    public string? Title { get; set; }

    [Required]
    public EventType? Type { get; set; }

    [Required]
    [StringLength(maximumLength: 200, MinimumLength = 10)]
    public string? UpdateMsg { get; set; }

    [Required]
    public EventStatus? Status { get; set; }
}
