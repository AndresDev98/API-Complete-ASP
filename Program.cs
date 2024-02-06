using API_Complete_ASP.Database;
using API_Complete_ASP.Database.Services;
using API_Complete_ASP.Helpers;
using API_Complete_ASP.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddControllersWithViews();
builder.Services.AddControllersWithViews(options => options.EnableEndpointRouting = false).AddSessionStateTempDataProvider();

// --------------------------------------------------------------------------------------------- //

builder.Services.AddDbContext<APICompleteContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("APICompleteContext"));
});

// --------------------------------------------------------------------------------------------- //

builder.Services.AddTransient<UserRepo>();
builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<JwtService>();

// --------------------------------------------------------------------------------------------- //
var proveedor = builder.Services.BuildServiceProvider();
var configuration = proveedor.GetService<IConfiguration>();
builder.Services.AddCors();

// Cookies ------------------------------------------------------------------------------------- //

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(5);
});
builder.Services.AddAuthentication(options =>
{
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, config =>
{
    config.AccessDeniedPath = "/Home/ErrorAcceso";
});

builder.Services.AddSingleton<ITempDataProvider, CookieTempDataProvider>();


var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Users}/{action=Login}/{id?}");

app.Run();
