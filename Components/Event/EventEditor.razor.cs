namespace StatusDashboard.Components.Event;

using Microsoft.AspNetCore.Components;

public partial class EventEditor {
    [SupplyParameterFromForm]
    private EventModel model { get; set; } = new();

    private void submit() {
        Console.WriteLine(this.model.Title);
    }
}
