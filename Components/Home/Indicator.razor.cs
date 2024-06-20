namespace StatusDashboard.Components.Home;

using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Event;
using Microsoft.AspNetCore.Components;

public partial class Indicator {
    private const string scaleIcon = "scale-icon-";
    private const string scaleColor = "var(--telekom-color-";
    private const string func = "functional-";
    private const string text = scaleColor + "text-and-icon-" + func;

    [NotNull]
    [Parameter]
    [EditorRequired]
    public EventType? Type { get; set; }

    [Parameter]
    [Range(12, 128)]
    public byte? Size { get; set; }

    [Parameter]
    public string? Class { get; set; }

    private RenderFragment icon {
        get {
            var name = this.Type switch {
                EventType.Maintenance => "service-maintanance",
                EventType.MinorIssue => "action-minus-circle",
                EventType.MajorIssue => "alert-warning",
                EventType.Outage => "action-circle-close",
                _ => "action-success"
            };

            var fillColor = this.Type switch {
                EventType.Maintenance => $"{text}informational)",
                EventType.MinorIssue => $"{scaleColor}{func}warning-standard)",
                EventType.MajorIssue => $"{text}warning)",
                EventType.Outage => $"{text}danger)",
                _ => $"{text}success)"
            };

            return x => {
                x.OpenElement(0, $"{scaleIcon}{name}");
                x.AddAttribute(1, "accessibility-title", this.Type);
                x.AddAttribute(2, "fill", fillColor);
                x.AddAttribute(3, "class", this.Class);

                if (this.Size is not null) 
                    x.AddAttribute(4, "size", this.Size);

                x.CloseElement();
            };
        }
    }
}
