namespace StatusDashboard.Components.History;

using System.Diagnostics.CodeAnalysis;
using Event;
using Microsoft.AspNetCore.Components;
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

    private EventStatus status => this.db.Histories
        .Where(x => x.Event == this.curr)
        .Where(x => x.Status != EventStatus.SysInfo)
        .OrderByDescending(x => x.Created)
        .Select(x => x.Status)
        .FirstOrDefault();

    protected override async Task OnInitializedAsync() => this.db = await this.context.CreateDbContextAsync();

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
