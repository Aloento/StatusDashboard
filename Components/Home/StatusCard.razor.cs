namespace StatusDashboard.Components.Home;

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Services;

public partial class StatusCard {
    [NotNull]
    private StatusContext? db { get; set; }

    [NotNull]
    [Parameter]
    [EditorRequired]
    public Category? Category { get; set; }

    [NotNull]
    [CascadingParameter]
    public Region? Region { get; set; }

    [NotNull]
    private RegionService[]? services { get; set; }

    public async ValueTask DisposeAsync() => await this.db.DisposeAsync();

    protected override async Task OnInitializedAsync() => this.db = await this.context.CreateDbContextAsync();

    protected override async Task OnParametersSetAsync() =>
        this.services = await this.db.RegionService
            .Where(x => x.Region == this.Region)
            .Where(x => x.Service.Category == this.Category)
            .OrderBy(x => x.Service.Name)
            .Include(x => x.Service)
            .ToArrayAsync();
}
