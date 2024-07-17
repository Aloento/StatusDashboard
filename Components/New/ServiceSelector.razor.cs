namespace StatusDashboard.Components.New;

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;

public partial class ServiceSelector {
    [NotNull]
    [Parameter]
    [EditorRequired]
    public IQueryable<ServiceItem>? Items { get; set; }
}

public class ServiceItem {
    public required int Id { get; set; }

    public required string Name { get; set; }

    public required List<RegionItem> Regions { get; set; }
}

public class RegionItem {
    public required int Id { get; set; }

    public required string Name { get; set; }

    public bool Selected { get; set; }
}
