using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using PMAPI.Data.Models;
using PMAPI.Domain.Models;

namespace PMAPI.Domain.Services
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

        public async Task<TransactionResponse> AddAsync(TransactionResponse entry)
        {
            var transaction = _mapper.Map<TransactionResponse, Transaction>(entry);
            transaction = await _service.AddAsync(transaction);
            return _mapper.Map<Transaction, TransactionResponse>(transaction);
        }

        public async Task<List<TransactionResponse>> GetAsync()
        {
            var result = await _service.GetAsync();
            return _mapper.Map<List<Transaction>, List<TransactionResponse>>(result);
        }

        public async Task<TransactionResponse> GetByIdAsync(int id)
        {
            var result = await _service.GetByIdAsync(id);
            return _mapper.Map<Transaction, TransactionResponse>(result);
        }

        public async Task<TransactionResponse> RemoveAsync(int id)
        {
            var transaction = await _service.RemoveAsync(id);
            return _mapper.Map<Transaction, TransactionResponse>(transaction);
        }

        public async Task<TransactionResponse> UpdateAsync(TransactionResponse entry)
        {
            var transaction = _mapper.Map<TransactionResponse, Transaction>(entry);
            transaction = await _service.UpdateAsync(transaction);
            return _mapper.Map<Transaction, TransactionResponse>(transaction);
        }

        public async Task<List<TransactionResponse>> WhereAsync(Expression<Func<Transaction, bool>> exp)
        {
            var result = await _service.WhereAsync(exp);
            return _mapper.Map<List<Transaction>, List<TransactionResponse>>(result);
        }
    }
}
