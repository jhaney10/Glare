﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Glare.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Glare
{
    public class LogoutModel : PageModel
    {
        private SignInManager<AppUser> _signInManager;
        private ILogger<LogoutModel> _logger;

        public LogoutModel(SignInManager<AppUser> signInManager, ILogger<LogoutModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }
        
        public async Task<IActionResult> OnPost()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out");
            HttpContext.Session.Clear();
            return RedirectToPage("/Index");
        }
        
    }
}