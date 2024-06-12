namespace StatusDashboard.Components.History;

using System.Diagnostics.CodeAnalysis;
using Event;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using Services;

public partial class EventItem {
    [NotNull]
    private StatusContext? db { get; set; }

    private ElementReference? labelElement { get; set; }

    [NotNull]
    [Parameter]
    [EditorRequired]
    public Tuple<Event?, Event>? Item { get; set; }

    private Event? prev => this.Item.Item1;

    private Event curr => this.Item.Item2;

    private bool isBegin {
        get {
            if (this.prev is null)
                return true;

            return this.prev.Start.Month != this.curr.Start.Month;
        }
    }

    private EventStatus status { get; set; }

    [NotNull]
    private Service[]? services { get; set; }

    [NotNull]
    private string? servicesTxt { get; set; }

    [NotNull]
    private string[]? regions { get; set; }

    [NotNull]
    private string? regionsTxt { get; set; }

    [NotNull]
    private string? color { get; set; }

    protected override async Task OnInitializedAsync() {
        this.db = await this.context.CreateDbContextAsync();

        this.status = await this.db.Histories
            .Where(x => x.Event == this.curr)
            .Where(x => x.Status != EventStatus.SysInfo)
            .OrderByDescending(x => x.Created)
            .Select(x => x.Status)
            .FirstOrDefaultAsync();

        this.services = await this.db.RegionService
            .Where(x => x.Events.Contains(this.curr))
            .Select(x => x.Service)
            .Distinct()
            .ToArrayAsync();

        var upper = this.services
            .Select(x => new { x.Name, Abbr = x.Abbr.ToUpperInvariant() })
            .ToArray();

        this.servicesTxt = upper.Length > 3
            ? string.Join(", ", upper.Take(3).Select(x => x.Abbr)) + $" (+{upper.Length - 3})"
            : string.Join(", ", upper.Select(x => x.Abbr));

        this.regions = this.db.RegionService
            .Where(x => x.Events.Contains(this.curr))
            .Select(x => x.Region.Name)
            .Distinct()
            .ToArray();

        this.regionsTxt = this.regions.Length > 2
            ? string.Join(", ", this.regions.Take(2)) + $" (+{this.regions.Length - 2})"
            : string.Join(", ", this.regions);

        this.color = this.status switch {
            EventStatus.Investigating or EventStatus.Fixing or EventStatus.Monitoring => "yellow",
            EventStatus.Scheduled or EventStatus.Performing => "violet",
            EventStatus.Resolved or EventStatus.Completed => "green",
            _ => "standard"
        };
    }

    protected override async Task OnAfterRenderAsync(bool firstRender) {
        if (firstRender && this.labelElement is not null) {
            await using var module = await this.JS.InvokeAsync<IJSObjectReference>(
                "import",
                $"./{nameof(Components)}/{nameof(Components.History)}/{nameof(EventItem)}.razor.js");

            await module.InvokeVoidAsync("onLabel", this.labelElement);
        }
    }

    public async ValueTask DisposeAsync() => await this.db.DisposeAsync();
}
