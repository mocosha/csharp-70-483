using System;
using System.Diagnostics;

namespace Bojan
{
    public delegate int GetMax(int[] items, int partitionCount);

    class Program
    {
        static Stopwatch stopwatch = new Stopwatch();
        //static int[] items = DummyData.Get();
        static int[] items = Generator.GenerateNumbers(1000000, refreshData: true);

        static int partitionCount = 10;

        public static void Banchmark(GetMax max)
        {
            foreach (var item in max.GetInvocationList())
            {
                stopwatch.Restart();
                var r = item.DynamicInvoke(items, partitionCount);
                stopwatch.Stop();
                Console.WriteLine($"Method: {item.Method.Name}; Max item: {r}; Total time: {stopwatch.Elapsed.TotalMilliseconds} miliseconds");
            }
        }

        static void Main(string[] args)
        {
            GetMax max = Parallel.MaxTasks;
            max += Parallel.MaxLinq;
            max += Parallel.MaxThreads;

            Banchmark(max);

            Console.ReadKey();
        }
    }
}
