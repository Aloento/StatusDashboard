namespace StatusDashboard.Components.Home;

using System.Diagnostics.CodeAnalysis;
using Event;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Services;

public partial class ServiceItem {
    [NotNull]
    private StatusContext? db { get; set; }

    [NotNull]
    [Parameter]
    [EditorRequired]
    public RegionService? RegionService { get; set; }

    private EventType status { get; set; }

    public async ValueTask DisposeAsync() => await this.db.DisposeAsync();

    protected override async Task OnInitializedAsync() {
        this.db = await this.context.CreateDbContextAsync();
        this.status = await this.db.RegionService
            .Where(x => x.Id == this.RegionService.Id)
            .SelectMany(x => x.Events)
            .Select(x => new { x.Type, x.Histories.OrderByDescending(h => h.Created).First().Status })
            .Where(x => x.Status != EventStatus.Completed && x.Status != EventStatus.Resolved)
            .OrderByDescending(x => x.Type)
            .Select(x => x.Type)
            .FirstOrDefaultAsync();
    }
}
