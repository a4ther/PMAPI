using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PM.Data.Models;

namespace PM.Domain.Repositories
{
    public interface ITransactionRepository : IBaseRepository<Transaction>
    {
        Task<List<Transaction>> GetByDateWithCategoryAsync(DateTime from, DateTime to);
    }
}
