using Glare.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Glare.ViewModels
{
    public class CartVM
    {
        public int ItemId { get; set; }

        //CartId specifies the Id of the User
        public string CartId { get; set; }
        public int Quantity { get; set; }
        public Product Product { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
