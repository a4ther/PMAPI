using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PM.Data.Models
{
    [Table("Transaction")]
    public class Transaction : BaseEntity
    {
        [ForeignKey("Batch")]
        public int? BatchID { get; set; }

        [ForeignKey("Category")]
        [Required(ErrorMessage = "Category ID is required")]
        public int CategoryID { get; set; }

        public virtual Category Category { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Currency is required")]
        public int Currency { get; set; }

        [Required(ErrorMessage = "Date is required")]
        public DateTime Date { get; set; }

        [MaxLength(10, ErrorMessage = "Wallet must be 10 characters or less")]
        public string Wallet { get; set; }
    }
}
