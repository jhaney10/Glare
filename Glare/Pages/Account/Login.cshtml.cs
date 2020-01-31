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
    public class LoginModel : PageModel
    {
        private UserManager<AppUser> _userManager;
        private SignInManager<AppUser> _signInManager;

        public LoginModel(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult OnGet()
        {
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToPage("/Index");
            }
            return Page();
        }
        [BindProperty]
        public LoginVM ReturningUser { get; set; }
        public async Task<IActionResult> OnPostAsync(string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var result = await _signInManager.PasswordSignInAsync(ReturningUser.Email, ReturningUser.Password, 
                ReturningUser.RememberMe,
                    lockoutOnFailure: false);
            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToPage("/Index");
                }
            }
            else
            {
                ModelState.AddModelError("", "Invalid Login Attempt");
                return Page();
            }


        }
    }
}