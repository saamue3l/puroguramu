using Microsoft.AspNetCore.Identity;
using Puroguramu.Infrastructures.DbContexts;

namespace Puroguramu.Infrastructures.Initializers;

public class IdentityDataInitializer
{
    public static async Task SeedData(UserManager<PuroUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        await SeedRoles(roleManager);
        await SeedUsers(userManager);
    }

    private static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
    {
        string[] roleNames = { "Enseignant", "Etudiant" };
        foreach (var roleName in roleNames)
        {
            var roleExist = await roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }

    private static async Task SeedUsers(UserManager<PuroUser> userManager)
    {
        if (await userManager.FindByEmailAsync("enseignant@example.com") == null)
        {
            var enseignant = new PuroUser
            {
                UserName = "Q123456",
                Email = "enseignant@example.com",
                Nom = "Doe",
                Prenom = "John",
                IDGroupe = 1
            };

            var enseignantResult = await userManager.CreateAsync(enseignant, "Enseignant123!");
            if (enseignantResult.Succeeded)
            {
                await userManager.AddToRoleAsync(enseignant, "Enseignant");
            }
        }

        if (await userManager.FindByEmailAsync("etudiant@example.com") == null)
        {
            var etudiant = new PuroUser
            {
                UserName = "Q654321",
                Email = "etudiant@example.com",
                Nom = "Smith",
                Prenom = "Jane",
                IDGroupe = 2
            };

            var etudiantResult = await userManager.CreateAsync(etudiant, "Etudiant123!");
            if (etudiantResult.Succeeded)
            {
                await userManager.AddToRoleAsync(etudiant, "Etudiant");
            }
        }
    }
}
