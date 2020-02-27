using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Glare.Data;
using Glare.Models;
using Microsoft.AspNetCore.Identity;
using Glare.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Glare.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private UserManager<AppUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public EditModel(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            RolesList = new List<RolesInUser>();
            UserClaims = new List<ClaimsInUser>();
        }

        
        [BindProperty]
        public List<RolesInUser> RolesList { get; set; }
        [BindProperty]
        public List<ClaimsInUser> UserClaims { get; set; }
        

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles =  _roleManager.Roles;
            
            foreach(var roleInUser in roles)
            {
                RolesInUser roleForUser = new RolesInUser();
                roleForUser.RoleId = roleInUser.Id ;
                roleForUser.RoleName = roleInUser.Name;
                if (await _userManager.IsInRoleAsync(user, roleInUser.Name))
                {
                    roleForUser.IsSelected = true;
                }
                RolesList.Add(roleForUser);
            }
            foreach(var claim in ClaimsStore.AllClaims)
            {
                ClaimsInUser claimsInUser = new ClaimsInUser();
                claimsInUser.ClaimType = claim.Type;
                if (userClaims.Any(c=>c.Type == claim.Type))
                {
                    claimsInUser.IsSelected = true;
                }
                UserClaims.Add(claimsInUser);
            }
            return Page();
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (!ModelState.IsValid || id == null)
            {
                return Page();
            }
            AppUser userToEdit = await _userManager.FindByIdAsync(id);
            try
            {
                for(int i = 0; i < RolesList.Count(); i++)
                {
                    var userRole = await _userManager.IsInRoleAsync(userToEdit, RolesList[i].RoleName);
                    var selectedRole = RolesList[i].IsSelected;
                    if (userRole && !selectedRole)
                    {
                        var result = await _userManager.RemoveFromRoleAsync(userToEdit, RolesList[i].RoleName);
                        if (!result.Succeeded)
                        {
                            ModelState.AddModelError("", "Cannot remove role from user");
                        }
                    }
                    else if (!userRole && selectedRole)
                    {
                        var result = await _userManager.AddToRoleAsync(userToEdit, RolesList[i].RoleName);
                        if (!result.Succeeded)
                        {
                            ModelState.AddModelError("", "Cannot Add role to user");
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
                for(int i = 0; i < UserClaims.Count(); i++)
                {
                    var userClaims = await _userManager.GetClaimsAsync(userToEdit);
                    var result = await _userManager.RemoveClaimsAsync(userToEdit, userClaims);
                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError("", "Cannot Remove Claims from user");
                    }
                    result = await _userManager.AddClaimsAsync(userToEdit, UserClaims.Where(x=> x.IsSelected)
                        .Select(c=>new Claim (c.ClaimType, c.ClaimType)));
                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError("", "Cannot Add Claims to user");
                    }
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                bool checkUser = await UserExistsAsync(id);
                if (!checkUser)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Details", new { id = id});
        }

        private async Task<bool> UserExistsAsync(string id)
        {
            var result = await _userManager.FindByIdAsync(id);
            if(result != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
