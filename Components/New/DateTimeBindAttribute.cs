namespace StatusDashboard.Components.New;

using System.ComponentModel.DataAnnotations;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class DateTimeBindAttribute(string targetPropertyName, bool isStart = false) : ValidationAttribute {
    private string targetPropertyName { get; } = targetPropertyName;

    private bool isStart { get; } = isStart;

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext) {
        var targetProperty = validationContext.ObjectType.GetProperty(this.targetPropertyName);
        if (targetProperty is null)
            return new($"Unknown Property: {this.targetPropertyName}");

        var targetValue = targetProperty.GetValue(validationContext.ObjectInstance);
        if (value == null || targetValue == null)
            return ValidationResult.Success;

        if (value is DateTime current && targetValue is DateTime target) {
            if (this.isStart) {
                if (current >= target)
                    return new($"{validationContext.DisplayName} must be before {this.targetPropertyName}");
            } else {
                if (current <= target)
                    return new($"{validationContext.DisplayName} must be after {this.targetPropertyName}");
            }
        } else
            return new($"{this.targetPropertyName} must be a valid DateTime");

        return ValidationResult.Success;
    }
}
