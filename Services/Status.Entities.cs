namespace StatusDashboard.Services;

using System.Text.Json;
using System.Text.Json.Serialization;

internal class StatusEntity {
    [JsonPropertyName("attributes")]
    public AttributeEntity[] Attributes { get; set; }

    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("incidents")]
    public IncidentEntity[] Incidents { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }
}

internal class AttributeEntity {
    [JsonPropertyName("name")]
    [JsonConverter(typeof(NameEnumConverter))]
    public NameEnum Name { get; set; }

    [JsonPropertyName("value")]
    public string Value { get; set; }
}

internal enum NameEnum {
    Category,
    Region,
    Type
}

internal class NameEnumConverter : JsonConverter<NameEnum> {
    public override NameEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        var value = reader.GetString()?.ToLower();
        return value switch {
            "category" => NameEnum.Category,
            "region" => NameEnum.Region,
            "type" => NameEnum.Type,
            _ => throw new JsonException($"Unknown value: {value}")
        };
    }

    public override void Write(Utf8JsonWriter writer, NameEnum value, JsonSerializerOptions options) => throw new NotImplementedException();
}

internal class IncidentEntity {
    [JsonPropertyName("end_date")]
    [JsonConverter(typeof(UtcConverter))]
    public DateTime? EndDate { get; set; }

    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("impact")]
    public int Impact { get; set; }

    [JsonPropertyName("start_date")]
    [JsonConverter(typeof(UtcConverter))]
    public DateTime? StartDate { get; set; }

    [JsonPropertyName("text")]
    public string Text { get; set; }

    [JsonPropertyName("updates")]
    public UpdateEntity[] Updates { get; set; }
}

internal class UpdateEntity {
    [JsonPropertyName("status")]
    [JsonConverter(typeof(StatusEnumConverter))]
    public StatusEnum Status { get; set; }

    [JsonPropertyName("text")]
    public string Text { get; set; }

    [JsonPropertyName("timestamp")]
    [JsonConverter(typeof(UtcConverter))]
    public DateTime? Timestamp { get; set; }
}

internal enum StatusEnum {
    Analyzing,
    Completed,
    Description,
    InProgress,
    Resolved,
    System,
    Scheduled,
    Fixing,
    Observing
}

internal class StatusEnumConverter : JsonConverter<StatusEnum> {
    public override StatusEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        var value = reader.GetString()?.ToLower();
        return value switch {
            "system" => StatusEnum.System,
            "analyzing" => StatusEnum.Analyzing,
            "completed" => StatusEnum.Completed,
            "description" => StatusEnum.Description,
            "in progress" => StatusEnum.InProgress,
            "resolved" => StatusEnum.Resolved,
            "scheduled" => StatusEnum.Scheduled,
            "fixing" => StatusEnum.Fixing,
            "observing" => StatusEnum.Observing,
            _ => throw new JsonException($"Unknown status value: {value}")
        };
    }

    public override void Write(Utf8JsonWriter writer, StatusEnum value, JsonSerializerOptions options) => throw new NotImplementedException();
}

internal class UtcConverter : JsonConverter<DateTime?> {
    private const string dateFormat = "yyyy-MM-dd HH:mm";

    public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        var timestamp = reader.GetString();
        var ok = DateTime.TryParseExact(timestamp, dateFormat, null, System.Globalization.DateTimeStyles.AssumeUniversal, out var res);
        return ok ? res : null;
    }

    public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options) => throw new NotImplementedException();
}
