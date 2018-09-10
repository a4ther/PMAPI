using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PM.Data.Contexts;
using PM.Data.Models;

namespace PM.Domain.Repositories
{
    public class TransactionRepository : BaseRepository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(DataContext context)
            : base(context)
        {
        }

        public Task<List<Transaction>> GetByDateAsync(DateTime from, DateTime to)
        {
            return _context.Transactions.Where(t => t.Date >= from && t.Date <= to)
                           .Include(t => t.Category).ToListAsync();
        }

        public override async Task<Transaction> InsertAsync(Transaction entity)
        {
            entity = await base.InsertAsync(entity);
            return _context.Transactions.Include(t => t.Category).SingleOrDefault(t => t.ID == entity.ID);
        }
    }
}
