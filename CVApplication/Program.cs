using CVApplication.Data;
using CVApplication.Data.Seeders;
using CVApplication.Models;
using CVApplication.Services;
using CVApplication.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Infrastructure;   

var builder = WebApplication.CreateBuilder(args);

QuestPDF.Settings.License = LicenseType.Community;

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services
    .AddIdentity<User, IdentityRole>(options =>
    {
        options.Password.RequiredLength = 8;
        options.Password.RequireDigit = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireNonAlphanumeric = false;
        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<ICVService, CVService>();
builder.Services.AddScoped<IAnalyseService, AnalyseService>();
builder.Services.AddScoped<IScoreService, ScoreService>();
builder.Services.AddScoped<IMatchingService, MatchingService>();
builder.Services.AddScoped<IRecommandationService, RecommandationService>();
builder.Services.AddScoped<IRapportService, RapportService>();
builder.Services.AddScoped<IOffreEmploiService, OffreEmploiService>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    await RoleSeeder.SeedRolesAsync(services);
    await RoleSeeder.SeedAdminAsync(services);
}

app.Run();