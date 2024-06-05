using Microsoft.EntityFrameworkCore;
using Microsoft.FluentUI.AspNetCore.Components;
using StatusDashboard.Components;
using StatusDashboard.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions<StatusOption>()
    .BindConfiguration(nameof(StatusDashboard))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddDbContext<StatusContext>(
    x => x
        .UseInMemoryDatabase(nameof(StatusDashboard))
        .EnableDetailedErrors()
        .EnableSensitiveDataLogging()
);

builder.Services.AddHttpClient();
builder.Services.AddSingleton<StatusService>();
builder.Services.AddHostedService(x => x.GetRequiredService<StatusService>());

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddFluentUIComponents();

var app = builder.Build();

if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Error", true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
