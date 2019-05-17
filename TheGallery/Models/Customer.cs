using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TheGallery.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(30, MinimumLength =3)]
        public string Name { get; set; }

        public int CartId { get; set; }
        public virtual Cart Cart { get; set; }
    }
}
