using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Glare.Data;
using Glare.Models;
using Glare.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace Glare
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly Glare.Data.ProductContext _context;

        public IndexModel(Glare.Data.ProductContext context)
        {
            _context = context;
        }

        public IList<Product> Product { get;set; }
        public PaginatedList<Product> Products { get; set; }
        public Category? CurrentFilter { get; set; }
        public int PageSize { get; set; } = 5;

        public async Task OnGetAsync(Category? currentFilter,Category? category, int? currentPage)
        {
            IQueryable<Product> productIQ = from m in _context.Products
                                          select m;

            //Product = await _context.Products.ToListAsync();
            CurrentFilter = category;

            if (category != null)
            {
                currentPage = 1;
                productIQ = productIQ.Where(s => s.Category == category);
            }
            else
            {
                currentFilter = category;
            }

            int pageSize = PageSize;
            Products = await PaginatedList<Product>.ItemAsync(productIQ.AsNoTracking(), currentPage ?? 1, pageSize);
            //Product = await productIQ.AsNoTracking().ToListAsync();
        }
    }
}
