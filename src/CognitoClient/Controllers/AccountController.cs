using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;

namespace CognitoClient.Controllers;

public class AccountController : Controller
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AccountController(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    
    [HttpGet]
    [ActionName("SignOut")]
    public async Task SignOutAsync()
    {
        // Note: To sign out the current user and delete their cookie, call SignOutAsync

        // 1. Initiate signout for cookie based authentication scheme
        await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        // 2. Initiate signout for OpenID authentication scheme
        await _httpContextAccessor.HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
    }
}
