using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TheGallery.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }

        public virtual List<CartItem> CartItems { get; set; }
    }
}
