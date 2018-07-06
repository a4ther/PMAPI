using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using PM.Data.Models;
using PM.Domain.Models;
using PM.Domain.Repositories;

namespace PM.Domain.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IBaseService<Transaction> _service;
        private readonly ITransactionRepository _repository;
        private readonly IMapper _mapper;

        public TransactionService(IBaseService<Transaction> service, ITransactionRepository repository, IMapper mapper)
        {
            _service = service;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<TransactionDTO> AddAsync(TransactionDTO entry)
        {
            var transaction = _mapper.Map<TransactionDTO, Transaction>(entry);
            transaction = await _service.AddAsync(transaction);
            return _mapper.Map<Transaction, TransactionDTO>(transaction);
        }

        public async Task<List<TransactionDTO>> GetAsync()
        {
            var transactions = await _service.GetAsync();
            return _mapper.Map<List<Transaction>, List<TransactionDTO>>(transactions);
        }

        public async Task<List<TransactionDTO>> GetByDateAsync(DateTime fromDate, DateTime toDate)
        {
            var transactions = await _service.WhereAsync(e => e.Date >= fromDate && e.Date <= toDate);
            return _mapper.Map<List<Transaction>, List<TransactionDTO>>(transactions);
        }

        public async Task<List<TransactionDTO>> GetByDateWithCategoryAsync(DateTime fromDate, DateTime toDate)
        {
            var transactions = await _repository.GetByDateWithCategoryAsync(fromDate, toDate);
            return _mapper.Map<List<Transaction>, List<TransactionDTO>>(transactions);
        }

        public async Task<TransactionDTO> GetByIdAsync(int id)
        {
            var transaction = await _service.GetByIdAsync(id);
            return _mapper.Map<Transaction, TransactionDTO>(transaction);
        }

        public async Task<TransactionDTO> RemoveAsync(int id)
        {
            var transaction = await _service.RemoveAsync(id);
            return _mapper.Map<Transaction, TransactionDTO>(transaction);
        }

        public async Task<TransactionDTO> UpdateAsync(TransactionDTO entry)
        {
            var transaction = _mapper.Map<TransactionDTO, Transaction>(entry);
            transaction = await _service.UpdateAsync(transaction);
            return _mapper.Map<Transaction, TransactionDTO>(transaction);
        }
    }
}
