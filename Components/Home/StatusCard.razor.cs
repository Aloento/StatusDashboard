namespace StatusDashboard.Components.Home;

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Services;

public partial class StatusCard {
    [NotNull]
    [Parameter]
    [EditorRequired]
    public Category? Category { get; set; }

    [NotNull]
    [CascadingParameter]
    public Region? Region { get; set; }

    [NotNull]
    private RegionService[]? services { get; set; }

    protected override async Task OnParametersSetAsync() =>
        this.services = await this.db.RegionService
            .Where(x => x.Region == this.Region)
            .Where(x => x.Service.Category == this.Category)
            .OrderBy(x => x.Service.Name)
            .Include(x => x.Service)
            .ToArrayAsync();
}
