using Microsoft.EntityFrameworkCore;
using MotoFix.Data;
using MotoFix.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<MotoFixContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("MotoFixConnection")));

builder.Services.AddScoped<IClienteService, ClienteService>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to enable the HSTS middleware.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();