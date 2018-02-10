using System;
using System.IO;
using System.Linq;

namespace TestDataGenerator
{
    class Generator
    {
        private const String TestDataFileName = "test_data.txt";
        private static int Max = 12;
        private const int Min = 0;

        private static int[] ReadFromFile()
        {
            string data = File.ReadAllText(TestDataFileName);
            string[] tokens = data.Split(',');

            return Array.ConvertAll(tokens, int.Parse);
        }

        private static void SaveToFile(int[] arr)
        {
            string content = String.Join(",", arr.Select(p => p.ToString()).ToArray());
            File.WriteAllText(TestDataFileName, content);
        }

        private static int[] GenerateIntArray()
        {
            Random randNum = new Random();
            return Enumerable
                .Repeat(Min, Max)
                .Select(i => randNum.Next(Min, Max))
                .ToArray();
        }

        private static int[] GenerateFromCache()
        {
            int[] data = null;

            if (File.Exists(TestDataFileName))
            {
                try
                {
                    data = ReadFromFile();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    data = GenerateIntArray();
                }
            }
            else
            {
                data = GenerateIntArray();
                SaveToFile(data);
            }
            return data;

        }

        public static void ClearCache()
        {
            File.Delete(TestDataFileName);
        }

        public static int[] GenerateNumbers(int max, bool refreshData = false)
        {
            Max = max;

            if (refreshData)
            {
                return GenerateIntArray();
            }
            return GenerateFromCache();
        }
    }
}
