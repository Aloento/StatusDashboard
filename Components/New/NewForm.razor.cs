namespace StatusDashboard.Components.New;

using Microsoft.AspNetCore.Components;

public partial class NewForm {
    [SupplyParameterFromForm]
    private NewModel model { get; set; } = new();

    private void submit() {
        Console.WriteLine(this.model.Title);
    }
}
