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
        Task<IEnumerable<TransactionResponse>> GetAsync();

        Task<TransactionResponse> GetByIdAsync(int id);

        IEnumerable<TransactionResponse> Where(Expression<Func<Transaction, bool>> exp);

        void AddOrUpdate(TransactionResponse entry);

        void Remove(int id);
    }
}
