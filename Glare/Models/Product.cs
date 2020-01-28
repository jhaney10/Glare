using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Glare.Models
{
    public enum Category
    {
        [Display(Name = "Select One")]
        None,
        [Display(Name = "Bags")]
        Bags,
        [Display(Name = "Shoes")]
        Shoes,
        [Display(Name = "Dresses")]
        Dresses,
        [Display(Name = "Accessories")]
        Accessories
    }
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}")]
        [DataType(DataType.Currency)]
        public decimal? Price { get; set; }
        public int Quantity { get; set; }
        public Category? Category { get; set; }
        public ICollection<Photo> Photo { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
