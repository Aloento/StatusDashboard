namespace StatusDashboard.Services;

internal class ServiceModel {
    public int Id { get; set; }

    public required string Name { get; set; }

    public required CategoryModel Category { get; set; }

    public required RegionModel Region { get; set; }

    public required List<EventModel> Events { get; set; }
}

internal class CategoryModel {
    public int Id { get; set; }

    public required string Name { get; set; }

    public required List<ServiceModel> Services { get; set; }
}

internal class RegionModel {
    public int Id { get; set; }

    public required string Name { get; set; }

    public required List<ServiceModel> Services { get; set; }
}

internal class EventModel {
    public int Id { get; set; }

    public required string Title { get; set; }
}

internal class HistoryModel {

}
