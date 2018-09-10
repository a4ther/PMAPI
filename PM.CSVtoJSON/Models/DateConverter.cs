using System;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace PM.CSVtoJSON.Models
{
    public class DateConverter : DateTimeConverter
    {
        private const String dateFormat = @"dd/MM/yyyy";

        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            DateTime newDate = default(System.DateTime);
            try
            {
                newDate = DateTime.ParseExact(text, dateFormat, CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(String.Format(@"Error parsing date '{0}': {1}", text, ex.Message));
            }

            return newDate;
        }
    }
}