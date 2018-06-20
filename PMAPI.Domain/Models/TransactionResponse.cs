using System;
using System.ComponentModel.DataAnnotations;

namespace PMAPI.Domain.Models
{
    public class TransactionResponse : BaseResponse, IEquatable<TransactionResponse>
    {
        public CategoryResponse Category { get; set; }

        public DateTime Date { get; set; }

        public Money Money { get; set; }

        public string Wallet { get; set; }

        public bool Equals(TransactionResponse other)
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

            var transactionObj = obj as TransactionResponse;
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

        public static bool operator == (TransactionResponse transaction1, TransactionResponse transaction2)
        {
            if (Object.ReferenceEquals(transaction1, null))
            {
                return Object.ReferenceEquals(transaction2, null);
            }

            return transaction1.Equals(transaction2);
        }

        public static bool operator != (TransactionResponse transaction1, TransactionResponse transaction2)
        {
            return !(transaction1 == transaction2);
        }
    }
}
