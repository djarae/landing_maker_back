using Microsoft.AspNetCore.Identity;
using LandingMakerBack.Models;

namespace LandingMakerBack.Data;

public static class DbInitializer
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        
        // Buscar si ya existe el admin específicamente
        var existingAdmin = await userManager.FindByEmailAsync("admin@landing.com");
        
        if (existingAdmin != null)
        {
            Console.WriteLine("⚠️ El usuario admin ya existe. No se realizarán cambios.");
            return;
        }
        
        // Crear usuario admin
        var adminUser = new ApplicationUser
        {
            UserName = "admin@landing.com",
            Email = "admin@landing.com",
            FullName = "Administrador Sistema",
            EmailConfirmed = true
        };
        
        var result = await userManager.CreateAsync(adminUser, "Admin123!");
        
        if (!result.Succeeded)
        {
            Console.WriteLine("Error creando usuario admin: " + string.Join(", ", result.Errors.Select(e => e.Description)));
        }
        else
        {
            Console.WriteLine("✅ Usuario admin creado: admin@landing.com / Admin123!");
        }
    }
}
