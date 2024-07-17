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

    private EventType status { get; set; }

    private bool future { get; set; }

    private int? id { get; set; }

    protected override async Task OnParametersSetAsync() {
        var res = await this.db.RegionService
            .Where(x => x.Id == this.RegionService.Id)
            .SelectMany(x => x.Events)
            .Select(x => new { x.Id, x.Type, x.Start, x.Histories.OrderByDescending(h => h.Created).First().Status })
            .Where(x => 
                x.Status != EventStatus.Completed && 
                x.Status != EventStatus.Resolved &&
                x.Status != EventStatus.Cancelled)
            .OrderByDescending(x => x.Type)
            .Select(x => new { x.Id, x.Type, x.Start })
            .FirstOrDefaultAsync();

        this.id = res?.Id;
        this.status = res?.Type ?? default;
        this.future = res?.Start.ToUniversalTime() > DateTime.UtcNow;
    }
}
