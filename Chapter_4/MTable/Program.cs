using System;
using System.IO;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace MTable
{
    [Serializable]
    class Book : ISerializable
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Summary { get; set; }

        public Book()
        {
        }

        public override string ToString()
        {
            return $"{Id}: {Author} - {Title}";
        }

        protected Book(SerializationInfo info, StreamingContext context)
        {
            Id = (Guid)info.GetValue("Value1", typeof(Guid));
            Title = info.GetString("Value2");
            Author = info.GetString("Value3");
            Summary = info.GetString("Value4");
        }

        //[NonSerialized]
        //private bool _isDirty = false;

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Value1", Id);
            info.AddValue("Value2", Title);
            info.AddValue("Value3", Author);
            info.AddValue("Value4", Summary);
        }
    }

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
                    Id = Guid.NewGuid(),
                    Author = "Book1",
                    Title = "TitleBook1",
                    Summary = "SummaryBook1"
                };

                mTable.Add(book1);

                var book2 = new Book
                {
                    Id = Guid.NewGuid(),
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
