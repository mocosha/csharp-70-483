using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bojan
{
    public static class Parallel
    {
        public static int MaxLinq(this int[] items, int partitionCount)
        {
            var tasks = new List<Task<int>>();
            var r = items.Length / partitionCount;
            var start = 0;
            int[][] jaggedArray = new int[partitionCount][];
            for (int i = 0; i < partitionCount; i++)
            {
                jaggedArray[i] = items.SubArray(start, r);
                start = start + r;
            }

            var test =
                jaggedArray.AsParallel().Select(i => i.MyMaxx()).ToArray().MyMaxx();

            return test;
        }

        public static int MaxTasks(this int[] items, int partitionCount)
        {
            var tasks = new List<Task<int>>();
            var r = items.Length / partitionCount;
            int start = 0;
            for (int i = 0; i < partitionCount; i++)
            {
                var chunk = items.SubArray(start, r);

                tasks.Add(Task.Run(() =>
                {
                    return chunk.MyMaxx();
                }));

                start = start + r;
            }

            var maxEl = Task.WhenAll(tasks).ContinueWith(res => res.Result.MyMaxx());
            return maxEl.Result;
        }

        public static int MaxThreads(this int[] items, int partitionCount)
        {
            throw new NotImplementedException();
        }


    }
}
