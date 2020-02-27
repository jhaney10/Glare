using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Glare.Data;
using Glare.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Glare.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class DetailsModel : PageModel
    {
        
        private UserManager<AppUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public DetailsModel(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public AppUser SelectedUser { get; set; }
        public IList<string> Roles { get; set; }
        public List<string> UserClaims { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            SelectedUser = await _userManager.FindByIdAsync(id);
            if (SelectedUser == null)
            {
                return NotFound();
            }
            Roles = await _userManager.GetRolesAsync(SelectedUser);
            var userClaims = await _userManager.GetClaimsAsync(SelectedUser);
            UserClaims = userClaims.Select(x => x.Value).ToList();

            return Page();
        }
    }
}
