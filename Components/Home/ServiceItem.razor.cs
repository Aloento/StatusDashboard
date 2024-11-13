namespace StatusDashboard.Components.Home;

using System.Diagnostics.CodeAnalysis;
using Event;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Services;

public partial class ServiceItem {
    [NotNull]
    [Parameter]
    [EditorRequired]
    public RegionService? RegionService { get; set; }

    private EventType type { get; set; }

    private bool future { get; set; }

    private int? id { get; set; }

    protected override async Task OnParametersSetAsync() {
        var res = await this.db.EventRegionService
            .Where(x => x.RegionServiceId == this.RegionService.Id)
            .Select(x => x.Event)
            .Where(x =>
                (x.Type == EventType.Maintenance || x.End == null) &&
                x.Status != EventStatus.Completed &&
                x.Status != EventStatus.Resolved &&
                x.Status != EventStatus.Cancelled)
            .OrderByDescending(x => x.Type)
            .FirstOrDefaultAsync();

        this.id = res?.Id;
        this.type = res?.Type ?? default;
        this.future = res?.Start.ToUniversalTime() > DateTime.UtcNow;
    }
}
