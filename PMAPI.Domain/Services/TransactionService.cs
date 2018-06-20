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

        public void AddOrUpdate(TransactionResponse entry)
        {
            _service.AddOrUpdate(_mapper.Map<TransactionResponse, Transaction>(entry));
        }

        public async Task<IEnumerable<TransactionResponse>> GetAsync()
        {
            var result = await _service.GetAsync();
            return _mapper.Map<IEnumerable<Transaction>, IEnumerable<TransactionResponse>>(result);
        }

        public async Task<TransactionResponse> GetByIdAsync(int id)
        {
            var result = await _service.GetByIdAsync(id);
            return _mapper.Map<Transaction, TransactionResponse>(result);
        }

        public void Remove(int id)
        {
            _service.Remove(id);
        }

        public IEnumerable<TransactionResponse> Where(Expression<Func<Transaction, bool>> exp)
        {
            var result = _service.Where(exp);
            return _mapper.Map<IEnumerable<Transaction>, IEnumerable<TransactionResponse>>(result);
        }
    }
}
