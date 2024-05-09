namespace StatusDashboard.Components.CurrentEvent;

using System.Runtime.InteropServices.JavaScript;
using System.Text.Json;
using System.Text.Json.Serialization;

public partial class DataGrid {
    [JSImport("setFields", "DataGrid")]
    private static partial void setFields(string fields);

    private readonly string fields = JsonSerializer.Serialize(
        new List<FieldOption> {
            new() { Type = "number", Label = "ID" },
            new() { Type = "text", Label = "Name", Sortable = true },
            new() { Type = "date", Label = "Time", StretchWeight = 1 }
        }, new JsonSerializerOptions {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

    protected override async Task OnInitializedAsync() {
        await JSHost.ImportAsync("DataGrid", "DataGrid.razor.js");
    }

    protected override async Task OnAfterRenderAsync(bool firstRender) {
        if (firstRender) {
            setFields(this.fields);
        }
    }
}
