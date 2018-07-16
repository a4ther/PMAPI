using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using PM.Data.Models;
using PM.Domain.Models;
using PM.Domain.Repositories;

namespace PM.Domain.Services
{
    public class BatchService : BaseService<BatchDTO, Batch>, IBatchService
    {
        private readonly ITransactionService _transactionService;
        private readonly ICategoryService _categoryService;
        private readonly ILogger<BatchService> _logger;

        public BatchService(IBatchRepository repository, ITransactionService transactionService, ICategoryService categoryService, IMapper mapper, ILogger<BatchService> logger)
            : base(repository, mapper)
        {
            _transactionService = transactionService;
            _categoryService = categoryService;
            _logger = logger;
        }

        public override async Task<BatchDTO> AddAsync(BatchDTO entry)
        {
            var prefix = "[AddAsync]";

            var batch = await base.AddAsync(entry);
            _logger.LogInformation($"{prefix} Successfully added batch with id {batch.ID}");

            await AddTransactionsAsync(batch, entry.Transactions, entry.AllowDuplicates, entry.ExcludeTransfers);

            batch = await base.UpdateAsync(batch);
            _logger.LogInformation($"{prefix} Successfully updated batch with id {batch.ID}");

            return batch;
        }

        public Task<List<BatchDTO>> GetByDateAsync(DateTime fromDate, DateTime toDate)
        {
            return base.WhereAsync(b => b.DateAdded >= fromDate && b.DateAdded <= toDate);
        }

        private async Task AddTransactionsAsync(BatchDTO batch, List<TransactionDTO> transactions, bool allowDuplicates, bool excludeTransfers)
        {
            var prefix = "[AddTransactionsAsync]";

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
                _logger.LogInformation($"{prefix} Successfully retrieved {storedTransactions.Count} transactions from store");
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

            _logger.LogInformation($"{prefix} Successfully excluded {batch.Duplicated} duplicated transactions");
            _logger.LogInformation($"{prefix} Failed adding {batch.Failed} transactions to batch");
            _logger.LogInformation($"{prefix} Successfully added {batch.Added} transactions to batch");
        }

        private void ExcludeTransfers(BatchDTO batch, ref List<TransactionDTO> transactions)
        {
            var prefix = "[ExcludeTransfers]";

            var totalTransactions = transactions.Count;
            transactions = transactions.Where(t => !Enum.IsDefined(typeof(TransferCategory), t.Category.Name)).ToList();

            batch.Excluded = totalTransactions - transactions.Count;
            _logger.LogInformation($"{prefix} Successfully excluded {batch.Excluded} transfers from batch");
        }

        private async Task AddTransactionAsync(TransactionDTO transaction, BatchDTO batch)
        {
            var prefix = "[AddTransactionAsync]";

            try
            {
                var category = await _categoryService.GetByNameAsync(transaction.Category.Name);

                if (category == null)
                {
                    batch.Failed += 1;
                    _logger.LogWarning($"{prefix} Unable to find category with name {transaction.Category.Name}");
                }
                else
                {
                    transaction.BatchID = batch.ID;
                    transaction.Category = category;
                    transaction = await _transactionService.AddAsync(transaction);
                    _logger.LogInformation($"{prefix} Successfully added transaction with id {transaction.ID} to batch");
                    batch.Added += 1;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{prefix} {ex.GetType().Name}  exception while adding transaction to batch");
                _logger.LogError(ex.StackTrace);
                batch.Failed += 1;
            }
        }
    }
}
