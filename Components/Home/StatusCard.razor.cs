namespace StatusDashboard.Components.Home;

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Services;

public partial class StatusCard {
    [NotNull]
    private StatusContext? db { get; set; }

    [NotNull]
    [Parameter]
    [EditorRequired]
    public Category? Category { get; set; }

    [NotNull]
    [CascadingParameter]
    public Region? Region { get; set; }

    private ICollection<Service> services => this.db.Services
        .Where(x => x.Regions.Contains(this.Region))
        .Where(x => x.Category == this.Category)
        .OrderBy(x => x.Name)
        .ToArray();

    public async ValueTask DisposeAsync() => await this.db.DisposeAsync();

    protected override async Task OnInitializedAsync() => this.db = await this.context.CreateDbContextAsync();
}
