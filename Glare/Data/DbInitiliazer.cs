using Glare.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Glare.Data
{
    public static class DbInitiliazer
    {
        public static void Initialize(ProductContext context)
        {
            context.Database.EnsureCreated();
            if (!context.Products.Any())
            {
                context.Products.AddRange(
                    new Product
                    {
                        Name = "Dior Bag",
                        Category = Category.Bags,
                        Price = 1000,
                        Quantity = 20
                    },
                    new Product 
                    { 
                        Name ="Gucci Sunglasses", 
                        Category = Category.Accessories, 
                        Quantity=10, 
                        Price= 15000},
                    new Product
                    {
                        Name = "Miu Miu Slippers",
                        Category = Category.Shoes,
                        Description = "Heeled Mules",
                        Price = 10000,
                        Quantity = 15
                    }
                    );
            }
            context.SaveChanges();
            if (!context.Photos.Any())
            {
                context.Photos.AddRange(
                    new Photo
                    {
                        ProductId = 1,
                        PhotoPath = "bag.jpg",
                    },
                    new Photo
                    {
                        ProductId = 1,
                        PhotoPath = "belt.jpg",
                    },
                    new Photo
                    {
                        ProductId = 1,
                        PhotoPath = "handcraft.jpg",
                    }
                    );
            }
            context.SaveChanges();


        }
    }
}
