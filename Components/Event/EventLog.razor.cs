namespace StatusDashboard.Components.Event;

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Services;

public partial class EventLog {
    [Parameter]
    [EditorRequired]
    public int Id { get; set; }

    [NotNull]
    private StatusContext? db { get; set; }

    [NotNull]
    private History[]? list { get; set; }

    public async ValueTask DisposeAsync() => await this.db.DisposeAsync();

    protected override async Task OnInitializedAsync() {
        this.db = await this.context.CreateDbContextAsync();

        this.list = await this.db.Events
            .Where(x => x.Id == this.Id)
            .SelectMany(x => x.Histories)
            .OrderByDescending(x => x.Created)
            .ToArrayAsync();
    }
}
