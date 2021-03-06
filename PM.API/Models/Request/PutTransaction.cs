﻿using System;
using PM.Domain.Models;

namespace PM.API.Models.Request
{
    public class PutTransaction
    {
        public int ID { get; set; }

        public decimal Amount { get; set; }

        public int CategoryID { get; set; }

        public Currency Currency { get; set; }

        public DateTime Date { get; set; }

        public string Wallet { get; set; }
    }
}
