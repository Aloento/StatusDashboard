namespace StatusDashboard.Components.Availability;

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Services;

public partial class AvailaMatrix {
    [NotNull]
    [CascadingParameter]
    public Region? Region { get; set; }

    [NotNull]
    private Category[]? categories { get; set; }

    private static List<string> get6Months() {
        var months = new List<string>();
        var date = DateTime.UtcNow;

        for (var i = 5; i >= 0; i--) {
            var time = date.AddMonths(-i);
            months.Add(
                $"{time.ToString("yyyy", CultureInfo.InvariantCulture)} <br /> {time.ToString("MMMM", CultureInfo.InvariantCulture)}");
        }

        return months;
    }

    protected override async Task OnParametersSetAsync() =>
        this.categories = await this.db.RegionService
            .Where(x => x.Region == this.Region)
            .Select(x => x.Service.Category)
            .Distinct()
            .OrderBy(x => x.Name)
            .ToArrayAsync();
}
