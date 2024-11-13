namespace StatusDashboard.Components.Home;

using System.Diagnostics.CodeAnalysis;
using Event;
using Microsoft.AspNetCore.Components;
using Services;

public partial class ServiceItem {
    [NotNull]
    [Parameter]
    [EditorRequired]
    public RegionService? RegionService { get; set; }

    private EventType status { get; set; }

    private bool future { get; set; }

    private int? id { get; set; }

    protected override Task OnParametersSetAsync() {
        var res = this.RegionService.Events
            .Where(x =>
                (x.Type == EventType.Maintenance || x.End == null) &&
                x.Status != EventStatus.Completed &&
                x.Status != EventStatus.Resolved &&
                x.Status != EventStatus.Cancelled)
            .MaxBy(x => x.Type);

        this.id = res?.Id;
        this.status = res?.Type ?? default;
        this.future = res?.Start.ToUniversalTime() > DateTime.UtcNow;

        return Task.CompletedTask;
    }
}
