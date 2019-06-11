using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MTable
{
    class Program
    {
        private static void PrintAll(IEnumerable<Book> books)
        {
            if (books.Any())
            {
                foreach (var book in books)
                {
                    Console.WriteLine(book);
                }
            }
            else
            {
                Console.WriteLine("NO RESULT");
            }

            Console.WriteLine(new string('-', 20));
        }

        static void Main(string[] args)
        {
            var mTable = new MTable<Book>();

            if (File.Exists("data.bin"))
            {
                var books = mTable.GetAll();
                PrintAll(books);

                books = mTable.Find(b => b.Title.StartsWith("bojan", StringComparison.OrdinalIgnoreCase));
                PrintAll(books);
            }
            else
            {
                var book1 = new Book
                {
                    Author = "Book1",
                    Title = "TitleBook1",
                    Summary = "SummaryBook1"
                };

                mTable.Add(book1);
                Console.WriteLine($"{book1.Author} added");

                var book2 = new Book
                {
                    Author = "Book2",
                    Title = "TitleBook2",
                    Summary = "SummaryBook2"
                };

                mTable.Add(book2);
                Console.WriteLine($"{book2.Author} added");

                var books = mTable.GetAll();
                Console.WriteLine("PRINT ALL");
                PrintAll(books);

                var book3 = new Book
                {
                    Author = "Book3",
                    Title = "Bojan",
                    Summary = "SummaryBook3"
                };

                mTable.Add(book3);
                Console.WriteLine($"{book3.Author} added");

                books = mTable.GetAll();
                Console.WriteLine("PRINT ALL");
                PrintAll(books);

                mTable.Delete(b => b.Author == "Book2");
                Console.WriteLine($"Book with author 'Book2' deleted");

                books = mTable.GetAll();
                Console.WriteLine("PRINT ALL");
                PrintAll(books);

                //books = mTable.Find(b => b.Title.StartsWith("bojan", StringComparison.OrdinalIgnoreCase));
                //PrintAll(books);
            }

            Console.ReadKey(true);
        }
    }
}
