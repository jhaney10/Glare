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
    public class UsersInRoleModel : PageModel
    {
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<IdentityUser> _userManager;

        public UsersInRoleModel(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        [BindProperty]
        public List<UsersInRoleVM> UsersRole { get; set; }
        [TempData]
        public string Message { get; set; }
        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return Page();
            }

            var role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                var usersInRole = new List<UsersInRoleVM>();
                foreach (var user in _userManager.Users)
                {
                    var userInRole = new UsersInRoleVM();
                    userInRole.UserId = user.Id;
                    userInRole.UserName = user.UserName;
                    if (await _userManager.IsInRoleAsync(user, role.Name))
                    {
                        userInRole.IsSelected = true;
                    }

                    usersInRole.Add(userInRole);
                }

                UsersRole = usersInRole;
                return Page();
            }
            else
            {
                Message = $"Role Id {role} was not found";
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (ModelState.IsValid)
            {
                var role = await _roleManager.FindByIdAsync(id);
                foreach (var user in UsersRole)
                {
                    IdentityResult result = null;
                    var roleUser = await _userManager.FindByIdAsync(user.UserId);
                    var checkRole = await _userManager.IsInRoleAsync(roleUser, role.Name);
                    if (user.IsSelected == false && checkRole)
                    {
                        result = await _userManager.RemoveFromRoleAsync(roleUser, role.Name);
                    }
                    else if (user.IsSelected == true && !checkRole)
                    {
                        result = await _userManager.AddToRoleAsync(roleUser, role.Name);
                    }
                    else
                    {
                        continue;
                    }
                }

                return RedirectToPage("/Admin/EditRole", new {id = id});
            }
            else
            {
                return Page();
            }
        }
    }
}