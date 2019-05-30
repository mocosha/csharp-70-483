using System;
using System.IO;

namespace MTable
{
    class Program
    {
        static void Main(string[] args)
        {
            var mTable = new MTable<Book>();

            if (File.Exists("data.bin"))
            {
                var books = mTable.GetAll();
                foreach (var book in books)
                {
                    Console.WriteLine(book);
                }
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

                var book2 = new Book
                {
                    Author = "Book2",
                    Title = "TitleBook2",
                    Summary = "SummaryBook2"
                };

                mTable.Add(book2);
            }

            Console.ReadKey(true);
        }
    }
}
