using System;
using Newtonsoft.Json;
using PM.Domain.Models;

namespace PM.API.Models.Request
{
    public class PostBatchTransaction
    {
        public decimal Amount { get; set; }

        [JsonProperty(PropertyName = "Category")]
        public string CategoryName { get; set; }

        public Currency Currency { get; set; }

        public DateTime Date { get; set; }

        public string Wallet { get; set; }
    }
}
