using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Glare.Models;
using Glare.ViewModels;
using Microsoft.AspNetCore.Authentication;
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
        [BindProperty]
        public LoginVM ReturningUser { get; set; }
        public async Task<IActionResult> OnGetAsync(string returnUrl)
        {
            var returningUser = new LoginVM
            {
                ReturnUrl = returnUrl,
                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
        };
           
            ReturningUser = returningUser;
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToPage("/Index");
            }
            return Page();
        }


        public async Task<IActionResult> OnPostAsync(string returnUrl)
        {
            ReturningUser.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var user = await _userManager.FindByEmailAsync(ReturningUser.Email);
            if (user != null && !user.EmailConfirmed && 
                (await _userManager.CheckPasswordAsync(user, ReturningUser.Password)))
            {
                ModelState.AddModelError("", "Kindly confirm your email address");
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
        public IActionResult OnPostGoogleLogin(string provider, string returnUrl)
        {
            var redirectUrl = Url.Page("/Account/ExternalLoginCallBack", new { ReturnUrl = returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }
    }
}