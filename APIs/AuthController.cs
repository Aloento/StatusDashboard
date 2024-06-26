namespace StatusDashboard.APIs;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]/[action]")]
public class AuthController : Controller {
    [HttpGet]
    public IActionResult Login(string redirectUri = "/") =>
        this.Challenge(new AuthenticationProperties {
            RedirectUri = redirectUri
        });

    [HttpGet]
    public async Task<IActionResult> Logout(string redirectUri = "/") =>
        this.SignOut(
            new AuthenticationProperties {
                RedirectUri = redirectUri,
                Items = { { "id_token_hint", await this.HttpContext.GetTokenAsync("id_token") } }
            },
            CookieAuthenticationDefaults.AuthenticationScheme,
            OpenIdConnectDefaults.AuthenticationScheme
        );
}
