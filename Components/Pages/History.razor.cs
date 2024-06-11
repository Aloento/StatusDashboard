namespace StatusDashboard.Components.Pages;

using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Services;
using JB = JetBrains.Annotations;

[JB.PublicAPI]
public partial class History {
    [NotNull]
    private StatusContext? db { get; set; }

    private bool isBottom { get; set; }

    private bool isEnd { get; set; }

    private bool loading => this.isBottom && !this.isEnd;

    [NotNull]
    private IAsyncEnumerator<Services.Event>? dbEvents { get; set; }

    private List<Tuple<Services.Event?, Services.Event>> items { get; } = [];

    protected override async Task OnAfterRenderAsync(bool firstRender) {
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
        this.db = await this.context.CreateDbContextAsync();

        this.dbEvents = this.db.Events
            .OrderByDescending(x => x.Start)
            .Include(x => x.RegionServices)
            .ThenInclude(x => x.Region)
            .Include(x => x.RegionServices)
            .ThenInclude(x => x.Service)
            .ThenInclude(x => x.Category)
            .AsAsyncEnumerable()
            .GetAsyncEnumerator();

        await this.dbEvents.MoveNextAsync();
        this.items.Add(new(null, this.dbEvents.Current));
    }

    public async ValueTask DisposeAsync() {
        await this.dbEvents.DisposeAsync();
        await this.db.DisposeAsync();
    }
}
