using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Glare.Data;
using Glare.Models;
using Glare.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Threading;
using System.Diagnostics;

namespace Glare
{
    public class CartModel : PageModel
    {
        private UserManager<IdentityUser> _userManager;
        private SignInManager<IdentityUser> _signInManager;
        private ProductContext _context;

        public CartModel(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ProductContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            MyCart = new List<CartItem>();
        }
        [BindProperty]
        public string ShoppingCartId { get; set; }
        public const string CartSessionKey = "CartId";
        [TempData]
        public string Message { get; set; }

        [BindProperty]
        public List<CartItem> MyCart { get; set; }
        [BindProperty]
        public string Total { get; set; }
        public async Task OnGetAsync(int? productId)
        {
            
            ShoppingCartId = GetCartId();
            if (productId != null)
            {
                var product = await _context.Products.FirstOrDefaultAsync(x => x.ProductId == productId);
                var cartItem = await _context.CartItems.FirstOrDefaultAsync
                    (x => x.CartId == ShoppingCartId && x.Product == product && x.Order == null);
                
                if (cartItem == null)
                {
                    cartItem = new CartItem
                    {
                        CartId = ShoppingCartId,
                        Quantity = 1,
                        Product = product,
                        DateCreated = DateTime.Now,
                        SubTotal = product.Price
                    };
                    _context.Add(cartItem);
                }
                else
                {
                    cartItem.Quantity++;
                    cartItem.SubTotal = cartItem.Quantity * cartItem.Product.Price;
                }

                await _context.SaveChangesAsync();
            }
            
            var cartItems = _context.CartItems.Where(x => x.CartId == ShoppingCartId && x.Order == null)
                .Include(x => x.Product).ToList();
            MyCart = cartItems;
            var total = _context.CartItems.Where(x => x.CartId == ShoppingCartId && x.Order == null)
                .Select(x => x.Quantity * x.Product.Price).Sum();
            Total = string.Format("{0:C}", total);
        }


        private string GetCartId()
        {
            //This variable retrieves the current session value
            var currentSession = HttpContext.Session.GetString(CartSessionKey);

            //This variable retrieves the current user's identity
            var currentUser = HttpContext.User.Identity.Name;


            if (string.IsNullOrEmpty(currentSession))
            {
                if (string.IsNullOrEmpty(currentUser))
                {
                    Guid tempCartId = Guid.NewGuid();
                    HttpContext.Session.SetString(CartSessionKey, tempCartId.ToString());
                }
                else
                {
                    HttpContext.Session.SetString(CartSessionKey, currentUser);
                }
            }
            else if (!string.IsNullOrEmpty(currentUser))
            {
                if (currentSession != currentUser)
                {
                    TransferCartItems(currentUser, currentSession);
                    HttpContext.Session.SetString(CartSessionKey, currentUser);
                }
            }
            else
            {
                return HttpContext.Session.GetString(CartSessionKey);
            }

            return HttpContext.Session.GetString(CartSessionKey);
        }

        private void TransferCartItems(string currentUser, string currentSession)
        {
            var cartItems = _context.CartItems.Where(x => x.CartId == currentSession && x.Order == null);
            var userCartItems = _context.CartItems.Where(x => x.CartId == currentUser && x.Order == null);
            foreach (var item in cartItems)
            {
                item.CartId = currentUser;
            }
            _context.SaveChanges();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (ModelState.IsValid)
            {
                var itemToDelete = await _context.CartItems.FindAsync(id);
                if (itemToDelete != null)
                {
                    try
                    {
                        _context.CartItems.Remove(itemToDelete);
                        await _context.SaveChangesAsync();
                        Message = "Product has been removed from your cart";
                        return RedirectToPage("/Cart");
                    }
                    catch (DbUpdateException ex)
                    {
                        EventLog.WriteEntry("GlareShop", ex.Message, EventLogEntryType.Warning);
                        return RedirectToAction("/Cart");
                    }
                }

                Message = "Product does not exist in your cart";
                return RedirectToPage("/Cart");
            }

            return RedirectToPage("/Cart");
        }
    }
}