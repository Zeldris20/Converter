using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using Newtonsoft.Json;

class Program
{
    static void Main()
    {
        Console.WriteLine("Enter the path of the directory containing JSON files:");
        string directoryPath = Console.ReadLine();

        // Check if the directory exists
        if (!Directory.Exists(directoryPath))
        {
            Console.WriteLine("Directory not found. Exiting...");
            return;
        }

        // Get all JSON files in the directory
        var jsonFiles = Directory.GetFiles(directoryPath, "*.json");

        if (jsonFiles.Length == 0)
        {
            Console.WriteLine("No JSON files found in the directory. Exiting...");
            return;
        }

        foreach (var jsonFilePath in jsonFiles)
        {
            Console.WriteLine($"Converting {jsonFilePath} to CSV...");

            // Read JSON content
            string jsonContent = File.ReadAllText(jsonFilePath);

            // Deserialize the JSON object
            var jsonObject = JsonConvert.DeserializeObject<Dictionary<string, List<Apple>>>(jsonContent);

            if (jsonObject.TryGetValue("fruit", out var records))
            {
                // Generate CSV file path by replacing .json extension with .csv
                string csvFilePath = Path.ChangeExtension(jsonFilePath, "csv");

                // Write to CSV
                using (var writer = new StreamWriter(csvFilePath))
                using (var csv = new CsvWriter(writer, new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture)))
                {
                    csv.WriteRecords(records);
                }

                Console.WriteLine($"CSV file written to {csvFilePath}");
            }
            else
            {
                Console.WriteLine($"Skipping {jsonFilePath}. The JSON does not contain the expected 'fruit' property or has an invalid format.");
            }
        }
    }
}
