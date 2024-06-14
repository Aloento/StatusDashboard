namespace StatusDashboard.Components.Event;

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Services;

// TODO
public partial class EventEditor {
    [NotNull]
    [Parameter]
    [EditorRequired]
    public Event? Event { get; set; }

    [NotNull]
    [SupplyParameterFromForm]
    private EventForm? form { get; set; }

    [NotNull]
    private StatusContext? db { get; set; }

    public async ValueTask DisposeAsync() => await this.db.DisposeAsync();

    private void submit() {
        Console.WriteLine(this.form.Title);
    }

    protected override async Task OnInitializedAsync() {
        this.db = await this.context.CreateDbContextAsync();

        this.form = new() {
            Title = this.Event.Title,
            Type = this.Event.Type
        };
    }
}
