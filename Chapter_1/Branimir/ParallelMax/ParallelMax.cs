using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

using System.Threading;
using System.Threading.Tasks;

namespace ParallelMax
{
    public static partial class Enumerable
    {
        public static void WriteLine(this int[] source)
        {
            //Console.WriteLine(string.Join(", ", source));
        }

        public static int PartitionMax(this int[] source, int startIndex, int endIndex)
        {
            //Console.Write("{0} ", Thread.CurrentThread.ManagedThreadId);
            var m = source[startIndex];

            for (int i = startIndex + 1; i < endIndex; i++)
            {
                m = source[i] > m ? source[i] : m;
            }

            return m;
        }

        public static IEnumerable<ValueTuple<int,int>> CreatePartitions(int length, int numberOfPartitions)
        {
            var lengthOfPartition = length / numberOfPartitions + (length % numberOfPartitions > 0 ? 1 : 0);

            for (int i = 0; i < numberOfPartitions - 1; i++)
            {
                yield return (i * lengthOfPartition, (i + 1) * lengthOfPartition);
            }

            yield return ((numberOfPartitions-1) * lengthOfPartition, length);
        }

        //public static IEnumerable

        //https://referencesource.microsoft.com/#System.Core/System/Linq/Enumerable.cs,dd1aa43ba83e50eb
        public static int Max(this int[] source)
        {
            if (source == null) throw new ArgumentNullException("source");
            int value = 0;
            bool hasValue = false;
            foreach (int x in source)
            {
                if (hasValue)
                {
                    if (x > value) value = x;
                }
                else
                {
                    value = x;
                    hasValue = true;
                }
            }
            if (hasValue) return value;
            throw new Exception("No Elements");
        }

        public static int MaxPlinq(this int[] source)
        {
            return source.AsParallel().Max();
        }

        public static int MaxPlinqPartitioned(this int[] source)
        {
            // Create a load-balancing partitioner. Or specify false for static partitioning.
            Partitioner<int> customPartitioner = Partitioner.Create(source, true);
            return customPartitioner.AsParallel().Max();
        }

        public static int MaxPlinqMyPartitioner(this int[] source, int numberOfPartitions)
        {
            var partitions = CreatePartitions(source.Length, numberOfPartitions);
            //Console.Write("\tThreads: ");
            var intermediate = partitions.AsParallel().Select(p => source.PartitionMax(p.Item1, p.Item2)).ToArray();
            return intermediate.PartitionMax(0, intermediate.Length);
        }

        public static int MaxTask(this int[] source, int numberOfPartitions)
        {
            var partitions = CreatePartitions(source.Length, numberOfPartitions);
            
            List<Task<int>> TaskList = new List<Task<int>>();

            //Console.Write("\tThreads: ");
            foreach (var p in partitions)
            {
                int a() => source.PartitionMax(p.Item1, p.Item2);
                var LastTask = new Task<int>(a);
                
                LastTask.Start();
                TaskList.Add(LastTask);
            }

            Task.WaitAll(TaskList.ToArray());
            return TaskList.Select(t => t.Result).Max();
        }


        public static int MaxThreadBadBoy(this int[] source, int numberOfPartitions)
        {
            var partitions = CreatePartitions(source.Length, numberOfPartitions).ToArray();
            var threads = new Thread[numberOfPartitions];
            var intermediate = new int[numberOfPartitions];

            //Console.Write("\tThreads: ");
            for (int j = 0; j < numberOfPartitions; j++)
            {
                var p = partitions[j];

                threads[j] = new Thread(() => intermediate[j] = source.PartitionMax(p.Item1, p.Item2));
            }

            foreach (var thread in threads) thread.Join();

            return intermediate.PartitionMax(0, numberOfPartitions);
        }

        public static int MaxThread(this int[] source, int numberOfPartitions)
        {
            var partitions = CreatePartitions(source.Length, numberOfPartitions).ToArray();
            var threads = new Thread[numberOfPartitions];
            var intermediate = new int[numberOfPartitions];

            //Console.Write("\tThreads: ");
            for (int j = 0; j < numberOfPartitions; j++)
            {
                var p = partitions[j];

                threads[j] = new Thread(new ParameterizedThreadStart(
                    i => intermediate[(int)i] = source.PartitionMax(p.Item1, p.Item2)
                ));
            }

            for (int i = 0; i < numberOfPartitions; i++) threads[i].Start(i);
            foreach (var thread in threads) thread.Join();
            return intermediate.PartitionMax(0, numberOfPartitions);
        }

        public static async Task<int[]> MaxTaskIntermediateAsync(this int[] source, int numberOfPartitions)
        {
            var partitions = CreatePartitions(source.Length, numberOfPartitions);

            List<Task<int>> TaskList = new List<Task<int>>();

            foreach (var p in partitions)
            {
                int a() => source.PartitionMax(p.Item1, p.Item2);
                var LastTask = new Task<int>(a);

                LastTask.Start();
                TaskList.Add(LastTask);
            }

            var intermediate = await Task.WhenAll(TaskList.ToArray());
            
            return intermediate;
        }
    }
}
