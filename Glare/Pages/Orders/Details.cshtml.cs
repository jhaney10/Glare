using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Glare.Data;
using Glare.Models;
using Microsoft.AspNetCore.Authorization;

namespace Glare.Orders
{
    [Authorize(Roles = "Admin")]
    public class DetailsModel : PageModel
    {
        private readonly Glare.Data.ProductContext _context;

        public DetailsModel(Glare.Data.ProductContext context)
        {
            _context = context;
        }

        public Order Order { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Order = await _context.Orders.Include(x=> x.OrderItems).ThenInclude(x=> x.Product).
                FirstOrDefaultAsync(m => m.OrderId == id);

            if (Order == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
