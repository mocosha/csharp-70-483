using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MTable
{
    class Program
    {
        static void Main(string[] args)
        {
            MTable<Book> mTable = CreateTable();

            if (File.Exists("data.bin"))
            {
                var books = mTable.GetAll();
                Print(books);

                books = mTable.Filter(b => b.Title.StartsWith("bojan", StringComparison.OrdinalIgnoreCase));
                Print(books);
            }
            else
            {
                var book = new Book
                {
                    Author = "Megan McCafferty",
                    Title = "Epic Reads Book Club Sampler"
                };

                mTable.Add(book);
                Console.WriteLine($"{book} added.");

                book = new Book
                {
                    Author = "Lauren Oliver",
                    Title = "The End Is Here: Teen Dystopian Sampler"
                };

                mTable.Add(book);
                Console.WriteLine($"{book} added.");

                book = new Book
                {
                    Author = "Mary Burton",
                    Title = "Cut and Run"
                };

                mTable.Add(book);
                Console.WriteLine($"{book} added.");

                var authorForDelete = "Lauren Oliver";
                mTable.Delete(b => b.Author == authorForDelete);
                Console.WriteLine($"Book with author '{authorForDelete}' deleted.");

                book = new Book
                {
                    Author = "Stephen King",
                    Title = "A Brief History of Time"
                };

                mTable.Add(book);
                Console.WriteLine($"{book} added.");

                book = new Book
                {
                    Author = "Stephen King",
                    Title = "The Shining"
                };

                mTable.Add(book);
                Console.WriteLine($"{book} added.");

                Console.WriteLine(new string('-', 50));

                var authorKey = "Stephen King";
                var books = mTable.SearchByIndex("IndexAuthor", authorKey);
                Console.WriteLine("Serach by author results:");
                foreach (var b in books)
                {
                    Console.WriteLine($"{b}");
                }

                var titleKey = "The";
                books = mTable.SearchByIndex("IndexTitle", titleKey);
                Console.WriteLine($"Serach by term '{titleKey}' title results:");
                foreach (var b in books)
                {
                    Console.WriteLine($"{b}");
                }
            }
        }

        private static void Print(IEnumerable<Book> books)
        {
            Console.WriteLine("PRINT");

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

            Console.WriteLine(new string('-', 50));
        }

        private static MTable<Book> CreateTable()
        {
            var mTable = new MTable<Book>();

            mTable
                .CreateIndex("IndexAuthor", x => x.Author)
                .CreateIndex("IndexTitle", m => m.Title);

            return mTable;
        }
    }
}
