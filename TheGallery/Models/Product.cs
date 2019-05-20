using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        [Required]
        public string ImageURL { get; set; }

        public string Information { get; set; }

        public virtual List<CartItem> CartItems { get; set; }

        public int ArtistId { get; set; }
        public virtual Artist Artist { get; set; }

        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}
