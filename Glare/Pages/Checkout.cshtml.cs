using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Glare.Data;
using Glare.Models;
using Glare.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Glare
{
    [Authorize]
    public class CheckoutModel : PageModel
    {
        private ProductContext _context;
        private UserManager<AppUser> _userManager;
        private SignInManager<AppUser> _signInManager;

        public CheckoutModel(ProductContext context, UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            MyOrder = new List<CartItem>();
        }
        [BindProperty]
        public List<CartItem> MyOrder { get; set; }
        [BindProperty]
        public string Total { get; set; }
        [BindProperty]
        public PlaceOrderVM PlaceOrder { get; set; }
        [TempData]
        public string Message { get; set; }
        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (_signInManager.IsSignedIn(User))
            {
                var cart = _context.CartItems.FirstOrDefault(x => x.CartId == id);
                if (cart == null)
                {
                    return RedirectToPage("/Error");
                }
                var currentUser = HttpContext.User.Identity.Name;
                if (id == currentUser)
                {
                    
                    var cartList = _context.CartItems.Where(x => x.CartId == id && x.Order == null)
                        .Include(x => x.Product).ToList();
                    MyOrder = cartList;
                    var totalCost = await _context.CartItems.Where(x => x.CartId == id && x.Order == null)
                        .Select(x => x.SubTotal).SumAsync();
                    Total = string.Format("{0:C}", totalCost);
                    return Page();
                }
                else
                {
                    TransferCartItems(id, currentUser);
                    var cartList = _context.CartItems.Where(x => x.CartId == currentUser && x.Order == null)
                        .Include(x => x.Product).ToList();
                    MyOrder = cartList;
                    var totalCost = await _context.CartItems.Where(x => x.CartId == currentUser && x.Order == null)
                        .Select(x => x.SubTotal).SumAsync();
                    Total = string.Format("{0:C}", totalCost);

                   
                    
                    return Page();
                }
                
            }
            else
            {
                return RedirectToPage("/Account/Login", new { returnUrl = Request.Path});
            }
            
        }

        private void TransferCartItems(string id, string currentUser)
        {
            var cartItems = _context.CartItems.Where(x => x.CartId == id && x.Order == null);
            foreach (var item in cartItems)
            {
                item.CartId = currentUser;
            }
            _context.SaveChanges();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            var crtId = HttpContext.User.Identity.Name;
            var orderList = _context.CartItems.Where(x => x.CartId == crtId && x.Order == null)
                        .Include(x => x.Product).ToList();
            var newOrder = new Order
            {
                Customer = await _userManager.GetUserAsync(User),
                OrderDate = DateTime.Now,
                OrderStatus = OrderStatus.Processing,
                OrderItems = orderList,
                DeliveryAddress = PlaceOrder.DeliveryAddress
            };
            try
            {
                _context.Orders.Add(newOrder);
                await _context.SaveChangesAsync();
                Message = "Your Order has been placed";
                return RedirectToPage("/Cart");
            }
            catch (DbUpdateException ex)
            {
                Message = ex.Message;
                return RedirectToPage("/Cart");
            }
            
        }
    }
}