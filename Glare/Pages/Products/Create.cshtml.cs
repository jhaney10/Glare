using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Glare.Data;
using Glare.Models;
using Glare.ViewModels;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace Glare
{
    [Authorize(Roles ="Admin")]
    public class CreateModel : PageModel
    {
        private readonly ProductContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public CreateModel(ProductContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public ProductVM ProductVM { get; set; }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            //var emptyProduct = new Product();
            //if (await TryUpdateModelAsync(emptyProduct, "Product", p=> p.Name, p=> p.Description, p =>p.Category,
            //    p=>p.Price))
            //{
            //    _context.Products.Add(Product);
            //    await _context.SaveChangesAsync();

            //    return RedirectToPage("./Index");
            //}
            ProductVM.DateCreated = DateTime.Now;
            var entry = _context.Products.Add(new Product());
            entry.CurrentValues.SetValues(ProductVM);
            await _context.SaveChangesAsync();
            var id = entry.Entity.ProductId;



            string uniqueFileName = null;
            if (ProductVM.Photos != null && ProductVM.Photos.Count > 0)
            {

                foreach (IFormFile photo in ProductVM.Photos)
                {
                    string[] permittedExtensions = { ".jpg", ".png", ",jpeg" };
                    string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
                    var ext = Path.GetExtension(uniqueFileName).ToLowerInvariant();
                    if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
                    {
                        return Page();
                    }
                    else
                    {
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        photo.CopyTo(new FileStream(filePath, FileMode.Create));
                        var photoToAdd = new Photo();
                        photoToAdd.PhotoPath = uniqueFileName;
                        photoToAdd.ProductId = id;
                        _context.Photos.Add(photoToAdd);
                        await _context.SaveChangesAsync();

                    }
                }

            }

            return RedirectToPage("./Index");
        }
    }
}
