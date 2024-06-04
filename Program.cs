using Microsoft.FluentUI.AspNetCore.Components;
using StatusDashboard.Components;
using StatusDashboard.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions<StatusOption>()
    .BindConfiguration("SDB")
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddHttpClient();
builder.Services.AddSingleton<StatusService>();
builder.Services.AddHostedService(x => x.GetRequiredService<StatusService>());

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddFluentUIComponents();

var app = builder.Build();

if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
