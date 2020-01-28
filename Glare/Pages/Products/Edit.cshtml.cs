using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Glare.Data;
using Glare.Models;
using Microsoft.AspNetCore.Authorization;

namespace Glare
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly Glare.Data.ProductContext _context;

        public EditModel(Glare.Data.ProductContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Product Product { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product = await _context.Products.FindAsync(id);

            if (Product == null)
            {
                return NotFound();
            }
            return Page();
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var productToUpdate = await _context.Products.FindAsync(id);
            if (productToUpdate == null)
            {
                return Page();
            }

            try
            {
                if (await TryUpdateModelAsync<Product>(productToUpdate, "Product", x=> x.Name, x=> x.Description, 
                    x=> x.Price, x=> x.Quantity, x=>x.Category))
                {
                    await _context.SaveChangesAsync();
                    return RedirectToPage("./Index");
                }
            }
            catch (DbUpdateException)
            {
                return Page();
            }

            //_context.Attach(Product).State = EntityState.Modified;

            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!ProductExists(Product.ProductId))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            return Page();
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}
