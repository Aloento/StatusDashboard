namespace StatusDashboard.Components.Pages;

using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Services;
using JB = JetBrains.Annotations;

[JB.PublicAPI]
public partial class History {
    private bool isBottom { get; set; }

    private bool isEnd { get; set; }

    private bool loading => this.isBottom && !this.isEnd;

    [NotNull]
    private IAsyncEnumerator<Services.Event>? dbEvents { get; set; }

    private List<Tuple<Services.Event?, Services.Event>> items { get; } = [];

    protected override async Task OnAfterRenderAsync(bool firstRender) {
        if (this.isEnd)
            return;

        if (this.isBottom) {
            var prev = this.dbEvents.Current;
            var has = await this.dbEvents.MoveNextAsync();

            if (!has) {
                this.isEnd = true;
                this.StateHasChanged();
                return;
            }

            this.items.Add(new(prev, this.dbEvents.Current));
            this.StateHasChanged();
        }
    }

    protected override async Task OnInitializedAsync() {
        await base.OnInitializedAsync();

        this.dbEvents = this.db.Events
            .OrderByDescending(x => x.Start)
            .AsAsyncEnumerable()
            .GetAsyncEnumerator();

        await this.dbEvents.MoveNextAsync();
        this.items.Add(new(null, this.dbEvents.Current));
    }

    public override async ValueTask DisposeAsync() {
        await this.dbEvents.DisposeAsync();
        await base.DisposeAsync();
    }
}
