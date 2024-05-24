namespace StatusDashboard.Components.Home;

using Microsoft.AspNetCore.Components;

public partial class RegionSelector {
    [Parameter]
    [EditorRequired]
    public string? Title { get; set; }
}
