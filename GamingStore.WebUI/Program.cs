using GamingStore.EL.Models;
using GamingStore.WebUI.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.ConfigureDbContext(builder.Configuration);

// ⚠️ Identity, auth cookie'yi default ayarlarla register eder
// ConfigureApplicationCookie bundan SONRA gelmeli
builder.Services.ConfigureIdentity();
builder.Services.ConfigureApplicationCookie();

builder.Services.ConfigureRepositoryRegistration();
builder.Services.ConfigureServiceRegistration(builder.Configuration);

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.ConfigureSession();
builder.Services.ConfigureRouting();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Session, Routing'den ÖNCE çalışmalı
app.UseSession();
app.UseRouting();

// Sıra önemli: Authentication → Authorization
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapAreaControllerRoute(
        name: "Admin",
        areaName: "Admin",
        pattern: "Admin/{controller=Home}/{action=Index}/{id?}");

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    endpoints.MapRazorPages();
    endpoints.MapControllers();
});

app.ConfigureLocalization();
app.ConfigureDefaultAdminUser();

app.Run();
