namespace StatusDashboard.Components.Home;

using Microsoft.AspNetCore.Components;

public partial class Indicator {
    private const string scaleIcon = "scale-icon-";
    private const string scaleColor = "var(--telekom-color-";
    private const string func = "functional-";
    private const string text = scaleColor + "text-and-icon-" + func;

    [Parameter] public StatusType Type { get; set; }

    [Parameter] public string? Class { get; set; }

    private RenderFragment icon {
        get {
            var name = this.Type switch {
                StatusType.Maintenance => "service-maintanance",
                StatusType.MinorIssue => "action-minus-circle",
                StatusType.MajorIssue => "alert-warning",
                StatusType.Outage => "action-circle-close",
                _ => "action-success"
            };

            var fillColor = this.Type switch {
                StatusType.Maintenance => $"{text}informational)",
                StatusType.MinorIssue => $"{scaleColor}{func}warning-standard)",
                StatusType.MajorIssue => $"{text}warning)",
                StatusType.Outage => $"{text}danger)",
                _ => $"{text}success)"
            };

            return x => {
                x.OpenElement(0, $"{scaleIcon}{name}");
                x.AddAttribute(1, "accessibility-title", Enum.GetName(this.Type));
                x.AddAttribute(2, "fill", fillColor);
                x.AddAttribute(3, "class", this.Class);
                x.CloseElement();
            };
        }
    }
}
