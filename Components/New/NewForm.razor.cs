namespace StatusDashboard.Components.New;

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

public partial class NewForm {
    [SupplyParameterFromForm]
    private NewModel model { get; set; } = new();

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

    private async void submit() {
        var regions = this.items
            .SelectMany(x => x.Regions, (service, region) => new { service.Id, region })
            .Where(x => x.region.Selected)
            .ToArray();

        var targets = await this.db.RegionService
            .Where(rs => regions
                .Any(sr => 
                    sr.Id == rs.ServiceId &&
                    sr.region.Id == rs.RegionId))
            .ToArrayAsync();
    }
}
