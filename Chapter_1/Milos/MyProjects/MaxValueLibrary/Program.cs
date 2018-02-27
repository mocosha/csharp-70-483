using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestDataGenerator;

namespace MaxValueLibrary
{

    public class InvalidPartitionCount : ArgumentOutOfRangeException
    {
        public InvalidPartitionCount(string paramName) : base(paramName)
        { }
    }

    class Program
    {
        static int[] SubArray(int[] data, int index, int length)
        {
            int[] result = new int[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }

        static int MaxValue(int[] arr)
        {
            int max = 0;

            foreach (int value in arr)
            {
                if (max < value)
                {
                    max = value;
                }
            }

            return max;
        }

        static int[][] MakePartitions(int[] arr, int partitionCount)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            List<int[]> result = new List<int[]>();

            if (partitionCount > arr.Length)
            {
                throw new InvalidPartitionCount("Partition count must not be greather then the length of the given array.");
            }

            if (partitionCount < 1)
            {
                throw new InvalidPartitionCount("Partition count must be greather than zero.");
            }

            int partitionLength = arr.Length / partitionCount;
            int rest = arr.Length % partitionCount;

            int length = partitionLength;
            for (int i = 0; i < partitionCount; i++)
            {
                // if its the last partition take everything that is left
                if((i + 1) == partitionCount && rest > 0)
                {
                    length = partitionLength + rest;
                }

                result.Add(SubArray(arr, (partitionLength * i), length));
            }

            Console.WriteLine("Partitioning: {0}", stopwatch.Elapsed);

            return result.ToArray();
        }

        static int FindMaxValue(int[] arr, int partitionCount) 
        {
            List<int> maxValues = new List<int>(0);
            
            foreach(int[] partition in MakePartitions(arr, partitionCount))
            {
                maxValues.Add(MaxValue(partition));
            }

            return MaxValue(maxValues.ToArray());
        }

        static void FindMaxValueWithThreads(int[] arr, int partitionCount)
        {

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            ConcurrentBag<int> maxValues = new ConcurrentBag<int>();
            int[][] partitions = MakePartitions(arr, partitionCount);

            foreach (int[] partition in partitions)
            {
                Thread t = new Thread(new ThreadStart(() => {
                    
                    int max = MaxValue(partition);
                    maxValues.Add(max);

                }));

                t.Start();
                t.Join();

               
            }

            int finalMax = MaxValue(maxValues.ToArray());
          //  Console.WriteLine("data = " + string.Join(" ", maxValues.ToArray()));
            Console.WriteLine("max = " + finalMax);
            Console.WriteLine("Time elapsed: {0}", stopwatch.Elapsed);


        }


        static void Main(string[] args)
        {

            //int[] testArr = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };

          //  Stopwatch stopwatch = new Stopwatch();
           // stopwatch.Start();

           // Generator.ClearCache();
            int[] testArr = Generator.GenerateNumbers();


            //int max = FindMaxValue(testArr, 1);


            FindMaxValueWithThreads(testArr, 4);

           // stopwatch.Stop();

            // Write result.


            //Console.WriteLine("data = " + string.Join(" ", testArr));
            // Console.WriteLine("max = " + max);
            // Console.WriteLine("Time elapsed: {0}", stopwatch.Elapsed);

            Console.ReadKey();

        }
    }
}
