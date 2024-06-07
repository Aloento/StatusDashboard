namespace StatusDashboard.Components.Home;

using System.Diagnostics.CodeAnalysis;
using Event;
using Microsoft.AspNetCore.Components;
using Services;

public partial class ServiceItem {
    [NotNull]
    private StatusContext? db { get; set; }

    [NotNull]
    [Parameter]
    [EditorRequired]
    public RegionService? RegionService { get; set; }

    private EventType status => this.db.RegionService
        .Where(x => x.Id == this.RegionService.Id)
        .SelectMany(x => x.Events)
        .Where(x => x.End == null)
        .OrderByDescending(x => x.Type)
        .Select(x => x.Type)
        .FirstOrDefault();

    public async ValueTask DisposeAsync() => await this.db.DisposeAsync();

    protected override async Task OnInitializedAsync() => this.db = await this.context.CreateDbContextAsync();
}
