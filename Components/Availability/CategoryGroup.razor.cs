namespace StatusDashboard.Components.Availability;

using System.Diagnostics.CodeAnalysis;
using Services;

public partial class CategoryGroup {
    [NotNull]
    private StatusContext? db { get; set; }

    public async ValueTask DisposeAsync() => await this.db.DisposeAsync();

    protected override async Task OnInitializedAsync() {
        this.db = await this.context.CreateDbContextAsync();
    }
}
