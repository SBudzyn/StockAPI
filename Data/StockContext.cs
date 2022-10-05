using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore;
using StockAPI.Models;

namespace StockAPI.Data
{
    public class StockContext : DbContext
    {
        public DbSet<Author> Authors { get; set; } = null!;
        public DbSet<Photo> Photos { get; set; } = null!;
        public DbSet<Text> Texts { get; set; } = null!;
        public StockContext(DbContextOptions<StockContext> context) : base(context)
        {

        }
    }
}
