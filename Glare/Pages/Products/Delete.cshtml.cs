using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Glare.Data;
using Glare.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;

namespace Glare
{
    [Authorize(Roles = "Admin")]
    public class DeleteModel : PageModel
    {
        private readonly Glare.Data.ProductContext _context;

        public DeleteModel(Glare.Data.ProductContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Product Product { get; set; }
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product = await _context.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ProductId == id);

            if (Product == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ErrorMessage = "The operation was unsuccessful";
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product = await _context.Products.FindAsync(id);

            if (Product != null)
            {
                try
                {
                    _context.Products.Remove(Product);
                    await _context.SaveChangesAsync();
                }
                catch(DbUpdateException ex)
                {
                    EventLog.WriteEntry("GlareShop", ex.Message, EventLogEntryType.Warning);
                    return RedirectToAction("./Delete", new { id = id, saveChangesError = true});
                }
               
            }
            else
            {
                return NotFound();
            }

            return RedirectToPage("./Index");
        }
    }
}
