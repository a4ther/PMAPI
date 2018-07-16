using Microsoft.EntityFrameworkCore;
using PM.Data.Models;

namespace PM.Data.Contexts
{
	public class DataContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }

        public DbSet<Batch> Batches { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        public DataContext(DbContextOptions<DataContext> options) :base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Batch>()
                        .HasMany(b => b.Transactions)
                        .WithOne()
                        .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
