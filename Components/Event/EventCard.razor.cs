namespace StatusDashboard.Components.Event;

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Services;

public partial class EventCard {
    [Parameter]
    [EditorRequired]
    public int Id { get; set; }

    [NotNull]
    private StatusContext? db { get; set; }

    [NotNull]
    private Event? theEvent { get; set; }

    private EventStatus status { get; set; }

    public async ValueTask DisposeAsync() => await this.db.DisposeAsync();

    protected override async Task OnInitializedAsync() {
        this.db = await this.context.CreateDbContextAsync();

        this.theEvent = await this.db.Events
            .Where(x => x.Id == this.Id)
            .SingleAsync();

        this.status = await this.db.Events
            .Where(x => x.Id == this.Id)
            .SelectMany(x => x.Histories)
            .OrderByDescending(x => x.Created)
            .Select(x => x.Status)
            .FirstOrDefaultAsync();
    }
}
