namespace StatusDashboard.Components.Pages;

using System.Diagnostics.CodeAnalysis;
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
