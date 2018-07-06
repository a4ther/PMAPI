using System;
using System.IO;
using System.Linq;
using CsvHelper;
using Newtonsoft.Json;
using PM.CSVtoJSON.Models;
using PM.API.Models.Request;

namespace PM.CSVtoJSON
{
    public class Program
    {
        private static string _filePath;
        private static PostBatch _request;

        public static void Main(string[] args)
        {
            _filePath = "";
#if DEBUG
            _filePath = "/Users/leealfarosoto/Downloads/Test.csv";
#else
            if (args.Length > 0) 
            {
                _filePath = args[0];
            }
#endif
            _request = new PostBatch
            {
                AllowDuplicates = false,
                ExcludeTransfers = true
            };

            ValidateFilePath();
            ReadCSV();
            WriteJSON();

            Console.WriteLine("Pres any key to exit...");
            Console.ReadKey();
        }

        private static void ValidateFilePath()
        {
            while(!File.Exists(Path.GetFullPath(_filePath)))
            {
                Console.WriteLine("The path of the CSV file is invalid...");
                Console.WriteLine("Enter the path of the CSV file to convert...");
                _filePath = Console.ReadLine();
            }
        }

        private static void ReadCSV()
        {
            using(var reader = new StreamReader(_filePath))
            {
                Console.WriteLine($"Reading {Path.GetFullPath(_filePath)} file...");
                var csv = new CsvReader(reader);
                csv.Configuration.HasHeaderRecord = true;
                csv.Configuration.RegisterClassMap<CSVTransactionMap>();
                _request.Transactions = csv.GetRecords<PostBatchTransaction>().ToList();
                Console.WriteLine($"Succesfully read {_request.Transactions.Count} transactions from {Path.GetFileName(_filePath)}...");
            }
        }

        private static void WriteJSON()
        {
            var fileName = $"{Path.GetFileNameWithoutExtension(_filePath)}.txt";
            var directoryName = Path.GetDirectoryName(_filePath);

            using (var writer = File.CreateText(Path.Combine(directoryName, fileName)))
            {
                Console.WriteLine($"Writing {Path.Combine(directoryName, fileName)} file...");
                JsonSerializer serializer = new JsonSerializer();
                serializer.Formatting = Formatting.Indented;
                serializer.Serialize(writer, _request);
                Console.WriteLine($"Succesfully wrote {_request.Transactions.Count} transactions to {fileName}...");
            }
        }
    }
}
