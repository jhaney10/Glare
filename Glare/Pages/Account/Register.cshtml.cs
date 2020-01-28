using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Glare.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Glare
{
    public class RegisterModel : PageModel
    {
        private UserManager<IdentityUser> _userManager;
        private SignInManager<IdentityUser> _signInManager;
        private RoleManager<IdentityRole> _roleManager;

        public RegisterModel(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public RegisterVM NewUser { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = new IdentityUser
            {
                UserName = NewUser.Email,
                Email = NewUser.Email,
            };
            var result = await _userManager.CreateAsync(user, NewUser.Password);
            if (result.Succeeded)
            {
                var role = await _roleManager.FindByNameAsync("User");
                if (role != null)
                {
                    await _userManager.AddToRoleAsync(user, role.Name);
                }
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToPage("/Index");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return Page();
            }

        }
    }
}