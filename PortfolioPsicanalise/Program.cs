using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using PortfolioPsicanalise.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using PortfolioPsicanalise.Controllers;
using PortfolioPsicanalise.Models;
using PortfolioPsicanalise.Services.SessionService;
using Microsoft.Extensions.DependencyInjection;



var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<PortfolioPsicanalise.Services.SessionService.ISession, Session>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


string mySqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContextPool<AppDbContext>(options =>
options.UseMySql(mySqlConnection, ServerVersion.AutoDetect(mySqlConnection)));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(
    options => { options.LoginPath = "/Account/Login"; });


builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly= true;
    options.Cookie.IsEssential= true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


app.UseSession();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=LandingPage}/{id?}");

app.Run();
