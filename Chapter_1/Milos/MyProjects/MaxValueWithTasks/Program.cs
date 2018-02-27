using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MaxValueLibrary;
using TestDataGenerator;

namespace MaxValueWithTasks
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] testArr = Generator.GenerateNumbers();

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            int[][] partitions = Util.MakePartitions(testArr, 4);

            List<Task<int>> tasks = new List<Task<int>>();

            foreach (int[] partition in partitions)
            {
                tasks.Add(Task.Run(() => { return Util.MaxValue(partition); }));
            }

            Task.WaitAll(tasks.ToArray());

            var finalPartition = tasks.Select(t => t.Result);
            var finalMax = Util.MaxValue(finalPartition.ToArray());

            Console.WriteLine("max = " + finalMax);
            Console.WriteLine("Time elapsed: {0}", stopwatch.Elapsed);
            Console.ReadKey();
        }
    }
}
