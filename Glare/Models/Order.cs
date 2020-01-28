using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Glare.Models
{
    public enum OrderStatus
    {
        Processing,
        Delivered,
        FailedDelivery,
        Cancelled
    }
    public class Order
    {
        public int OrderId { get; set; }
        [Display(Name = "Order Date")]
        public DateTime OrderDate { get; set; }
        public IdentityUser Customer { get; set; }
        public ICollection<CartItem> OrderItems { get; set; }
        [Display(Name = "Order Status")]
        public OrderStatus? OrderStatus { get; set; }
        [Display(Name = "Delivery Address")]
        public string DeliveryAddress { get; set; }
        public DateTime DeliveryDate { get; set; }
    }
}
