using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
.AddCookie()
.AddOpenIdConnect(options =>
{
    options.Authority = builder.Configuration["Cognito:Domain"];
    options.ClientId = builder.Configuration["Cognito:ClientId"];
    options.ClientSecret = builder.Configuration["Cognito:ClientSecret"];
    options.MetadataAddress = builder.Configuration["Cognito:MetadataAddress"];
    options.ResponseType = builder.Configuration["Cognito:ResponseType"];
    options.SaveTokens = true;
    options.Scope.Clear();
    options.Scope.Add("openid");
    options.Scope.Add("profile");
    options.Scope.Add("email");
    
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true
    };
    
    options.Events = new OpenIdConnectEvents
    {
        OnRedirectToIdentityProviderForSignOut = OnRedirectToIdentityProviderForSignOut
    };
});

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("AdminOnly", policy => policy.RequireClaim("cognito:groups", "Admin"));

builder.Services.AddHttpContextAccessor();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");

    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();

// This method performs a AWS Cognito sign-out, and then redirects user back to the application
Task OnRedirectToIdentityProviderForSignOut(RedirectContext context)
{
    context.ProtocolMessage.Scope = "openid";

    context.ProtocolMessage.ResponseType = "code";

    string cognitoDomain = builder.Configuration["Cognito:Domain"];

    string clientId = builder.Configuration["Cognito:ClientId"];

    string logoutUrl = $"{context.Request.Scheme}://{context.Request.Host}{builder.Configuration["Cognito:AppSignOutUrl"]}";

    string issuerAddress = $"{cognitoDomain}/logout?client_id={clientId}&logout_uri={logoutUrl}";

    context.ProtocolMessage.IssuerAddress = issuerAddress; 

    return Task.CompletedTask;
}
