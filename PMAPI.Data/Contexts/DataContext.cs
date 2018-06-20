using Microsoft.EntityFrameworkCore;
using PMAPI.Data.Models;

namespace PMAPI.Data.Contexts
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
