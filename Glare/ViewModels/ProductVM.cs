using Glare.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Glare.ViewModels
{
    public class ProductVM
    {
        public int ProductId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public decimal? Price { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public Category? Category { get; set; }
        public ICollection<IFormFile> Photos { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
