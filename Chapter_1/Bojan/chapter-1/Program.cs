using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Bojan
{
    public delegate int GetMax(int[] items, int partitionCount);

    class Program
    {
        static Stopwatch stopwatch = new Stopwatch();
        static int[] items = DummyData.Get();
        static int partitionCount = 10;

        public static void Banchmark(GetMax d2)
        {
            stopwatch.Restart();
            var r = d2(items, partitionCount);
            stopwatch.Stop();
            Console.WriteLine($"{d2.Method.Name}; Max item: {r}; Total time: {stopwatch.Elapsed.TotalMilliseconds} miliseconds");
        }

        public static int GetMax(int[] items)
        {
            var r = items.Max();
            return r;
        }        

        static void Main(string[] args)
        {
            GetMax max = Parallel.MaxTasks;
            Banchmark(max);

            max = Parallel.MaxLinq;
            Banchmark(max);

            //max = Parallel.MaxThreads;
            //Banchmark(max);

            Console.ReadKey();
        }

        private static IEnumerable<IEnumerable<int>> GetPartitionDummyData()
        {
            int[][] jaggedArray = new int[3][];
            jaggedArray[0] = new int[5] { 1, 2, 3, 100, 3 };
            jaggedArray[1] = new int[5] { 1, 2, 3, 122, 3 };
            jaggedArray[2] = new int[3] { 1, 2, 3 };

            return jaggedArray.AsEnumerable();
        }
    }
}
