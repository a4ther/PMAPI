using System;
using System.ComponentModel.DataAnnotations;
using PMAPI.Data.Models;

namespace PMAPI.Models.Transactions
{
    public class TransactionRequest
    {
        public int ID { get; set; }
        
        [Required(ErrorMessage = "Category ID is required")]
        public int CategoryID { get; set; }

        [Required(ErrorMessage = "Date is required")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Currency is required")]
        [EnumDataType(typeof(Currency), ErrorMessage = "Currency value is not valid")]
        public Currency Currency { get; set; }

        [MaxLength(10, ErrorMessage = "Wallet must be 10 characters or less")]
        public string Wallet { get; set; }
    }
}
