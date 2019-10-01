using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace MTable
{
    public class Index<T> : IIndex<T>
    {
        public string Name { get; set; }

        public Index(string name, Func<T, string> column)
        {
            Name = name;
            Get = column;

            // TODO: read indexes if exists
            // Init()
            Recreate();
        }

        private Func<T, string> Get { get; set; }

        private Dictionary<string, List<long>> _indexes;

        public void AddProperty(T value, long position)
        {
            var property = Get(value);

            AddTerm(property, position);

            foreach (var term in property.Split(" .,`".ToCharArray()))
            {
                AddTerm(term, position);
            }
        }

        private void AddTerm(string property, long position)
        {
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
            _indexes.SaveToFile($"{Name}.bin");
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
