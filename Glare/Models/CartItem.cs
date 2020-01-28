using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Glare.Models
{
    public class CartItem
    {
        [Key]
        public int ItemId { get; set; }

        //CartId specifies the Id of the User
        public string CartId { get; set; }
        public int Quantity { get; set; }
        public Product Product { get; set; }
        public DateTime DateCreated { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}")]
        [DataType(DataType.Currency)]
        public decimal? SubTotal { get; set; }
        public Order Order { get; set; }
         
    }
}
