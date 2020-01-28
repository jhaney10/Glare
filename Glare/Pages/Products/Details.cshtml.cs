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

namespace Glare
{
    
    public class DetailsModel : PageModel
    {
        private readonly Glare.Data.ProductContext _context;

        public DetailsModel(Glare.Data.ProductContext context)
        {
            _context = context;
        }

        public Product Product { get; set; }
        public List<Photo> Photos { get; private set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //Product = await _context.Products.FirstOrDefaultAsync(m => m.ProductId == id);
            //Photos = await _context.Photos.Where(m => m.ProductId == id).ToListAsync();
            Product = await _context.Products
                .Include(m => m.Photo)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ProductId == id);

            if (Product == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
