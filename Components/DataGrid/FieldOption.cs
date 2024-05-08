namespace StatusDashboard.Components.DataGrid;

using System.Text.Json.Serialization;

public record FieldOption {
    /// <remarks>
    /// <see cref="FieldTypes"/>.
    /// </remarks>
    [JsonPropertyName("type")]
    public required string Type { get; set; }

    [JsonPropertyName("label")]
    public string? Label { get; set; }

    [JsonPropertyName("maxWidth")]
    public ushort? MaxWidth { get; set; }

    [JsonPropertyName("minWidth")]
    public ushort? MinWidth { get; set; }

    [JsonPropertyName("mobileTitle")]
    public bool? MobileTitle { get; set; } = false;

    [JsonPropertyName("resizable")]
    public bool? Resizable { get; set; } = true;

    [JsonPropertyName("sortable")]
    public bool? Sortable { get; set; } = false;

    /// <remarks>
    /// <see cref="DataGrid.SortBy"/>.
    /// </remarks>
    [JsonPropertyName("sortBy")]
    public string? SortBy { get; set; }

    /// <remarks>
    /// <see cref="DataGrid.TextAlign"/>.
    /// </remarks>
    [JsonPropertyName("textAlign")]
    public string? TextAlign { get; set; }

    [JsonPropertyName("stretchWeight")]
    public byte? StretchWeight { get; set; }

    [JsonPropertyName("visible")]
    public bool? Visible { get; set; }

    [JsonPropertyName("width")]
    public ushort? Width { get; set; }
}
