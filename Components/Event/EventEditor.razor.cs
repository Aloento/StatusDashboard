namespace StatusDashboard.Components.Event;

using Microsoft.AspNetCore.Components;

public partial class EventEditor {
    [SupplyParameterFromForm]
    private EventForm form { get; set; } = new();

    private void submit() {
        Console.WriteLine(this.form.Title);
    }
}
