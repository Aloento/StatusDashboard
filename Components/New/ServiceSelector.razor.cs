namespace StatusDashboard.Components.New;

public partial class ServiceSelector {
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
