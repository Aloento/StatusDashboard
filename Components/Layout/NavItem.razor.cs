namespace StatusDashboard.Components.Layout;

using System.Reflection;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

public partial class NavItem {
    [EditorRequired]
    [Parameter]
    public string? Label { get; set; }

    public string? Href => (string?)this.AdditionalAttributes?.GetValueOrDefault("href");

    public bool IsActive {
        get {
            var parent = typeof(NavLink);
            var filedInf = parent.GetField("_isActive", BindingFlags.NonPublic | BindingFlags.Instance);

            var isActive = (bool)filedInf!.GetValue(this)!;
            return isActive;
        }
    }
}
