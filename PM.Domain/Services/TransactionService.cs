using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using PM.Data.Models;
using PM.Domain.Models;

namespace PM.Domain.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IBaseService<Transaction> _service;
        private readonly IMapper _mapper;

        public TransactionService(IBaseService<Transaction> service, IMapper mapper)
        {
            _service = service;
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
            var result = await _service.GetAsync();
            return _mapper.Map<List<Transaction>, List<TransactionDTO>>(result);
        }


        public async Task<List<TransactionDTO>> GetByDateAsync(DateTime fromDate, DateTime toDate)
        {
            var result = await _service.WhereAsync(e => e.Date >= fromDate && e.Date <= toDate);
            return _mapper.Map<List<Transaction>, List<TransactionDTO>>(result);
        }

        public async Task<TransactionDTO> GetByIdAsync(int id)
        {
            var result = await _service.GetByIdAsync(id);
            return _mapper.Map<Transaction, TransactionDTO>(result);
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
