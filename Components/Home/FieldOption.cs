namespace StatusDashboard.Components.Home;

public record FieldOption {
    /// <remarks>
    /// <see cref="FieldTypes"/>.
    /// </remarks>
    public required string Type { get; set; }

    public string? Label { get; set; }

    public ushort? MaxWidth { get; set; }

    public ushort? MinWidth { get; set; }

    public bool? MobileTitle { get; set; }

    public bool? Resizable { get; set; }

    public bool? Sortable { get; set; }

    /// <remarks>
    /// <see cref="Home.SortBy"/>.
    /// </remarks>
    public string? SortBy { get; set; }

    /// <remarks>
    /// <see cref="Home.TextAlign"/>.
    /// </remarks>
    public string? TextAlign { get; set; }

    public float? StretchWeight { get; set; }

    public bool? Visible { get; set; }

    public ushort? Width { get; set; }
}
