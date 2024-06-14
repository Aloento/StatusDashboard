namespace StatusDashboard.Components.Event;

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Services;

public partial class EventAffected {
    [CascadingParameter]
    public int Id { get; set; }

    [NotNull]
    private StatusContext? db { get; set; }

    private Dictionary<string, HashSet<string>> list { get; } = new();

    public async ValueTask DisposeAsync() => await this.db.DisposeAsync();

    protected override async Task OnInitializedAsync() {
        this.db = await this.context.CreateDbContextAsync();
        
        var all = this.db.Events
            .Where(x => x.Id == this.Id)
            .SelectMany(x => x.RegionServices)
            .Include(x => x.Service)
            .Include(x => x.Region)
            .AsAsyncEnumerable();

        await foreach (var item in all) {
            var service = item.Service.Name;
            var region = item.Region.Name;

            if (!this.list.TryGetValue(service, out var value))
                this.list[service] = [region];
            else
                value.Add(region);
        }
    }
}
