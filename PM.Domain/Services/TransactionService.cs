using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using PM.Data.Models;
using PM.Domain.Models;
using PM.Domain.Repositories;

namespace PM.Domain.Services
{
    public class TransactionService : BaseService<TransactionDTO, Transaction>, ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionService(ITransactionRepository repository, IMapper mapper)
            : base(repository, mapper)
        {
            _transactionRepository = (_repository as ITransactionRepository);
        }

        public async Task<List<TransactionDTO>> GetByDateAsync(DateTime fromDate, DateTime toDate)
        {
            var transactions = await _transactionRepository.GetByDateAsync(fromDate, toDate);
            return _mapper.Map<List<Transaction>, List<TransactionDTO>>(transactions);
        }
    }
}
