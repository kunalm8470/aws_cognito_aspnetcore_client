using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CognitoClient.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace CognitoClient.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HomeController(ILogger<HomeController> logger, IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        
        _httpContextAccessor = httpContextAccessor;
    }

    public IActionResult Index()
    {
        return View();
    }

    [Authorize(Policy = "AdminOnly")]
    public IActionResult AdminView()
    {
        return View();
    }

    [Authorize]
    public async Task<IActionResult> Claims()
    {
        string accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
        
        string idToken = await _httpContextAccessor.HttpContext.GetTokenAsync("id_token");

        ViewBag.AccessToken = accessToken;
        
        ViewBag.IdToken = idToken;

        return View();
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
