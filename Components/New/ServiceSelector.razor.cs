namespace StatusDashboard.Components.New;

using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

public partial class ServiceSelector {
    [NotNull]
    private ServiceItem[]? items { get; set; }

    protected override async Task OnInitializedAsync() {
        await base.OnInitializedAsync();

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
