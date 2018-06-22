using Microsoft.EntityFrameworkCore;
using PM.Data.Models;

namespace PM.Data.Contexts
{
	public class DataContext : DbContext
    {

        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DataContext(DbContextOptions<DataContext> options) :base(options)
        {
            
        }
    }
}
