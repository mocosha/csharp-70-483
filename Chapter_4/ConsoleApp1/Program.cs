using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Permissions;

namespace ConsoleApp1
{
    [Serializable]
    class Book : ISerializable
    {
        //public Guid Id { get; private set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Summary { get; set; }

        public Book()
        {
            //Id = Guid.NewGuid();
        }

        public override string ToString()
        {
            return $"{Author} - {Title}";
        }

        protected Book(SerializationInfo info, StreamingContext context)
        {
            //Id = (Guid)info.GetValue("Value1", typeof(Guid));
            Title = info.GetString("Value2");
            Author = info.GetString("Value3");
            Summary = info.GetString("Value4");
        }

        [NonSerialized]
        private bool _isDirty = false;

        [OnSerializing()]
        internal void OnSerializingMethod(StreamingContext context)
        {
            Console.WriteLine("OnSerializing.");
        }

        [OnSerialized()]
        internal void OnSerializedMethod(StreamingContext context)
        {
            Console.WriteLine("OnSerialized.");
        }

        [OnDeserializing()]
        internal void OnDeserializingMethod(StreamingContext context)
        {
            Console.WriteLine("OnDeserializing.");
        }

        [OnDeserialized()]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            Console.WriteLine("OnSerialized.");
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            //info.AddValue("Value1", Id);
            info.AddValue("Value2", Title);
            info.AddValue("Value3", Author);
            info.AddValue("Value4", Summary);
        }
    }

    class MTable<T> where T : ISerializable
    {
        public T Get(Guid id)
        {
            throw new NotImplementedException();
        }

        private static T FromByteArray(byte[] data)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream(data))
            {
                object obj = bf.Deserialize(ms);
                return (T)obj;
            }
        }

        public IEnumerable<T> GetAll()
        {
            using (BinaryReader reader = new BinaryReader(File.Open("data.bin", FileMode.Open)))
            {
                int length = (int)reader.BaseStream.Length;
                while (reader.BaseStream.Position != length)
                {
                    int bytesToRead = reader.ReadInt32();
                    byte[] v = reader.ReadBytes(bytesToRead);
                    T value = FromByteArray(v);
                    yield return value;
                }
            }
        }

        private static byte[] ObjectToByteArray(T value)
        {
            var bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, value);
                return ms.ToArray();
            }
        }

        public void Add(T value)
        {
            using (Stream stream = new FileStream("data.bin", FileMode.Append))
            {
                using (BinaryWriter bw = new BinaryWriter(stream))
                {
                    // var position = stream.Position; //need for index
                    byte[] bytes = ObjectToByteArray(value);
                    bw.Write(bytes.Length);
                    bw.Write(bytes);
                }
            }

            // TODO: how to get PK from T object
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
