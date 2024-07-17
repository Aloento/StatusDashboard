namespace StatusDashboard.Components.Pages;

using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Services;
using JB = JetBrains.Annotations;

[JB.PublicAPI]
public partial class Home {
    [NotNull]
    private Region? currentRegion { get; set; }

    [NotNull]
    private Category[]? categories { get; set; }

    private int abnormalCount { get; set; }

    [NotNull]
    private string? heading { get; set; }

    protected override async Task OnInitializedAsync() {
        await base.OnInitializedAsync();

        this.currentRegion = await this.db.Regions.FirstAsync();
        await this.getCategory();

        this.abnormalCount = await this.db.Events
            .Where(x => x.End == null)
            .SelectMany(x => x.RegionServices)
            .Select(x => x.Service)
            .Distinct()
            .CountAsync();

        this.heading = this.abnormalCount > 0
            ? $"{this.abnormalCount} components have issue, but don't worry, we are working on it."
            : "All Systems Operational";
    }

    private async Task getCategory() =>
        this.categories = await this.db.RegionService
            .Where(x => x.Region == this.currentRegion)
            .Select(x => x.Service.Category)
            .Distinct()
            .OrderBy(x => x.Name)
            .ToArrayAsync();

    private async void onClick(Region r) {
        this.currentRegion = r;
        await this.getCategory();
        this.StateHasChanged();
    }
}
