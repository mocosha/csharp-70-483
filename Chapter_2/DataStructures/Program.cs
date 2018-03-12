using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stack
{   

    class Program
    {
        static void StackUsageSample()
        {
            var stack = new Stack<int>();

            Console.WriteLine("Empty stack count: {0}", stack.Length);

            stack.Push(1);
            Console.WriteLine("1 pushed to the stack, count: {0}", stack.Length);
            stack.Push(54);
            stack.Push(13);

            Console.WriteLine("Count: {0}", stack.Length);

            Console.Write("Pop all elements:");

            while (stack.Length > 0)
                Console.Write("{0} ", stack.Pop());

            Console.WriteLine();
        }

        static void Main(string[] args)
        {
            StackUsageSample();

            Console.ReadLine();
        }
    }
}
