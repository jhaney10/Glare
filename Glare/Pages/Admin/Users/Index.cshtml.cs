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
using Glare.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace Glare.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public IndexModel(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public List<AppUser> Users { get; set; }
        public int PageSize { get; set; } = 10;
        public PaginatedList<AppUser> PaginatedUsers { get; set; }

        public async Task OnGetAsync(int? currentPage)
        {
            int pageSize = PageSize;
            var allUsers = _userManager.Users;
            Users = allUsers.ToList();
            PaginatedUsers = await PaginatedList<AppUser>.ItemAsync(allUsers, currentPage ?? 1, pageSize);
        }
    }
}
