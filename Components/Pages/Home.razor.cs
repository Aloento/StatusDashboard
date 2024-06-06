namespace StatusDashboard.Components.Pages;

using Services;

public partial class Home {
    private StatusContext db { get; set; }

    protected override async Task OnInitializedAsync() {
        this.db = await this.context.CreateDbContextAsync();
    }

    public async ValueTask DisposeAsync() {
        await this.db.DisposeAsync();
    }
}
