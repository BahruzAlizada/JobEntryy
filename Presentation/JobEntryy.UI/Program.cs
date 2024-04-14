using JobEntryy.Domain.Identity;
using JobEntryy.Persistence.Concrete;
using Microsoft.AspNetCore.Identity;
using JobEntryy.Persistence.Registration;
using JobEntryy.Infrastructure.Registration;
using Serilog;
using Serilog.Core;
using Serilog.Sinks.MSSqlServer;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

Logger log = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt")
    .WriteTo.MSSqlServer("Server=DESKTOP-OK3QKVJ;Database=JobEntryyDataBase;Trusted_Connection=SSPI;Encrypt=false;TrustServerCertificate=true;Integrated Security=True;", "Logs", 
    autoCreateSqlTable: true)
    .CreateLogger();

builder.Host.UseSerilog(log);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<Context>();

builder.Services.AddPersistenceServices();
builder.Services.AddInfrastructureServices();

builder.Services.AddIdentity<AppUser, AppRole>(Identityoptions =>
{
    Identityoptions.User.RequireUniqueEmail = true;
    Identityoptions.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789._";
    Identityoptions.Password.RequiredLength = 8;
    Identityoptions.Password.RequireNonAlphanumeric = false;
    Identityoptions.Lockout.AllowedForNewUsers = true;
    Identityoptions.Lockout.MaxFailedAccessAttempts = 5;
    Identityoptions.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
}).AddEntityFrameworkStores<Context>().AddDefaultTokenProviders();


builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Profile}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
