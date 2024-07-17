namespace StatusDashboard.Components.Event;

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Services;

public partial class EventEditor {
    [NotNull]
    [Parameter]
    [EditorRequired]
    public Event? Event { get; set; }

    [NotNull]
    [CascadingParameter]
    public IEventManager<EventEditor>? OnSubmit { get; set; }

    [NotNull]
    [SupplyParameterFromForm]
    private EventForm? form { get; set; }

    private async Task submit() {
        var user = await this.auth.GetAuthenticationStateAsync();
        var str = JsonSerializer.Serialize(this.form);
        this.logger.LogInformation("User: [{0}]\n\tRequest: [{1}]", user.User.Identity!.Name, str);

        this.db.Attach(this.Event);
        this.Event.Title = this.form.Title;
        this.Event.Type = this.form.Type;

        this.db.Histories.Add(new() {
            Event = this.Event,
            Created = DateTime.UtcNow,
            Message = this.form.UpdateMsg,
            Status = this.form.Status
        });

        await this.db.SaveChangesAsync();
        this.closeModal();
        this.OnSubmit.Publish();
    }

    private async void closeModal() =>
        await this.JS.InvokeVoidAsync(nameof(this.closeModal));

    protected override async Task OnInitializedAsync() {
        await base.OnInitializedAsync();

        this.form = new() {
            Title = this.Event.Title,
            Type = this.Event.Type
        };
    }
}
