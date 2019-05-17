using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TheGallery.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public int Click { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        [StringLength(30, MinimumLength =3)]
        public string Name { get; set; }
        
        public string Information { get; set; }

        public virtual List<CartItem> CartItems { get; set; }
    }
}
