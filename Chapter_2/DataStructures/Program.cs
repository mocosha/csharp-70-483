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
        static void Main(string[] args)
        {
            //StackUsageSample();

            DoubleLinkedListUsageSample();

            //SetUsageSample();

            Console.ReadLine();
        }

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

            var set2 = new Set<string>() { "Bojan", "Mlađan" };

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

            WriteSet(set, "Remove Branimir");

            Console.WriteLine();

            Console.WriteLine($"Overlaps: {set.Overlaps(set2)}");
            Console.WriteLine($"Overlaps: {set.Overlaps(new Set<string> { "1", "2" })}");
            //Console.WriteLine($"Overlaps: {set.Overlaps(new Set<int> { 1, 2 })}");
        }

        private static void ShowItems<T>(IEnumerable<T> list)
        {
            foreach (var item in list)
            {
                Console.WriteLine(item);
            }
        }

        private static void DoubleLinkedListUsageSample()
        {
            var addresses = new DoubleLinkedList<string>();
            ShowItems(addresses);
            Console.WriteLine(new string('-', 10));
            addresses.AddToEnd("Nehruova 56");
            ShowItems(addresses);
            Console.WriteLine(new string('-', 10));
            addresses.Delete("Nehruova 56");
            ShowItems(addresses);
            Console.WriteLine(new string('-', 10));

            addresses.AddToEnd("Djordja Stanojevica 11g");
            addresses.AddToEnd("Jurija Gagarina 13");
            addresses.AddToEnd("Jurija Gagarina 144");
            ShowItems(addresses);
            Console.WriteLine(new string('-', 10));

            addresses.Delete("Jurija Gagarina 13");
            ShowItems(addresses);
            Console.WriteLine(new string('-', 10));

            var searchAddress = "nehruova 56";
            var item = addresses.Find(searchAddress);
            Console.WriteLine("Search text {0}; Found: {1}", searchAddress, item);
            searchAddress = "Djordja Stanojevica 11g";
            item = addresses.Find(searchAddress);
            Console.WriteLine("Search text {0}; Found: {1}", searchAddress, item);
            Console.WriteLine(new string('-', 10));
            addresses.AddToEnd("Test adresa 1");
            addresses.AddToEnd("Test adresa 2");
            ShowItems(addresses);
            Console.WriteLine(new string('-', 10));
            addresses.Delete("Djordja Stanojevica 11g");
            ShowItems(addresses);
        }
    }
}
