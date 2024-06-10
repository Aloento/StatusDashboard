namespace StatusDashboard.Components.Home;

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using Event;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using Services;

public partial class EventGrid {
    private readonly byte[] fields = JsonSerializer.SerializeToUtf8Bytes(
        new List<FieldOption> {
            new() { Type = FieldTypes.Number, Label = "ID" },
            new() { Type = FieldTypes.Tags, Label = "Type" },
            new() { Type = FieldTypes.Date, Label = "Start" },
            new() { Type = FieldTypes.Text, Label = "Status / Plan" },
            new() { Type = FieldTypes.Text, Label = "Region", Sortable = true },
            new() { Type = FieldTypes.Text, Label = "Service", Sortable = true, StretchWeight = 0.70f },
            new() { Type = FieldTypes.Actions, Label = "Detail" }
        }, new JsonSerializerOptions {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

    private IJSObjectReference? module;

    [NotNull]
    private StatusContext? db { get; set; }

    public async ValueTask DisposeAsync() {
        await this.db.DisposeAsync();

        if (this.module is not null)
            await this.module.DisposeAsync();
    }

    protected override async Task OnInitializedAsync() => this.db = await this.context.CreateDbContextAsync();

    protected override async Task OnAfterRenderAsync(bool firstRender) {
        if (firstRender)
            this.module = await this.JS.InvokeAsync<IJSObjectReference>(
                "import",
                $"./{nameof(Components)}/{nameof(Home)}/{nameof(EventGrid)}.razor.js");

        await this.module!.InvokeVoidAsync("setFields", this.fields);

        var events = await this.db.Events
            .Select(x => new {
                x.Id, x.Type,
                x.Start, x.End,
                Services = x.RegionServices.Select(s => s.Service.Name).ToArray(),
                Regions = x.RegionServices.Select(r => r.Region.Name).ToArray(),
                Latest = x.Histories.OrderByDescending(e => e.Created).FirstOrDefault()
            })
            .Where(x => x.Latest!.Status != EventStatus.Completed && x.Latest.Status != EventStatus.Resolved)
            .OrderByDescending(x => x.Start)
            .ToArrayAsync();

        var rows = events.Select(x => {
            var tag = x.Type switch {
                EventType.MinorIssue => new {
                    content = "Minor",
                    color = "yellow"
                },
                EventType.MajorIssue => new {
                    content = "Major",
                    color = "orange"
                },
                EventType.Outage => new {
                    content = "Outage",
                    color = "red"
                },
                _ => new {
                    content = "Maintain",
                    color = "cyan"
                }
            };

            return new object[] {
                x.Id,
                new object[] { tag },
                x.Start.ToUniversalTime().ToString("yyyy-MM-dd HH:mm 'UTC'"),
                x.End.HasValue
                    ? x.End.Value.ToUniversalTime().ToString("MM-dd HH:mm")
                    : (x.Latest?.Status ?? default).ToString(),
                x.Regions.Length > 1
                    ? $"{x.Regions[0]} +{x.Regions.Length - 1}"
                    : x.Regions[0],
                x.Services.Length > 1
                    ? $"{x.Services[0]} +{x.Services.Length - 1}"
                    : x.Services[0],
                new object[] {
                    new {
                        label = "↗",
                        variant = "secondary",
                        href = $"/Event/{x.Id}"
                    }
                }
            };
        });

        await this.module!.InvokeVoidAsync("setRows", rows);
    }
}
