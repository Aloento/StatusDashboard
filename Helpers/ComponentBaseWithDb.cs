namespace StatusDashboard.Helpers;

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Services;

public abstract class ComponentBaseWithDb : ComponentBase, IAsyncDisposable {
    [Inject]
    [NotNull]
    private IDbContextFactory<StatusContext>? context { get; set; }

    [NotNull]
    internal StatusContext? db { get; set; }

    public virtual async ValueTask DisposeAsync() => await this.db.DisposeAsync();

    protected override async Task OnInitializedAsync() =>
        this.db = await this.context.CreateDbContextAsync();
}
