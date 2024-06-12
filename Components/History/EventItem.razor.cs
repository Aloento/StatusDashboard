namespace StatusDashboard.Components.History;

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Services;

public partial class EventItem {
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

    protected override async Task OnAfterRenderAsync(bool firstRender) {
        if (firstRender && this.labelElement is not null) {
            await using var module = await this.JS.InvokeAsync<IJSObjectReference>(
                "import",
                $"./{nameof(Components)}/{nameof(Components.History)}/{nameof(EventItem)}.razor.js");

            await module.InvokeVoidAsync("onLabel", this.labelElement);
        }
    }
}
