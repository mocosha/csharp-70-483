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
            //StackUsageSample();
            
            DoubleLinkedListUsageSample();

            Console.ReadLine();
        }

        private static void DoubleLinkedListUsageSample()
        {
            var address = new DoubleLinkedList();
            address.Add("Nehruova 56");
            address.Add("Djordja Stanojevica 11g");
            var r = address.Values();
            Console.WriteLine(r);

            //var item = address.Find("nehruova 56");
            //Console.WriteLine("Found: {0}", item);
            //item = address.Find("2nehruova 56");
            //Console.WriteLine("Found: {0}", item);

            address.Add("Test Adres");
            var values =  address.Values();
            Console.WriteLine(values);
            address.Delete("Djordja Stanojevica 11g");
            values = address.Values();
            Console.WriteLine(values);
            Console.ReadKey();
        }
    }
}
