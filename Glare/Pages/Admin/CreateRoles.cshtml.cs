using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Glare.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Glare
{
    [Authorize]
    public class CreateRolesModel : PageModel
    {
        private RoleManager<IdentityRole> _roleManager;

        public CreateRolesModel(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        public void OnGet()
        {

        }
        [BindProperty]
        public CreateRoleVM NewRole { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            IdentityRole newRole = new IdentityRole
            {
                Name = NewRole.RoleName
            };

            IdentityResult result = await _roleManager.CreateAsync(newRole);
            if (result.Succeeded)
            {
                return RedirectToPage("/Admin/RolesList");
            }
            else
            {
                foreach(IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return Page();

        }
    }
}