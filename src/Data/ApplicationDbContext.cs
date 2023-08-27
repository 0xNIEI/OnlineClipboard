using Microsoft.EntityFrameworkCore;
using OnlineClipboard.Models;

namespace OnlineClipboard.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> option) : base(option) 
        { 
        }

        public DbSet<Entry> Entry { get; set; }
    }
}
