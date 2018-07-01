using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using PM.Data.Models;
using PM.Domain.Models;

namespace PM.Domain.Services
{
    public interface ITransactionService
    {
        Task<List<TransactionDTO>> GetAsync();

        Task<TransactionDTO> GetByIdAsync(int id);

        Task<List<TransactionDTO>> GetByDateAsync(DateTime fromDate, DateTime toDate);

        Task<TransactionDTO> AddAsync(TransactionDTO entry);

        Task<TransactionDTO> UpdateAsync(TransactionDTO entry);

        Task<TransactionDTO> RemoveAsync(int id);
    }
}
