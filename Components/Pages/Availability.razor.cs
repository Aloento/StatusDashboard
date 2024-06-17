namespace StatusDashboard.Components.Pages;

using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Services;
using JB = JetBrains.Annotations;

[JB.PublicAPI]
public partial class Availability {
    [NotNull]
    private Region? currentRegion { get; set; }

    protected override async Task OnInitializedAsync() {
        await using var db = await this.context.CreateDbContextAsync();
        this.currentRegion = await db.Regions.FirstAsync();
    }

    private void onClick(Region r) {
        this.currentRegion = r;
        this.StateHasChanged();
    }
}
