namespace StatusDashboard.Components.Event;

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Services;

public partial class EventCard {
    [Parameter]
    [EditorRequired]
    public int Id { get; set; }

    [NotNull]
    private StatusContext? db { get; set; }

    public async ValueTask DisposeAsync() => await this.db.DisposeAsync();

    protected override async Task OnInitializedAsync() {
        this.db = await this.context.CreateDbContextAsync();
    }
}
