using System;
using System.Text.RegularExpressions;
using CsvHelper.Configuration;
using PM.API.Models.Request;

namespace PM.CSVtoJSON.Models
{
    public class CSVTransactionMap : ClassMap<PostBatchTransaction>
    {
        public CSVTransactionMap()
        {
            Map(t => t.Amount).ConvertUsing(row => 
            {
                var amountString = Regex.Replace(row.GetField<string>("AMOUNT"), "[^-.0-9]", "");
                if (Decimal.TryParse(amountString, out var amountDecimal))
                {
                    return amountDecimal;
                }
                return 0;
            });
            Map(t => t.CategoryName).Name("CATEGORY").Index(1);
            Map(t => t.Currency).Name("CURRENCY").Index(5);
            Map(t => t.Date).Name("DATE").Index(6);
            Map(t => t.Wallet).Name("WALLET").Index(4);
        }
    }
}
