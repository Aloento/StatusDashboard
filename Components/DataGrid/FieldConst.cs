namespace StatusDashboard.Components.DataGrid;

public record FieldTypes {
    public string Actions => "actions";
    public string Checkbox => "checkbox";
    public string Email => "email";
    public string Date => "date";
    public string Graph => "graph";
    public string Html => "html";
    public string Link => "link";
    public string Number => "number";
    public string Select => "select";
    public string Tags => "tags";
    public string Telephone => "telephone";
    public string Text => "text";
}

public record SortBy {
    public string Number => "number";
    public string Text => "text";
}

public record TextAlign {
    public string Left => "left";
    public string Right => "right";
    public string Center => "center";
}
