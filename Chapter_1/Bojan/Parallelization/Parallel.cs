using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Bojan
{
    public static class Parallel
    {
        public static int MaxLinq(this int[] items, int partitionCount)
        {
            var tasks = new List<Task<int>>();
            var chunkSize = items.Length / partitionCount;
            var addition = items.Length % partitionCount;
            var firstChunkSize = chunkSize + addition;

            var start = 0;
            int[][] partition = new int[partitionCount][];
            for (int i = 0; i < partitionCount; i++)
            {
                partition[i] = items.SubArray(start, firstChunkSize);
                start = start + chunkSize;
            }

            var test =
                partition.AsParallel().Select(i => i.MyMaxx()).ToArray().MyMaxx();

            return test;
        }

        public static int MaxTasks(this int[] items, int partitionCount)
        {
            var tasks = new List<Task<int>>();
            var chunkSize = items.Length / partitionCount;
            var addition = items.Length % partitionCount;
            var firstChunkSize = chunkSize + addition;
            int start = 0;
            for (int i = 0; i < partitionCount; i++)
            {
                var chunk = items.SubArray(start, firstChunkSize);

                tasks.Add(Task.Run(() =>
                {
                    return chunk.MyMaxx();
                }));

                start = start + chunkSize;
            }

            var maxEl = Task.WhenAll(tasks).ContinueWith(res => res.Result.MyMaxx());
            return maxEl.Result;
        }

        public static int MaxThreads(this int[] items, int partitionCount)
        {
            var results = new List<int>();
            var threads = new Thread[partitionCount];

            var chunkSize = items.Length / partitionCount;
            var addition = items.Length % partitionCount;
            var firstChunkSize = chunkSize + addition;

            int start = 0;
            for (int i = 0; i < partitionCount; i++)
            {
                var chunk = items.SubArray(start, firstChunkSize);

                threads[i] = new Thread(() => results.Add(chunk.MyMaxx()));
                threads[i].Start();
                start = start + chunkSize;
            }

            foreach (var item in threads)
            {
                item.Join();
            }

            return results.ToArray().MyMaxx();
        }


    }
}
