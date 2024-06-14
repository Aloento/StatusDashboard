namespace StatusDashboard.Components.Pages;

using System.Diagnostics.CodeAnalysis;
using Services;

public partial class Availability {
    [NotNull]
    private Region? currentRegion { get; set; }

    private void onClick(Region r) {
        this.currentRegion = r;
        this.StateHasChanged();
    }
}
