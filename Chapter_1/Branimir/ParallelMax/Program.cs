using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestDataGenerator;

namespace Branimir
{
    class Program
    {
        public delegate int MaxDelegate(IEnumerable<int> data);

        private static int[] data = Enumerable.Range(0,99999).ToArray();

        private static void Benchmark(MaxDelegate myMethod, IEnumerable<int> data)
        {
            Stopwatch sw = Stopwatch.StartNew();
            var result = myMethod.DynamicInvoke(data);
            sw.Stop();
            Console.WriteLine("Time taken: {0}ms, result: {1}", sw.Elapsed.TotalMilliseconds, result);
        }

        private static int FrameworkMax(IEnumerable<int> data)
        {
            return data.Max();
        }

        static void Main(string[] args)
        {
            Console.WriteLine("--PARALLEL MAX BENCHMARK--");

            var data = Generator.GenerateNumbers();

            //.NET FRAMEWORK
            //MSDN: https://msdn.microsoft.com/en-us/library/bb347632(v=vs.110).aspx
            //Implementation: https://referencesource.microsoft.com/#System.Core/System/Linq/Enumerable.cs,dd1aa43ba83e50eb
            Console.WriteLine("Framework Max()");
            MaxDelegate myMethod = FrameworkMax;
            Benchmark(myMethod, data);

            Console.ReadLine();
        }
    }
}
