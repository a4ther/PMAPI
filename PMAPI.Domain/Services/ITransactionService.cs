using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using PMAPI.Data.Models;
using PMAPI.Domain.Models;

namespace PMAPI.Domain.Services
{
    public interface ITransactionService
    {
        Task<List<TransactionResponse>> GetAsync();

        Task<TransactionResponse> GetByIdAsync(int id);

        Task<List<TransactionResponse>> WhereAsync(Expression<Func<Transaction, bool>> exp);

        Task<TransactionResponse> AddAsync(TransactionResponse entry);

        Task<TransactionResponse> UpdateAsync(TransactionResponse entry);

        Task<TransactionResponse> RemoveAsync(int id);
    }
}
