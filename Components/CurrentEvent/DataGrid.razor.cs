namespace StatusDashboard.Components.CurrentEvent;

using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.JSInterop;

public partial class DataGrid {
    private IJSObjectReference? module;

    private readonly byte[] fields = JsonSerializer.SerializeToUtf8Bytes(
        new List<FieldOption> {
            new() { Type = "number", Label = "ID" },
            new() { Type = "text", Label = "Name", Sortable = true },
            new() { Type = "date", Label = "Time", StretchWeight = 1 }
        }, new JsonSerializerOptions {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

    protected override async Task OnAfterRenderAsync(bool firstRender) {
        if (firstRender) {
            this.module = await this.JS.InvokeAsync<IJSObjectReference>(
                "import",
                $"./{nameof(Components)}/{nameof(CurrentEvent)}/{nameof(DataGrid)}.razor.js");

            await this.module.InvokeVoidAsync("setFields", this.fields);
        }
    }
}
