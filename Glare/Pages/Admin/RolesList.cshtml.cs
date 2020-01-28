using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Glare
{
    [Authorize]
    public class RolesListModel : PageModel
    {
        private RoleManager<IdentityRole> _roleManager;

        public RolesListModel(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        public void OnGet()
        {
            MyRoles = _roleManager.Roles;
        }
        public IEnumerable<IdentityRole> MyRoles { get; set; }
    }
}