using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bojan
{
    class Output
    {
        public int MaxNumber { get; set; }
        public double TotalMiliseconds { get; set; }
    }
    class Program
    {
        static Stopwatch stopwatch = new Stopwatch();

        public static Output GetMax(int[] items)
        {
            stopwatch.Restart();
            var r = items.Max();
            stopwatch.Stop();
            return new Output
            {
                TotalMiliseconds = stopwatch.Elapsed.TotalMilliseconds,
                MaxNumber = r
            };
        }

        public static Output GetMaxPLINQ(int[] items, int partitionCount)
        {
            stopwatch.Restart();
            //TODO: partitioning array of items
            var r = GetPartitionDummyData()
                .AsParallel()
                .Select(i => i.Max())
                .Max();

            stopwatch.Stop();
            return new Output
            {
                TotalMiliseconds = stopwatch.Elapsed.TotalMilliseconds,
                MaxNumber = r
            };
        }

        static void Main(string[] args)
        {
            var r = GetMax(GetData());
            Console.WriteLine($"Max item: {r.MaxNumber}; Total time: {r.TotalMiliseconds} miliseconds");

            r = GetMaxPLINQ(GetData(), 5000);
            Console.WriteLine($"PLINQ; Max item: {r.MaxNumber}; Total time: {r.TotalMiliseconds} miliseconds");
            Console.ReadKey();
        }

        private static int[] GetData()
        {
            int[] items = new int[10000];

            Random randNum = new Random();
            for (int i = 0; i < items.Length; i++)
            {
                items[i] = randNum.Next(0, 10000);
            }

            return items;
        }

        private static int[][] GetPartitionDummyData()
        {
            int[][] jaggedArray = new int[3][];
            jaggedArray[0] = new int[5] { 1, 2, 3, 100, 3 };
            jaggedArray[1] = new int[5] { 1, 2, 3, 122, 3 };
            jaggedArray[2] = new int[3] { 1, 2, 3 };

            return jaggedArray;
        }
    }
}
