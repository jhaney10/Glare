using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Glare.Models;
using Glare.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Glare
{
    public class ExternalLoginCallBackModel : PageModel
    {
        private UserManager<AppUser> _userManager;
        private SignInManager<AppUser> _signInManager;

        public ExternalLoginCallBackModel(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public LoginVM GoogleUser { get; set; }
        public async Task<IActionResult> OnGetAsync(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            var returningUser = new LoginVM
            {
                ReturnUrl = returnUrl,
                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };
            if(remoteError != null)
            {
                ModelState.AddModelError("", $"Error from external provider: {remoteError}");
                return Page();
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if(info == null)
            {
                ModelState.AddModelError("", $"Error loading external login information");
                return Page();
            }

            var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider,
                info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (signInResult.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                var email = info.Principal.FindFirst(ClaimTypes.Email).Value;
                if(email != null)
                {
                    var user = await _userManager.FindByEmailAsync(email);
                    if(user == null)
                    {
                        user = new AppUser
                        {
                            UserName = info.Principal.FindFirst(ClaimTypes.Email).Value,
                            Email = info.Principal.FindFirst(ClaimTypes.Email).Value
                        };
                        await _userManager.CreateAsync(user);

                    }

                    await _userManager.AddLoginAsync(user, info);
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
            }
            return Page();

        }
    }
}