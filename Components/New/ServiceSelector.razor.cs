namespace StatusDashboard.Components.New;

using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Services;

public partial class ServiceSelector {
    [NotNull]
    private StatusContext? db { get; set; }

    [NotNull]
    private ServiceItem[]? items { get; set; }

    public async ValueTask DisposeAsync() => await this.db.DisposeAsync();

    protected override async Task OnInitializedAsync() {
        this.db = await this.context.CreateDbContextAsync();

        this.items = await this.db.Services
            .OrderBy(x => x.Name)
            .Select(x => new ServiceItem {
                Id = x.Id,
                Name = x.Name,
                Regions = x.Regions
                    .Select(r => new RegionItem {
                        Id = r.Id,
                        Name = r.Name
                    }).ToList()
            })
            .ToArrayAsync();
    }
}

public class ServiceItem {
    public required int Id { get; set; }

    public required string Name { get; set; }

    public required List<RegionItem> Regions { get; set; }
}

public class RegionItem {
    public required int Id { get; set; }

    public required string Name { get; set; }

    public bool Selected { get; set; }
}
