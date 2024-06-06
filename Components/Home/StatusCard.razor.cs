namespace StatusDashboard.Components.Home;

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Services;

public partial class StatusCard {
    private Region? region;

    [NotNull]
    private StatusContext? db { get; set; }

    [NotNull]
    [Parameter]
    [EditorRequired]
    public Category? Category { get; set; }

    [NotNull]
    [Parameter]
    [EditorRequired]
    public Region? Region {
        get => this.region!;
        set {
            if (this.region == value)
                return;

            this.region = value;
            this.RegionChanged?.Invoke(null, null!);
        }
    }

    [NotNull]
    private ICollection<Service>? services { get; set; }

    public async ValueTask DisposeAsync() {
        await this.db.DisposeAsync();
        this.RegionChanged -= this.onRegionChanged;
    }

    private event PropertyChangedEventHandler? RegionChanged;

    protected override async Task OnInitializedAsync() {
        this.db = await this.context.CreateDbContextAsync();
        this.onRegionChanged();
        this.RegionChanged += this.onRegionChanged;
    }

    private async void onRegionChanged(object? sender = null, PropertyChangedEventArgs? e = null) =>
        this.services = await this.db.Services
            .Where(x => x.Regions.Contains(this.Region))
            .Where(x => x.Category == this.Category)
            .OrderBy(x => x.Name)
            .ToArrayAsync();
}
