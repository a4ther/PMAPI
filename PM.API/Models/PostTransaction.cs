using System;
using PM.Domain.Models;

namespace PM.API.Models
{
    public class PostTransaction
    {
        public decimal Amount { get; set; }

        public int CategoryID { get; set; }

        public CurrencyResponse Currency { get; set; }

        public DateTime Date { get; set; }

        public string Wallet { get; set; }
    }
}
