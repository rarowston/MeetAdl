using MeetAdl.Configuration;
using MeetAdl.Data;
using MeetAdl.Security;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;

var builder = WebApplication.CreateBuilder(args);

IConfigurationBuilder configBuilder = new ConfigurationBuilder()
                    .SetBasePath(builder.Environment.ContentRootPath)
                    // base appsettings (pipelines should replace values here)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    // dev specific app settings that are stored in the repository
                    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
                    // local appsettings file for secret secrets
                    .AddJsonFile("appsettings.Local.json", optional: true)
                    .AddEnvironmentVariables();

IConfigurationRoot configurationRoot = configBuilder.Build();

// Add configuration
builder.Services.AddSingleton(configurationRoot.GetSection("DatabaseConfiguration").Get<DatabaseConfiguration>()!);

// Add services to the container.
builder.Services.AddRazorPages()
    .AddMicrosoftIdentityUI();

// Add authentication and authorization
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(configurationRoot.GetSection("AzureAd"));
builder.Services.AddAuthorization();

// Add our custom auth code. 
builder.Services.AddScoped<IAuthorizationHandler, GlobalPermissionHandler>();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();

// Turn this on to allow for redirect URI override for devtunnels. 
// builder.Services.Configure<OpenIdConnectOptions>(OpenIdConnectDefaults.AuthenticationScheme, options =>
// {
//     options.Events = new OpenIdConnectEvents
//     {
//         OnRedirectToIdentityProvider = (context) =>
//         {
//             // Override the redirect_uri
//             //  Ideally extract this from config 
//             //  Or context.Request.Headers["X-Forwarded-Host"]
//             //  see: https://learn.microsoft.com/en-us/azure/frontdoor/front-door-http-headers-protocol#front-door-to-backend

//             context.ProtocolMessage.RedirectUri
//                 = "https://mdc1g0kg-7221.aue.devtunnels.ms/signin-oidc";
//             return Task.FromResult(0);
//         }
//     };
// });

// Add Data services
builder.Services.AddScoped<MeetAdlDbContext>();
builder.Services.AddScoped<IGroupRepository, GroupRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Add General Services
builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddScoped<ICurrentIdentityService, CurrentIdentityService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Require that authorisation is enabled by default
app.MapRazorPages().RequireAuthorization();
app.MapControllers();

app.Run();
