using System;
using System.Text.RegularExpressions;
using CsvHelper.Configuration;
using PMAPI.Data.Models;

namespace PMAPI.CSVtoJSON.Models
{
    public class CSVTransactionMap : ClassMap<Transaction>
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
            //Map(t => t.Category).Name("CATEGORY").Index(1);
            Map(t => t.Currency).Name("CURRENCY").Index(5);
            Map(t => t.Date).Name("DATE").Index(6);
            Map(t => t.Wallet).Name("WALLET").Index(4);
        }
    }
}
