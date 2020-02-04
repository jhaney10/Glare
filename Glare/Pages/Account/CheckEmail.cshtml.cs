using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Glare.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace Glare
{
    public class CheckEmailModel : PageModel
    {
        private UserManager<AppUser> _userManager;

        public CheckEmailModel(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<JsonResult> OnGetAsync(string email)
        {
            if(email == null)
            {
                email = "a@b.c";
            }
            

            try{
                System.Threading.Thread.Sleep(200);
                var result = await _userManager.FindByEmailAsync(email);
                if (result != null)
                {
                    return new JsonResult(1);
                }
                else
                {
                    return new JsonResult(0);
                }
            }
            catch (DbException ex)
            {
                throw new InvalidOperationException("Data could not be read", ex);
            }
            
        }
    }
}