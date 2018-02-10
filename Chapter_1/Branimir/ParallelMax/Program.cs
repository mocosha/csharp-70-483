using System;
using System.Diagnostics;
using System.Threading.Tasks;
using TestDataGenerator;

namespace ParallelMax
{
    class Program
    {
        public delegate int MaxDelegate(int[] data);

        private static void Benchmark(MaxDelegate myMethod, int[] data)
        {
            Stopwatch sw = Stopwatch.StartNew();
            var result = myMethod.DynamicInvoke(data);
            sw.Stop();
            Console.WriteLine();
            Console.WriteLine("\tTime taken: {0}ms, result: {1}", sw.Elapsed.TotalMilliseconds, result);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("--PARALLEL MAX BENCHMARK--");
            Console.WriteLine();

            var data = Generator.GenerateNumbers(9999999, refreshData: true);

            FrameworkMax(data);
            PlinqMax(data);
            PlinqPartitionedMax(data);
            PlinqMyPartitionerMax(data, 100);
            TaskMyPartitionerMax(data, 100);
            TaskAsyncMax(data, 100);
            ThreadMyPartitionerMax(data, 10);
            //ThreadBadBoyMax(data, 10);

            Console.ReadLine();
        }

        private static void FrameworkMax(int[] data)
        {
            //.NET FRAMEWORK
            //MSDN: https://msdn.microsoft.com/en-us/library/bb347632(v=vs.110).aspx
            //Implementation: https://referencesource.microsoft.com/#System.Core/System/Linq/Enumerable.cs,dd1aa43ba83e50eb
            Console.WriteLine("Framework Max()");
            int myMax(int[] d) => d.Max();
            Benchmark(myMax, data);
            Console.WriteLine();
        }

        private static void PlinqMax(int[] data)
        {
            //PLINQ          
            Console.WriteLine("PLINQ Max()");
            int myMax(int[] d) => d.MaxPlinq();
            Benchmark(myMax, data);
            Console.WriteLine();
        }

        private static void PlinqPartitionedMax(int[] data)
        {
            //PLINQ framework partitioner      
            Console.WriteLine("PLINQ framework partitioner Max()");
            int myMax(int[] d) => d.MaxPlinqPartitioned();
            Benchmark(myMax, data);
            Console.WriteLine();
        }

        private static void PlinqMyPartitionerMax(int[] data, int numberOfPartitions)
        {
            //PLINQ my partitioner     
            Console.WriteLine("PLINQ my partitioner Max()");
            int myMax(int[] d) => d.MaxPlinqMyPartitioner(numberOfPartitions);
            Console.WriteLine();
            Benchmark(myMax, data);
            Console.WriteLine();
        }

        private static void TaskMyPartitionerMax(int[] data, int numberOfPartitions)
        {
            //Tasks my partitioner     
            Console.WriteLine("Tasks Max()");
            int myMax(int[] d) => d.MaxTask(numberOfPartitions);
            Console.WriteLine();
            Benchmark(myMax, data);
            Console.WriteLine();
        }

        private async static Task<int> TaskAsyncMaxHelper(int[] data, int numberOfPartitions)
        {
            var intermediate = await data.MaxTaskIntermediateAsync(numberOfPartitions);  
            return await Task.FromResult<int>(intermediate.Max());
        }

        private static void TaskAsyncMax(int[] data, int numberOfPartitions)
        {
            Console.WriteLine("Tasks async Max()");
            Task<int> callTask = Task.Run(() => TaskAsyncMaxHelper(data, numberOfPartitions));
            Console.WriteLine("\tprogress bar");
            // Wait for it to finish
            callTask.Wait();
            // Get the result
            Console.WriteLine("\tResult: {0}", callTask.Result);
            Console.WriteLine();
        }

        private static void ThreadMyPartitionerMax(int[] data, int numberOfPartitions)
        {
            //Thread my partitioner     
            Console.WriteLine("Thread Max()");
            int myMax(int[] d) => d.MaxThread(numberOfPartitions);
            Console.WriteLine();
            Benchmark(myMax, data);
            Console.WriteLine();
        }

        private static void ThreadBadBoyMax(int[] data, int numberOfPartitions)
        {
            //Thread my partitioner     
            Console.WriteLine("Thread Max()");
            int myMax(int[] d) => d.MaxThreadBadBoy(numberOfPartitions);
            Console.WriteLine();
            Benchmark(myMax, data);
            Console.WriteLine();
        }
    }
}
