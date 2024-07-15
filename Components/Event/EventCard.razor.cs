﻿namespace StatusDashboard.Components.Event;

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Services;

public partial class EventCard {
    [CascadingParameter]
    public int Id { get; set; }

    [NotNull]
    [CascadingParameter]
    public IEventManager<EventEditor>? OnSubmit { get; set; }

    [NotNull]
    private StatusContext? db { get; set; }

    [NotNull]
    private Event? theEvent { get; set; }

    private EventStatus status { get; set; }

    public async ValueTask DisposeAsync() {
        await this.db.DisposeAsync();
        this.OnSubmit.Unsubscribe(this.onSubmit);
    }

    private async void onSubmit() {
        await this.OnParametersSetAsync();
        this.StateHasChanged();
    }

    protected override async Task OnInitializedAsync() {
        this.db = await this.context.CreateDbContextAsync();
        this.OnSubmit.Subscribe(this.onSubmit);
    }

    protected override async Task OnParametersSetAsync() {
        this.theEvent = await this.db.Events
            .Where(x => x.Id == this.Id)
            .SingleAsync();

        this.status = await this.db.Events
            .Where(x => x.Id == this.Id)
            .SelectMany(x => x.Histories)
            .OrderByDescending(x => x.Created)
            .Select(x => x.Status)
            .FirstOrDefaultAsync();
    }
}
