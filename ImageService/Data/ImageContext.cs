using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ImageService.Models;

namespace ImageService.Data
{
    public class ImageContext : DbContext
    {
        public ImageContext(DbContextOptions<ImageContext> options) : base(options) { }

        public DbSet<Image> Images { get; set; }
    }
}
