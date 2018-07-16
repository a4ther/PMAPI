using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PM.Data.Models
{
    [Table("Batch")]
    public class Batch : BaseEntity
    {
        [Required(ErrorMessage = "Number of added transactions required")]
        public int Added { get; set; }

        [Required(ErrorMessage = "Number of duplicated transactions required")]
        public int Duplicated { get; set; }

        [Required(ErrorMessage = "Number of excluded transactions required")]
        public int Excluded { get; set; }

        [Required(ErrorMessage = "Number of failed transactions required")]
        public int Failed { get; set; }

        public ICollection<Transaction> Transactions { get; set; }
    }
}
