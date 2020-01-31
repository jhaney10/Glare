using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Glare.Models;
using Glare.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Glare
{
    public class EditRoleModel : PageModel
    {
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<AppUser> _userManager;

        public EditRoleModel(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [BindProperty]
        public EditRoleVM UserRole { get; set; }
        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return RedirectToPage("/NotFound");
            }

            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return RedirectToPage("/NotFound");
            }
            //UserRole = new EditRoleVM();
            var newEdit = new EditRoleVM();
            newEdit.RoleId = id;
            newEdit.RoleName = role.Name;
            foreach (var user in _userManager.Users)
            {
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    newEdit.RoleUsers.Add(user.UserName);
                }
            }

            UserRole = newEdit;
            return Page();


        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return Page();
            }
            else
            {
                role.Name = UserRole.RoleName;
                var result = await _roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToPage("/Admin/RolesList");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
                return Page();
            }
        }
    }
}