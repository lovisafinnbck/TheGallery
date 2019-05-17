using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TheGallery.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Date of Birthday")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required]
        [StringLength(50, MinimumLength =5)]
        public string Address { get; set; }

        [Required]
        public double TotalPrice { get; set; }

        // cardinality
        public int CartId { get; set; }
        public virtual Cart Cart { get; set; }
    }
}
