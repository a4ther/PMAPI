using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using PM.Data.Models;
using PM.Domain.Models;

namespace PM.Domain.Services
{
    public class BatchService : IBatchService
    {
        private readonly IBaseService<Batch> _service;
        private readonly ITransactionService _transactionService;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public BatchService(IBaseService<Batch> service, ITransactionService transactionService, ICategoryService categoryService, IMapper mapper)
        {
            _service = service;
            _transactionService = transactionService;
            _categoryService = categoryService;
            _mapper = mapper;
        }

        public async Task<BatchDTO> AddAsync(BatchDTO entry)
        {
            var batch = _mapper.Map<BatchDTO, Batch>(entry);
            batch = await _service.AddAsync(batch);

            await AddTransactionsAsync(batch, entry.Transactions, entry.AllowDuplicates, entry.ExcludeTransfers);

            batch = await _service.UpdateAsync(batch);
            return _mapper.Map<Batch, BatchDTO>(batch);
        }

        public async Task<List<BatchDTO>> GetByDateAsync(DateTime fromDate, DateTime toDate)
        {
            var batches = await _service.WhereAsync(b => b.DateAdded >= fromDate && b.DateAdded <= toDate);
            return _mapper.Map<List<Batch>, List<BatchDTO>>(batches);
        }

        public async Task<BatchDTO> GetByIdAsync(int id)
        {
            var batch = await _service.GetByIdAsync(id);
            return _mapper.Map<Batch, BatchDTO>(batch);
        }

        public async Task<BatchDTO> RemoveAsync(int id)
        {
            var batch = await _service.RemoveAsync(id);
            return _mapper.Map<Batch, BatchDTO>(batch);
        }

        public async Task<BatchDTO> UpdateAsync(BatchDTO entry)
        {
            var batch = _mapper.Map<BatchDTO, Batch>(entry);
            batch = await _service.UpdateAsync(batch);
            return _mapper.Map<Batch, BatchDTO>(batch);
        }

        private async Task AddTransactionsAsync(Batch batch, List<TransactionDTO> transactions, bool allowDuplicates, bool excludeTransfers)
        {
            if (excludeTransfers)
            {
                ExcludeTransfers(batch, ref transactions);
            }

            var storedTransactions = new List<TransactionDTO>();
            if (!allowDuplicates)
            {
                var fromDate = transactions.Min(t => t.Date);
                var toDate = transactions.Max(t => t.Date);
                storedTransactions = await _transactionService.GetByDateWithCategoryAsync(fromDate, toDate);
            }

            foreach (var transaction in transactions)
            {
                if (!allowDuplicates && storedTransactions.Any(t => t.Equals(transaction)))
                {
                    batch.Duplicated += 1;
                }
                else
                {
                    await AddTransactionAsync(transaction, batch);
                }
            }
        }

        private void ExcludeTransfers(Batch batch, ref List<TransactionDTO> transactions)
        {
            var totalTransactions = transactions.Count;
            transactions = transactions.Where(t => !Enum.IsDefined(typeof(TransferCategory), t.Category.Name)).ToList();
            batch.Excluded = totalTransactions - transactions.Count;
        }

        private async Task AddTransactionAsync(TransactionDTO transaction, Batch batch)
        {
            try
            {
                var category = await _categoryService.GetByNameAsync(transaction.Category.Name);

                if (category == null)
                {
                    batch.Failed += 1;
                }
                else
                {
                    transaction.BatchID = batch.ID;
                    transaction.Category = category;
                    await _transactionService.AddAsync(transaction);
                    batch.Added += 1;
                }
            }
            catch (Exception)
            {
                batch.Failed += 1;
            }
        }
    }
}
