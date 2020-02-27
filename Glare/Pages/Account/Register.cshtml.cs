using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Glare.Models;
using Glare.Services;
using Glare.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Glare
{
    public class RegisterModel : PageModel
    {
        private UserManager<AppUser> _userManager;
        private SignInManager<AppUser> _signInManager;
        private RoleManager<IdentityRole> _roleManager;
        private IEmailSender _emailSender;

        public RegisterModel(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
            RoleManager<IdentityRole> roleManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
        }
        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public RegisterVM NewUser { get; set; }
        [TempData]
        public string Message { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                var checkEmail = await _userManager.FindByEmailAsync(NewUser.Email);
                if (checkEmail != null)
                {
                    Message = "This email already exists";
                    return Page();
                }
            }
            catch (DbException ex)
            {
                throw new InvalidOperationException("Database connect error", ex);
            }
            var user = new AppUser
            {
                UserName = NewUser.Email,
                Email = NewUser.Email,
                FirstName = NewUser.FirstName,
                LastName = NewUser.LastName
            };
            
            var result = await _userManager.CreateAsync(user, NewUser.Password);
            if (result.Succeeded)
            {

                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                string confirmationLink = Url.Page(
                  "/Account/ConfirmEmail",
                  pageHandler: null,
                  values: new { area = "Identity", userId = user.Id, tokenId = token },
                  protocol: HttpContext.Request.Scheme);
                string email = user.Email;
                string subject = "Confirm your account";
                await _emailSender.SendEmailAsync(email, subject, "Click on this link to confirm your account </br> <a class=\"btn btn-success\" href=\"" + confirmationLink + "\">" + confirmationLink + "</a>");

                var role = await _roleManager.FindByNameAsync("User");
                if (role != null)
                {
                    result = await _userManager.AddToRoleAsync(user, role.Name);
                }
                if (_signInManager.IsSignedIn(User) && User.IsInRole("Admin"))
                {
                    return RedirectToPage("/Admin/Users/Index");
                }
                else
                {
                    //await _signInManager.SignInAsync(user, isPersistent: false);
                    //return RedirectToPage("/Index");
                    return RedirectToPage("/Account/RegisterConfirmation");
                }
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return Page();
            }

        }

    }
}