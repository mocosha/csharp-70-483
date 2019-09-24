using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace MTable
{
    interface IIndex<T>
    {
        void AddProperty(T value, long position);
        void Save();
        void Recreate();
        List<long> GetPositions(string value);
    }

    public class Index<T> : IIndex<T>
    {
        public Index(Func<T, string> column)
        {
            Get = column;
            Recreate();
        }

        private Func<T, string> Get { get; set; }

        private Dictionary<string, List<long>> _indexes;

        public void AddProperty(T value, long position)
        {
            var property = Get(value);

            if (_indexes.ContainsKey(property))
            {
                _indexes[property].Add(position);
            }
            else
            {
                _indexes.Add(property, new List<long> { position });
            }
        }

        public List<long> GetPositions(string value)
        {
            if (_indexes.TryGetValue(value, out List<long> positions))
            {
                return positions;
            }

            return new List<long>();
        }

        public void Save()
        {
            _indexes.SaveToFile("index.bin");
        }

        public void Recreate()
        {
            _indexes = new Dictionary<string, List<long>>();
        }

        // TODO: read existing indexes
        private static Dictionary<string, List<long>> FromByteArrayToDict(byte[] data)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream(data))
            {
                object obj = bf.Deserialize(ms);
                return (Dictionary<string, List<long>>)obj;
            }
        }
    }
}
