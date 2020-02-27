using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Glare.Data;
using Glare.Models;
using Glare.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Glare.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ProductContext _context;

        public IndexModel(ILogger<IndexModel> logger, ProductContext context)
        {
            _logger = logger;
            _context = context;
        }
        public ICollection<Product> Products { get; set; }
        public PaginatedList<Product> PaginatedProducts { get; set; }
        public int PageSize { get; set; } = 30;
        public async Task OnGetAsync(int? currentPage)
        {
            //Sets the currency to Naira
            Thread.CurrentThread.CurrentCulture = new CultureInfo("yo-NG");

            IQueryable<Product> productIQ = from m in _context.Products
                                            select m;
            int pageSize = PageSize;
            PaginatedProducts = await PaginatedList<Product>.ItemAsync(productIQ.Include(m => m.Photo)
                .AsNoTracking(), currentPage ?? 1, pageSize);

            //Products = await _context.Products
            //    .Include(m => m.Photo)
            //    .AsNoTracking()
            //    .ToListAsync();
        }
    }
}
