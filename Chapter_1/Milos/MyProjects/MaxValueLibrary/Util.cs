using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxValueLibrary
{
    public static class Util
    {
        public static int[] SubArray(int[] data, int index, int length)
        {
            int[] result = new int[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }

        public static int MaxValue(int[] arr)
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

        public static int[][] MakePartitions(int[] arr, int partitionCount)
        {
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
                if ((i + 1) == partitionCount && rest > 0)
                {
                    length = partitionLength + rest;
                }

                result.Add(SubArray(arr, (partitionLength * i), length));
            }

            return result.ToArray();
        }

        public static int FindMaxValue(int[] arr, int partitionCount)
        {
            List<int> maxValues = new List<int>(0);

            foreach (int[] partition in MakePartitions(arr, partitionCount))
            {
                maxValues.Add(MaxValue(partition));
            }

            return MaxValue(maxValues.ToArray());
        }
    }
}
