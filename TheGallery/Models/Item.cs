using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheGallery.Models
{
    // Model for an item in cart
    public class Item
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}
