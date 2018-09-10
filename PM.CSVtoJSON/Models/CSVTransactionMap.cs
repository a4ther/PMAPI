using System;
using System.Text.RegularExpressions;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using PM.API.Models.Request;

namespace PM.CSVtoJSON.Models
{
    public class CSVTransactionMap : ClassMap<PostBatchTransaction>
    {
        public CSVTransactionMap()
        {
            var dateTimeConverter = new DateConverter();

            Map(t => t.Amount).ConvertUsing(row => 
            {
                var amountString = Regex.Replace(row.GetField<string>("Amount"), "[^-.0-9]", "");
                if (Decimal.TryParse(amountString, out var amountDecimal))
                {
                    return amountDecimal;
                }
                return 0;
            });
            Map(t => t.CategoryName).Name("Category").Index(3);
            Map(t => t.Currency).Name("Currency").Index(5);
            Map(t => t.Date).Name("Date").Index(6).TypeConverter(dateTimeConverter);
            Map(t => t.Wallet).Name("Account").Index(4);
        }
    }
}
