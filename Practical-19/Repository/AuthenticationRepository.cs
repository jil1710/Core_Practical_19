using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Practical_19.Interfaces;
using Practical_19.Models;

namespace Practical_19.Repository
{
    public class AuthenticationRepository : IAuthentication
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AuthenticationRepository(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,RoleManager<IdentityRole> roleManager)
        {

            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }
        public async Task<IdentityResult> RegisterAsync(RegisterViewModel model)
        {
            var user = new IdentityUser
            {
                UserName = model.Email,
                Email = model.Email
            };

            var role = new IdentityRole
            {
                Name = "Admin" // Change to User to check User Feature
            };

            var res = await roleManager.CreateAsync(role);

            var result = await userManager.CreateAsync(user, model.Password);
            await userManager.AddToRoleAsync(user, role.Name);
            return result;

        }

        public async Task<Microsoft.AspNetCore.Identity.SignInResult> LoginAsync(LoginViewModel model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                var identityResult = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (identityResult.Succeeded)
                {
                    return identityResult;
                }
                return null;
            }
            return null;
        }
          
    }
}
