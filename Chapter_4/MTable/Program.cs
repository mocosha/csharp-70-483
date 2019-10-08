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
            var mTable = new MTable<Book>();

            mTable
               .CreateIndex("IndexAuthor", x => x.Author)
               .CreateIndex("IndexTitle", m => m.Title);

            if (mTable.Exists())
            {
                mTable.RecreateIndexes();
            }
            else
            {
                LoadDemoData(mTable);
            }

            Print(mTable.GetAll());

            Search(mTable);
        }

        private static void LoadDemoData(MTable<Book> mTable)
        {
            mTable.Add(new Book
            {
                Author = "Megan McCafferty",
                Title = "Epic Reads Book Club Sampler"
            });

            mTable.Add(new Book
            {
                Author = "Lauren Oliver",
                Title = "The End Is Here: Teen Dystopian Sampler"
            });

            mTable.Add(new Book
            {
                Author = "Charles Dickens",
                Title = "Oliver Twist"
            });

            mTable.Add(new Book
            {
                Author = "Mary Burton",
                Title = "Cut and Run"
            });

            mTable.Add(new Book
            {
                Author = "Stephen King",
                Title = "A Brief History of Time"
            });

            mTable.Add(new Book
            {
                Author = "Stephen King",
                Title = "The Shining"
            });
        }

        static void Search(MTable<Book> mTable)
        {
            Console.WriteLine(new string('-', 50));
            
            var authorKey = "Stephen";
            var books = mTable.SearchByIndex("IndexAuthor", authorKey);
            Console.WriteLine($"Search by author '{authorKey}' results:");
            foreach (var b in books)
            {
                Console.WriteLine($"\t{b}");
            }

            var term = "The";
            books = mTable.SearchByIndex("IndexTitle", term);
            Console.WriteLine($"Search by term '{term}' results:");
            foreach (var b in books)
            {
                Console.WriteLine($"\t{b}");
            }

            term = "Oliver";
            books = mTable.Search(term);
            Console.WriteLine($"Search by term '{term}' results:");
            foreach (var b in books)
            {
                Console.WriteLine($"\t{b}");
            }
        }

        private static void Print(IEnumerable<Book> books)
        {
            Console.WriteLine("Show all database items (Author - Title):");

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
        }
    }
}
