namespace StatusDashboard.Components.CurrentEvent;

using System.Text.Json;
using System.Text.Json.Serialization;
using Home;
using Microsoft.JSInterop;

public partial class DataGrid {
    private readonly byte[] fields = JsonSerializer.SerializeToUtf8Bytes(
        new List<FieldOption> {
            new() { Type = FieldTypes.Number, Label = "ID" },
            new() { Type = FieldTypes.Tags, Label = "Type" },
            new() { Type = FieldTypes.Text, Label = "Name", Sortable = true },
            new() { Type = FieldTypes.Date, Label = "Time", StretchWeight = 1 }
        }, new JsonSerializerOptions {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

    private IJSObjectReference? module;

    protected override async Task OnAfterRenderAsync(bool firstRender) {
        if (firstRender) {
            this.module = await this.JS.InvokeAsync<IJSObjectReference>(
                "import",
                $"./{nameof(Components)}/{nameof(CurrentEvent)}/{nameof(DataGrid)}.razor.js");

            await this.module.InvokeVoidAsync("setFields", this.fields);

            var rows = new List<object[]> {
                new object[] {
                    1,
                    new object[] {
                        new {
                            content = "Outage",
                            color = "red"
                        }
                    },
                    Enum.GetName(StatusType.Outage)!, DateTime.Now
                },
                new object[] {
                    2,
                    new object[] {
                        new {
                            content = "Major",
                            color = "orange"
                        }
                    },
                    Enum.GetName(StatusType.MajorIssue)!, DateTime.Now
                },
                new object[] {
                    3,
                    new object[] {
                        new {
                            content = "Minor",
                            color = "yellow"
                        }
                    },
                    Enum.GetName(StatusType.MinorIssue)!, DateTime.Now
                }
            };

            await this.module.InvokeVoidAsync("setRows", rows);
        }
    }
}
