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

if (!builder.Environment.IsDevelopment())
    builder.Services.AddLettuceEncrypt();

builder.Services
    .AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddKeycloakWebApp(
        builder.Configuration.GetSection(KeycloakAuthenticationOptions.Section),
        configureOpenIdConnectOptions: x => {
            x.SaveTokens = true;
            x.ResponseType = OpenIdConnectResponseType.Code;
            x.Events = new() {
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

builder.Services.AddResponseCompression(x => x.EnableForHttps = true);
builder.Services.AddResponseCaching();
builder.Services.AddOutputCache(x => x.AddBasePolicy(b => b.Cache()));

var app = builder.Build();

if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Error", true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseResponseCompression();
app.UseResponseCaching();

app.UseStaticFiles();
app.UseRouting();
app.UseAntiforgery();

app.UseOutputCache();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapControllers();

app.Run();
