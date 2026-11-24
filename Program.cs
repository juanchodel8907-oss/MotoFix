using Microsoft.EntityFrameworkCore;
using MotoFix.Data;
using MotoFix.Services;
using Microsoft.AspNetCore.Identity;
using MotoFix.Models;
using Microsoft.AspNetCore.Authorization;
using MotoFix.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<MotoFixContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("MotoFixConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // Configuración de contraseñas
    options.Password.RequiredLength = 8;
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;

    // Configuración de usuarios
    options.User.RequireUniqueEmail = true;

    // No exigiremos confirmación de correo por ahora
    options.SignIn.RequireConfirmedAccount = false;

})
.AddEntityFrameworkStores<MotoFixContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";

    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;

    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    options.SlidingExpiration = true;
});

builder.Services.AddScoped<IClienteService, ClienteService>();

builder.Services.AddScoped<IMotocicletaService, MotocicletaService>();

builder.Services.AddControllersWithViews();

// Registrar el sistema de autorización mediante permisos.
builder.Services.AddSingleton<IAuthorizationPolicyProvider,
    PermissionPolicyProvider>();

builder.Services.AddScoped<IAuthorizationHandler,
    PermissionAuthorizationHandler>();

var app = builder.Build();

// Crear el usuario administrador inicial.
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    await IdentitySeeder.SeedAsync(services);

    await PermissionSeeder.SeedAsync(services);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();