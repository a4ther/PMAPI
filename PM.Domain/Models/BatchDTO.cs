using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace PM.Domain.Models
{
    public class BatchDTO : BaseDTO
    {
        public bool AllowDuplicates { get; set; }

        public bool ExcludeTransfers { get; set; }

        public DateTime Date { get; set; }

        public int Added { get; set; }

        public int Duplicated { get; set; }

        public int Excluded { get; set; }

        public int Failed { get; set; }

        public List<TransactionDTO> Transactions { get; set; }
    }
}
