using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructures
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
            
            //DoubleLinkedListUsageSample();

            SetUsageSample();

            Console.ReadLine();
        }

        private static void SetUsageSample()
        {
            var theTeam = new[] { "Miloš", "Bojan", "Miloš", "Branimir" };

            //https://stackoverflow.com/questions/32027271/generate-two-different-strings-with-the-same-hashcode            
            var theSameHashCodeItems = new[] { "xqzrbn", "krumld" };

            Console.WriteLine("-- HashSet --");
            var hashSet = new HashSet<string>();

            void hashSetAddAndWriteCount(string item)
            {
                hashSet.Add(item);
                Console.WriteLine($"Element '{item}' added, count: {hashSet.Count}");
            }

            foreach (var item in theTeam)
                hashSetAddAndWriteCount(item);

            foreach (var item in theSameHashCodeItems)
                hashSetAddAndWriteCount(item);

            Console.WriteLine();
            Console.WriteLine("-- Set --");

            var set = new Set<string>();

            void setAddAndWriteCount(string item)
            {
                set.Add(item);
                Console.WriteLine($"Element '{item}' added, count: {set.Count}");
            }

            foreach (var item in theTeam)
                setAddAndWriteCount(item);

            foreach (var item in theSameHashCodeItems)
                setAddAndWriteCount(item);

            var set2 = new Set<string>();
            set2.Add("Bojan");
            set2.Add("Mlađan");

            
            set.UnionWith(set2);

            void WriteSet(Set<string> mySet, string message)
            {
                Console.WriteLine();
                Console.WriteLine($"-- {message} --");
                Console.WriteLine($"Count: {mySet.Count()}");
                Console.Write("Elements: ");
                foreach (var item in mySet)
                    Console.Write($"{item} ");
                Console.WriteLine();
            }

            WriteSet(set, "Union");

            set.Remove("Branimir");

            WriteSet(set, "Remove Mladjan");

            Console.WriteLine();

            Console.WriteLine($"Overlaps: {set.Overlaps(set2)}");
            Console.WriteLine($"Overlaps: {set.Overlaps(new Set<string> {"1", "2" })}");
            //Console.WriteLine($"Overlaps: {set.Overlaps(new Set<int> { 1, 2 })}");
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
