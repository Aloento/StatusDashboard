namespace StatusDashboard.Components.CurrentEvent;

using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.JSInterop;

public partial class DataGrid {
    private readonly string fields = JsonSerializer.Serialize(
        new List<FieldOption> {
            new() { Type = "number", Label = "ID" },
            new() { Type = "text", Label = "Name", Sortable = true },
            new() { Type = "date", Label = "Time", StretchWeight = 1 }
        }, new JsonSerializerOptions {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

    protected override async Task OnAfterRenderAsync(bool firstRender) {
        if (firstRender)
            await this.jsRuntime.InvokeVoidAsync($"{nameof(CurrentEvent)}.setFields", this.fields);
    }
}
