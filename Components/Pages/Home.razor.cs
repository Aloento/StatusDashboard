namespace StatusDashboard.Components.Pages;

using System.Diagnostics.CodeAnalysis;
using Components.Event;
using Microsoft.EntityFrameworkCore;
using Services;
using JB = JetBrains.Annotations;

[JB.PublicAPI]
public partial class Home {
    [NotNull]
    private StatusContext? db { get; set; }

    [NotNull]
    private Region? currentRegion { get; set; }

    [NotNull]
    private ICollection<Category>? categories { get; set; }

    private int abnormalCount => this.db.Events
        .Where(x => x.End == null)
        .Count(x => x.Type != EventType.Maintenance);

    private string heading => this.abnormalCount > 0
        ? $"{this.abnormalCount} components have issue, but don't worry, we are working on it."
        : "All Systems Operational";

    public async ValueTask DisposeAsync() => await this.db.DisposeAsync();

    protected override async Task OnInitializedAsync() {
        this.db = await this.context.CreateDbContextAsync();
        this.currentRegion = await this.db.Regions.FirstAsync();

        this.categories = await this.db.RegionService
            .Include(x => x.Service.Category)
            .Where(x => x.Region == this.currentRegion)
            .Select(x => x.Service.Category)
            .Distinct()
            .OrderBy(x => x.Name)
            .ToArrayAsync();
    }

    private void onClick(Region r) {
        this.currentRegion = r;
        this.StateHasChanged();
    }
}
