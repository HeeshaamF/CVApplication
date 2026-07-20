using CVApplication.Models;
using Microsoft.AspNetCore.Identity;

namespace CVApplication.Data.Seeders;

public static class RoleSeeder
{
    public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        string[] roles = { "Admin", "Candidat" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }

    public static async Task SeedAdminAsync(IServiceProvider serviceProvider)
    {
        var userManager =
            serviceProvider.GetRequiredService<UserManager<User>>();

        string adminEmail = "admin@test.com";
        string adminPassword = "Admin12!";

        var admin =
            await userManager.FindByEmailAsync(adminEmail);

        if (admin == null)
        {
            admin = new User
            {
                UserName = adminEmail,
                Email = adminEmail,
                Nom = "Administrateur"
            };

            await userManager.CreateAsync(
                admin,
                adminPassword);
        }

        if (!await userManager.IsInRoleAsync(
                admin,
                "Admin"))
        {
            await userManager.AddToRoleAsync(
                admin,
                "Admin");
        }
    }
}