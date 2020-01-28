using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Glare.ViewModels
{
    public class EditRoleVM
    {
        public EditRoleVM()
        {
            RoleUsers = new List<string>();
        }
        public string RoleId { get; set; }
        [Required]
        public string RoleName { get; set; }
        public List<string> RoleUsers { get; set; }
    }
}
