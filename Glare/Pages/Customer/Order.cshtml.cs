using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Glare.Data;
using Glare.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Glare
{
    public class OrderModel : PageModel
    {
        private ProductContext _context;
        private UserManager<IdentityUser> _userManager;
        private SignInManager<IdentityUser> _signInManager;

        public OrderModel(ProductContext context, UserManager<IdentityUser> userManager, 
            SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            Orders = new List<Order>();
        }

        public List<Order> Orders { get; set; }
        public async Task OnGetAsync()
        {
            var userName = HttpContext.User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);
            Orders = await _context.Orders.Where(x => x.Customer == user)
                .Include(x => x.OrderItems)
                .ThenInclude(x => x.Product)
                .ThenInclude(x=> x.Photo).ToListAsync();

        }
    }
}