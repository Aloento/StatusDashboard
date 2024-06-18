namespace StatusDashboard.Components.Availability;

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Services;

public partial class CategoryGroup {
    [NotNull]
    [Parameter]
    [EditorRequired]
    public Category? Category { get; set; }

    [NotNull]
    [CascadingParameter]
    public Region? Region { get; set; }

    [NotNull]
    private StatusContext? db { get; set; }

    [NotNull]
    private RegionService[]? services { get; set; }

    private List<List<double>> slas { get; set; } = [];

    public async ValueTask DisposeAsync() => await this.db.DisposeAsync();

    protected override async Task OnInitializedAsync() {
        this.db = await this.context.CreateDbContextAsync();
    }

    protected override async Task OnParametersSetAsync() {
        this.services = await this.db.RegionService
            .Where(x => x.Region == this.Region)
            .Where(x => x.Service.Category == this.Category)
            .Include(x => x.Service)
            .OrderBy(x => x.Service.Name)
            .ToArrayAsync();

        foreach (var service in this.services) {
            var temp = await this.sla.Calc6Months(service);
            this.slas.Add(temp);
        }
    }

    private string getColor(double val) {
        var color = val switch {
            >= 99.95 => "emerald",
            >= 99 => "amber",
            _ => "rose"
        };

        return $"bg-{color}-100 hover:bg-{color}-200";
    }
}
