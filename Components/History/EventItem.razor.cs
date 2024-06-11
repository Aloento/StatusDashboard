namespace StatusDashboard.Components.History;

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;

public partial class EventItem {
    [NotNull]
    [Parameter]
    [EditorRequired]
    public Tuple<Services.Event?, Services.Event>? Item { get; set; }

    private Services.Event? prev => this.Item.Item1;

    private Services.Event curr => this.Item.Item2;

    private bool isBegin {
        get {
            if (this.prev is null) 
                return true;

            return this.prev.Start.Month != this.curr.Start.Month;
        }
    }
}
