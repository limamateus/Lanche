using Microsoft.AspNetCore.Identity;

namespace Lanche.Services
{
    public class SeedUserRoleInitial : ISeedUserRoleInitial
    {

        private readonly UserManager<IdentityUser> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        public SeedUserRoleInitial(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void SeedRole()
        { // Estou verificando se ele existe e se ele não existir ele vai criar um novo perfil
            if (!_roleManager.RoleExistsAsync("Member").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Member";
                role.NormalizedName = "MEMBER";
                IdentityResult roleResult = _roleManager.CreateAsync(role).Result;

            }
            if (!_roleManager.RoleExistsAsync("Admin").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Admin";
                role.NormalizedName = "ADMIN";
                IdentityResult roleResult = _roleManager.CreateAsync(role).Result;

            }
        }

        public void SeedUsers()
        { // verificando se o usuario for igual a null se ele for , o serviço vai criar um usuario padrão
            if (_userManager.FindByEmailAsync("usuario@localhost").Result == null)
            {
                IdentityUser user = new IdentityUser();

                user.UserName = "usuario@localhost";
                user.Email = "usuario@localhost";
                user.NormalizedEmail = "USUARIO@LOCALHOST";
                user.NormalizedUserName = "USUARIO@LOCALHOST";
                user.EmailConfirmed = true;
                user.LockoutEnabled = false;
                user.SecurityStamp = Guid.NewGuid().ToString();

                IdentityResult result = _userManager.CreateAsync(user,"Mateus28%").Result;

                if (result.Succeeded)
                {
                    _userManager.AddToRoleAsync(user, "Member").Wait();
                }



            }
            if (_userManager.FindByEmailAsync("admin@localhost").Result == null)
            {
                IdentityUser user = new IdentityUser();

                user.UserName = "admin@localhost";
                user.Email = "admin@localhost";
                user.NormalizedEmail = "ADMIN@LOCALHOST";
                user.NormalizedUserName = "ADMIN@LOCALHOST";
                user.EmailConfirmed = true;
                user.LockoutEnabled = false;
                user.SecurityStamp = Guid.NewGuid().ToString();
                IdentityResult result = _userManager.CreateAsync(user,"Mateus28%").Result;

                if (result.Succeeded)
                {
                    _userManager.AddToRoleAsync(user, "Admin").Wait();
                }

            }

        }
    }
}
