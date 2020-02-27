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
    public class DeleteModel : PageModel
    {
        private UserManager<AppUser> _userManager;

        public DeleteModel(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public AppUser SelectedUser { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SelectedUser = await _userManager.FindByIdAsync(id);

            if(SelectedUser == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SelectedUser = await _userManager.FindByIdAsync(id);

            if (SelectedUser != null)
            {
                await _userManager.DeleteAsync(SelectedUser);
            }

            return RedirectToPage("./Index");
        }
    }
}
