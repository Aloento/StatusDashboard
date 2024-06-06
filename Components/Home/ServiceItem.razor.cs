namespace StatusDashboard.Components.Home;

using System.Diagnostics.CodeAnalysis;
using Event;
using Microsoft.AspNetCore.Components;

public partial class ServiceItem {
    [NotNull]
    [Parameter]
    [EditorRequired]
    public EventType? Type { get; set; }

    [NotNull]
    [Parameter]
    [EditorRequired]
    public string? Name { get; set; }
}
