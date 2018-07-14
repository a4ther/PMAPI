using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PM.Data.Contexts;
using PM.Data.Models;

namespace PM.Domain.Repositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(DataContext context)
            : base(context)
        {
        }

        public override Task<Category> GetByIdAsync(int id)
        {
            return _context.Categories.Include(c => c.Parent).SingleOrDefaultAsync(e => e.ID == id);
        }
    }
}
