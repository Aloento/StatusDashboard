﻿namespace StatusDashboard.Components.Pages;

using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Services;
using JB = JetBrains.Annotations;

[JB.PublicAPI]
public partial class Home {
    [NotNull]
    private StatusContext? db { get; set; }

    [NotNull]
    private Region? currentRegion { get; set; }

    [NotNull]
    private ICollection<Category>? categories { get; set; }

    private int abnormalCount { get; set; }

    [NotNull]
    private string? heading { get; set; }

    public async ValueTask DisposeAsync() => await this.db.DisposeAsync();

    protected override async Task OnInitializedAsync() {
        this.db = await this.context.CreateDbContextAsync();
        this.currentRegion = await this.db.Regions.FirstAsync();

        await this.updateCategory();

        this.abnormalCount = await this.db.Events
            .Where(x => x.End == null)
            .SelectMany(x => x.RegionServices)
            .Select(x => x.Service)
            .Distinct()
            .CountAsync();

        this.heading = this.abnormalCount > 0
            ? $"{this.abnormalCount} components have issue, but don't worry, we are working on it."
            : "All Systems Operational";
    }

    private async Task updateCategory() =>
        this.categories = await this.db.RegionService
            .Include(x => x.Service.Category)
            .Where(x => x.Region == this.currentRegion)
            .Select(x => x.Service.Category)
            .Distinct()
            .OrderBy(x => x.Name)
            .ToArrayAsync();

    private async void onClick(Region r) {
        this.currentRegion = r;
        await this.updateCategory();
        this.StateHasChanged();
    }
}
