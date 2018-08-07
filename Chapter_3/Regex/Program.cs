using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Chapter_3
{
    class Program
    {
        public static void Match(string input, string pattern)
        {
            Console.WriteLine("Match example");
            Console.WriteLine($"Input: {input}");
            Console.WriteLine($"Pattern: {pattern}");

            var regex = new Regex(pattern);
            Match match = regex.Match(input);
            
            if (match.Success)
            {
                Console.WriteLine($"Matched value: {match.Value}");
                Console.WriteLine($"Groups for {match.Value}:");
                foreach (Group item in match.Groups)
                {
                    Console.WriteLine(item.Value);
                }
            }
            else
            {
                Console.WriteLine("No match value");
            }
        }

        public static void Matches(string input, string pattern)
        {
            Console.WriteLine("MATCHES example");
            Console.WriteLine($"Input: {input}");
            Console.WriteLine($"Pattern: {pattern}");

            var regex = new Regex(pattern);
            var matches = regex.Matches(input);

            if (matches.Count ==0)
            {
                Console.WriteLine("No match values");
                return;
            }

            foreach (Match match in matches)
            {
                Console.WriteLine($"Matched value: {match.Value}");
                Console.WriteLine($"Groups for '{match.Value}':");
                foreach (Group item in match.Groups)
                {
                    Console.WriteLine(item.Value);
                }
            }
        }

        public static void Replace(string input, string pattern, string replacement)
        {
            Console.WriteLine("REPLACE example");
            Console.WriteLine($"Input: {input}");
            Console.WriteLine($"Pattern: {pattern}");
            Console.WriteLine($"Replacement: {replacement}");

            var rgx = new Regex(pattern);
            string result = rgx.Replace(input, replacement);
            Console.WriteLine($"Result: {result}");
        }

        static void Main(string[] args)
        {
            string input = args[0];
            string pattern = args[1];

            string replacement = null;
            if (args.Length == 3)
            {
                replacement = args[2];
            }
            
            Matches(input, pattern);
            
            var shouldReplace = !string.IsNullOrWhiteSpace(replacement);
            if (shouldReplace)
            {
                Console.WriteLine(new string('-', 20));
                Replace(input, pattern, replacement);
            }

            Console.ReadKey(true);
        }
    }
}
