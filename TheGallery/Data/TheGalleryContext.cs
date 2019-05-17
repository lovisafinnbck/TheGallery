using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TheGallery.Models;

namespace TheGallery.Models
{
    public class TheGalleryContext : DbContext
    {
        public TheGalleryContext (DbContextOptions<TheGalleryContext> options)
            : base(options)
        {
        }

        public DbSet<TheGallery.Models.Artist> Artist { get; set; }

        public DbSet<TheGallery.Models.Cart> Cart { get; set; }

        public DbSet<TheGallery.Models.CartItem> CartItem { get; set; }

        public DbSet<TheGallery.Models.Category> Category { get; set; }

        public DbSet<TheGallery.Models.Customer> Customer { get; set; }

        public DbSet<TheGallery.Models.Order> Order { get; set; }

        public DbSet<TheGallery.Models.Product> Product { get; set; }
    }
}
