using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Glare.Models
{
    public class Photo
    {
        public int PhotoId { get; set; }
        public int ProductId { get; set; }
        public string PhotoPath { get; set; }
    }
}
