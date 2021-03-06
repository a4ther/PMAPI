﻿using System;
using Newtonsoft.Json;

namespace PM.Domain.Models
{
    public class TransactionDTO : BaseDTO, IEquatable<TransactionDTO>
    {
        [JsonIgnore]
        public int? BatchID { get; set; }

        public CategoryDTO Category { get; set; }

        public DateTime Date { get; set; }

        public MoneyDTO Money { get; set; }

        public string Wallet { get; set; }

        public bool Equals(TransactionDTO other)
        {
            if (other == null)
            {
                return false;
            }

            if (this.ID != 0 && other.ID != 0)
            {
                return this.ID.Equals(other.ID);
            }

            return this.Category == other.Category && this.Date == other.Date && 
                       this.Money == other.Money && this.Wallet == other.Wallet;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            var transactionObj = obj as TransactionDTO;
            if (transactionObj == null)
            {
                return false;
            }
            return Equals(transactionObj);
        }

        public override int GetHashCode()
        {
            if (this.ID != 0)
            {
                return this.ID.GetHashCode();
            }

            var hastString = $"{this.Category}{this.Date}{this.Money}{this.Wallet}";
            return hastString.GetHashCode();
        }

        public static bool operator == (TransactionDTO transaction1, TransactionDTO transaction2)
        {
            if (Object.ReferenceEquals(transaction1, null))
            {
                return Object.ReferenceEquals(transaction2, null);
            }

            return transaction1.Equals(transaction2);
        }

        public static bool operator != (TransactionDTO transaction1, TransactionDTO transaction2)
        {
            return !(transaction1 == transaction2);
        }
    }
}
