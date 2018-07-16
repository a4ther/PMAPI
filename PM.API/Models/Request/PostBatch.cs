using System;
using System.Collections.Generic;
using PM.Domain.Models;

namespace PM.API.Models.Request
{
    public class PostBatch
    {
        public bool AllowDuplicates { get; set; }

        public bool ExcludeTransfers { get; set; }

        public List<PostBatchTransaction> Transactions { get; set; }
    }
}
