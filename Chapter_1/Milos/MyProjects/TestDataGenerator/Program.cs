using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDataGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var data = Generator.GenerateNumbers();

            Console.WriteLine(string.Join(" ", data));
            Console.ReadKey();
        }
    }
}
