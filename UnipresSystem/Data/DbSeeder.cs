using Entity.Models;
using Microsoft.AspNetCore.Identity;

namespace UnipresSystem.Data
{
    public class DbSeeder
    {
        public static async Task SeedRolesAndSuperAdminAsync(IServiceProvider services)
        {
            // Ocupamos los servicios de Identity
            var userManager = services.GetRequiredService<UserManager<AuthUser>>();
            var roleManager = services.GetRequiredService<RoleManager<AuthRole>>();

            // --- Nombres de roles ---
            string[] roleNames = { "SuperAdmin", "Admin", "Vendedor", "Contador", "Usuario" };

            foreach (var roleName in roleNames)
            {
                // Si el rol no existe, lo crea
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new AuthRole { Name = roleName });
                }
            }

            // --- Creación del Super Admin ---
            // (Puedes leer estos datos desde appsettings.json)
            var superAdminEmail = "23905";

            var superAdminUser = await userManager.FindByEmailAsync(superAdminEmail);

            // Si el Super Admin no existe...
            if (superAdminUser == null)
            {
                var user = new AuthUser
                {
                    UserName = "superadmin",
                    Email = superAdminEmail,
                    EmailConfirmed = true // Confírmalo de una vez
                };

                // ...lo crea con esta contraseña
                var result = await userManager.CreateAsync(user, "Upm3141592$");

                // ...y le asigna el rol "SuperAdmin"
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "SuperAdmin");
                }
            }
        }
    }
}
