using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Branimir
{
    class Program
    {
        public delegate int MaxDelegate(int[] data);

        private static int[] data = Enumerable.Range(0,99999).ToArray();

        private static void Benchmark(MaxDelegate myMethod, IEnumerable<int> data)
        {
            Stopwatch sw = Stopwatch.StartNew();
            var result = myMethod.DynamicInvoke(data);
            sw.Stop();
            Console.WriteLine("Time taken: {0}ms, result: {1}", sw.Elapsed.TotalMilliseconds, result);
        }

        private static int FrameworkMax(int[] data)
        {
            return data.Max();
        }

        static void Main(string[] args)
        {
            MaxDelegate myMethod = FrameworkMax;
            Benchmark(myMethod, data);

            Console.ReadLine();
        }
    }
}
