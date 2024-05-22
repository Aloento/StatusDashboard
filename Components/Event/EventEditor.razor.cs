namespace StatusDashboard.Components.Event;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

public partial class EventEditor {
    private IJSObjectReference? module;

    [SupplyParameterFromForm] private EventForm model { get; set; } = new();

    public async ValueTask DisposeAsync() {
        if (this.module is not null)
            await this.module.DisposeAsync();
    }

    private void openModal() => this.module?.InvokeVoidAsync("openModal");

    protected override async Task OnAfterRenderAsync(bool firstRender) {
        if (firstRender)
            this.module = await this.JS.InvokeAsync<IJSObjectReference>(
                "import",
                $"./{nameof(Components)}/{nameof(Event)}/{nameof(EventEditor)}.razor.js");
    }

    private void submit() {
        Console.WriteLine(this.model.Title);
    }
}
