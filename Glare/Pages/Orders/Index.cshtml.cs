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
    public class IndexModel : PageModel
    {
        private readonly Glare.Data.ProductContext _context;

        public IndexModel(Glare.Data.ProductContext context)
        {
            _context = context;
        }

        public IList<Order> Order { get;set; }

        public async Task OnGetAsync()
        {
            Order = await _context.Orders.Include(x => x.Customer).ToListAsync();
                
        }
    }
}
