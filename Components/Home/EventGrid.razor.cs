namespace StatusDashboard.Components.Home;

using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.JSInterop;

public partial class EventGrid {
    private readonly byte[] fields = JsonSerializer.SerializeToUtf8Bytes(
        new List<FieldOption> {
            new() { Type = FieldTypes.Number, Label = "ID" },
            new() { Type = FieldTypes.Tags, Label = "Type" },
            new() { Type = FieldTypes.Date, Label = "Started" },
            new() { Type = FieldTypes.Text, Label = "Status / Planned" },
            new() { Type = FieldTypes.Text, Label = "Region", Sortable = true },
            new() { Type = FieldTypes.Text, Label = "Service", Sortable = true, StretchWeight = 0.70f },
            new() { Type = FieldTypes.Actions, Label = "Detail" }
        }, new JsonSerializerOptions {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

    private IJSObjectReference? module;

    protected override async Task OnAfterRenderAsync(bool firstRender) {
        if (firstRender)
            this.module = await this.JS.InvokeAsync<IJSObjectReference>(
                "import",
                $"./{nameof(Components)}/{nameof(Home)}/{nameof(EventGrid)}.razor.js");

        await this.module!.InvokeVoidAsync("setFields", this.fields);

        var rows = new List<object[]> {
            new object[] {
                1,
                new object[] {
                    new {
                        content = "Outage",
                        color = "red"
                    }
                },
                DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm 'UTC'"),
                "Analysing", "EU-DE", "Cloud Trace Service",
                new object[] {
                    new {
                        label = "↗",
                        variant = "secondary",
                        href = "/Event/1"
                    }
                }
            },
            new object[] {
                2,
                new object[] {
                    new {
                        content = "Major",
                        color = "orange"
                    }
                },
                DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm 'UTC'"),
                "Fixing", "EU-NL", "Elastic Cloud Server",
                new object[] {
                    new {
                        label = "↗",
                        variant = "secondary",
                        href = "/Event/1"
                    }
                }
            },
            new object[] {
                3,
                new object[] {
                    new {
                        content = "Minor",
                        color = "yellow"
                    }
                },
                DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm 'UTC'"),
                "Validating", "EU-DE", "Distributed Cache Service",
                new object[] {
                    new {
                        label = "↗",
                        variant = "secondary",
                        href = "/Event/1"
                    }
                }
            },
            new object[] {
                4,
                new object[] {
                    new {
                        content = "Maintain",
                        color = "cyan"
                    }
                },
                DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm 'UTC'"),
                DateTime.UtcNow.AddDays(1).ToString("MM-dd HH:mm"),
                "EU-DE", "Resource Management Service",
                new object[] {
                    new {
                        label = "↗",
                        variant = "secondary",
                        href = "/Event/1"
                    }
                }
            }
        };

        await this.module!.InvokeVoidAsync("setRows", rows);
    }

    public async ValueTask DisposeAsync() {
        if (this.module is not null)
            await this.module.DisposeAsync();
    }
}
