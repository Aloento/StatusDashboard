namespace StatusDashboard.Components.New;

using System.Diagnostics.CodeAnalysis;
using Event;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;

public partial class NewForm {
    [SupplyParameterFromForm]
    private NewModel model { get; set; } = new();

    [NotNull]
    private ServiceItem[]? items { get; set; }

    protected override async Task OnInitializedAsync() {
        await base.OnInitializedAsync();

        this.items = await this.db.Services
            .OrderBy(x => x.Name)
            .Select(x => new ServiceItem {
                Id = x.Id,
                Name = x.Name,
                Regions = x.Regions
                    .Select(r => new RegionItem {
                        Id = r.Id,
                        Name = r.Name
                    }).ToList()
            })
            .ToArrayAsync();
    }

    private async Task submit(EditContext ctx) {
        var targets = this.items
            .SelectMany(x => x.Regions
                .Where(r => r.Selected)
                .Select(r => this.db.RegionService
                    .Single(y =>
                        y.ServiceId == x.Id &&
                        y.RegionId == r.Id)))
            .ToArray();

        this.model.Services = targets.Length == 0 ? null : new();
        var isValid = ctx.Validate();
        if (!isValid) return;

        var entity = this.db.Events.Add(new() {
            Title = this.model.Title,
            Type = this.model.Type,
            Start = this.model.Start.Value,
            End = this.model.Type is EventType.Maintenance ? this.model.End : null,
            RegionServices = targets
        }).Entity;

        if (!string.IsNullOrWhiteSpace(this.model.Description))
            entity.Histories = [
                new() {
                    Created = DateTime.UtcNow,
                    Message = this.model.Description,
                    Status = this.model.Type is EventType.Maintenance ? EventStatus.Scheduled : default
                }
            ];

        await this.db.SaveChangesAsync();
        this.nav.NavigateTo($"Event/{entity.Id}");
    }
}
