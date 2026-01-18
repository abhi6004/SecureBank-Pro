using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using SecureBank_Pro.Data;
using SecureBank_Pro.Models;
using SecureBank_Pro.Services;
using StackExchange.Redis;


var builder = WebApplication.CreateBuilder(args);

QuestPDF.Settings.License = LicenseType.Community;
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();  
builder.Services.AddSession();
builder.Services.AddSignalR();
builder.Services.AddHttpClient();
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Information);
//if (builder.Environment.IsDevelopment())
//{
//    var redis = ConnectionMultiplexer.Connect("localhost:6379");
//    builder.Services.AddSingleton<IConnectionMultiplexer>(redis);
//}



builder.Services.AddDbContext<BankDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")),
    ServiceLifetime.Scoped);

builder.Services.AddControllersWithViews().AddNewtonsoftJson();
builder.Services.AddSingleton<AutoBackgroundService>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<AutoBackgroundService>());

builder.Services.AddAuthentication("UserCookies")
    .AddCookie("UserCookies", options =>
    {
        options.LoginPath = "/Users/Login";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(120);
        options.Cookie.Name = "UserCookie";
        options.Cookie.Path = "/";
        options.AccessDeniedPath = "/Users/AccessDenied";
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();   // for .css and .js
app.UseRouting();
app.UseSession();

app.Use(async (context, next) =>
{
    context.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
    context.Response.Headers["Pragma"] = "no-cache";
    context.Response.Headers["Expires"] = "0";
    await next();
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Users}/{action=Login}");

app.MapHub<ChatBoxHub>("/chatHub");


app.Run();
