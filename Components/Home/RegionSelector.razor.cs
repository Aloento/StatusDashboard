namespace StatusDashboard.Components.Home;

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using Services;

public partial class RegionSelector {
    [NotNull]
    [Parameter]
    [EditorRequired]
    public string? Title { get; set; }

    [NotNull]
    [Parameter]
    [EditorRequired]
    public Action<Region>? OnClick { get; set; }

    [NotNull]
    private StatusContext? db { get; set; }

    [NotNull]
    private ICollection<Region>? regions { get; set; }

    [NotNull]
    private DotNetObjectReference<RegionSelector>? objRef { get; set; }

    protected override async Task OnInitializedAsync() {
        this.db = await this.context.CreateDbContextAsync();
        this.regions = await this.db.Regions.ToArrayAsync();
        this.objRef = DotNetObjectReference.Create(this);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender) {
        if (firstRender)
            await this.JS.InvokeVoidAsync("injectRS", this.objRef, nameof(this.OnTabClick));
    }

    [JSInvokable]
    public void OnTabClick(string region) {
        var res = this.regions.Single(x => x.Name == region);
        this.OnClick(res);
    }

    public async ValueTask DisposeAsync() {
        await this.db.DisposeAsync();
        this.objRef.Dispose();
    }
}
