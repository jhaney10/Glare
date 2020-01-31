using Glare.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Glare.ViewModels
{
    public class PlaceOrderVM
    {
        public PlaceOrderVM()
        {
            OrderItems = new List<CartItem>();
        }
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public List<CartItem> OrderItems { get; set; }
        public OrderStatus? OrderStatus { get; set; }
        public AppUser Customer { get; set; }
        [Required]
        public string DeliveryAddress { get; set; }
    }
}
