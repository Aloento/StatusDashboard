using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Authorization;
using Ljbc1994.Blazor.IntersectionObserver;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.EntityFrameworkCore;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using StatusDashboard.Components;
using StatusDashboard.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions<StatusOption>()
    .BindConfiguration(nameof(StatusDashboard))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services
    .AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddKeycloakWebApp(
        builder.Configuration.GetSection(KeycloakAuthenticationOptions.Section),
        configureOpenIdConnectOptions: options => {
            options.SaveTokens = true;
            options.ResponseType = OpenIdConnectResponseType.Code;
            options.Events = new() {
                OnSignedOutCallbackRedirect = context => {
                    context.Response.Redirect("/");
                    context.HandleResponse();

                    return Task.CompletedTask;
                }
            };
        }
    );

builder.Services
    .AddAuthorization()
    .AddKeycloakAuthorization(builder.Configuration);

builder.Services.AddDbContextFactory<StatusContext>(
    x => x
        .UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
        .EnableDetailedErrors()
        .EnableSensitiveDataLogging()
);

builder.Services.AddMemoryCache();
builder.Services.AddHttpClient<StatusHttp>();
builder.Services.AddHostedService<StatusService>();
builder.Services.AddScoped<SlaService>();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddControllers();

builder.Services.AddFluentUIComponents();
builder.Services.AddIntersectionObserver();

var app = builder.Build();

if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Error", true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapControllers();

app.Run();
