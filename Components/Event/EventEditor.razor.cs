﻿namespace StatusDashboard.Components.Event;

using Microsoft.JSInterop;

public partial class EventEditor {
    private IJSObjectReference? module;

    private void openModal() {
        this.module?.InvokeVoidAsync("openModal");
    }

    protected override async Task OnAfterRenderAsync(bool firstRender) {
        if (firstRender)
            this.module = await this.JS.InvokeAsync<IJSObjectReference>(
                "import",
                $"./{nameof(Components)}/{nameof(Event)}/{nameof(EventEditor)}.razor.js");
    }

    public async ValueTask DisposeAsync() {
        if (this.module is not null)
            await this.module.DisposeAsync();
    }
}
