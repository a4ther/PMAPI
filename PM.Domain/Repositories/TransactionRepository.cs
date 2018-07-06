﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PM.Data.Contexts;
using PM.Data.Models;

namespace PM.Domain.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly DataContext _context;

        public TransactionRepository(DataContext context)
        {
            _context = context;
        }

        public Task<List<Transaction>> GetByDateWithCategoryAsync(DateTime from, DateTime to)
        {
            return _context.Transactions.Where(t => t.Date >= from && t.Date <= to).Include(t => t.Category).ToListAsync();
        }
    }
}
