namespace StatusDashboard.Components.Home;

using Microsoft.AspNetCore.Components;

public partial class Indicator {
    [Parameter] public StatusType Type { get; set; }

    private string fillColor {
        get {
            const string pref = "var(--telekom-color-";
            const string func = "functional-";
            const string text = pref + "text-and-icon-" + func;

            return this.Type switch {
                StatusType.Maintenance => $"{text}informational)",
                StatusType.MinorIssue => $"{pref}{func}warning-standard)",
                StatusType.MajorIssue => $"{text}warning)",
                StatusType.Outage => $"{text}danger)",
                _ => $"{text}success)"
            };
        }
    }

    private RenderFragment icon {
        get {
            const string pref = "scale-icon-";

            var name = this.Type switch {
                StatusType.Maintenance => "service-maintanance",
                StatusType.MinorIssue => "action-minus-circle",
                StatusType.MajorIssue => "alert-warning",
                StatusType.Outage => "action-circle-close",
                _ => "action-success"
            };

            return x => {
                x.OpenElement(0, $"{pref}{name}");
                x.AddAttribute(1, "accessibility-title", Enum.GetName(this.Type));
                x.AddAttribute(2, "fill", this.fillColor);
                x.CloseElement();
            };
        }
    }
}
