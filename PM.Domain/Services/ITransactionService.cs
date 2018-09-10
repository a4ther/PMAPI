using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PM.Data.Models;
using PM.Domain.Models;

namespace PM.Domain.Services
{
    public interface ITransactionService : IBaseService<TransactionDTO, Transaction>
    {
        Task<List<TransactionDTO>> GetByDateAsync(DateTime fromDate, DateTime toDate);
    }
}
