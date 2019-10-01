using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace MTable
{
    public class MTable<T> : IMTable<T> where T : ISerializable
    {
        public MTable()
        {
            _indexes = new Dictionary<string, Index<T>>();
        }

        public IEnumerable<T> GetAll()
        {
            return
                GetAllNotDeletedRows();
        }

        public IEnumerable<T> Filter(Predicate<T> predicate)
        {
            return
                 GetAllNotDeletedRows()
                 .Where(i => predicate(i));
        }

        // TODO: like ...
        public IEnumerable<T> SearchByIndex(string name, string value)
        {
            var positions = _indexes[name].GetPositions(value);

            foreach (var item in positions)
            {
                var row = GetRowByPosition(item);
                yield return row.Payload;
            }
        }

        public int Add(T value)
        {
            var row = new MRow<T>(value);

            long position = default(long);
            using (Stream stream = new FileStream("data.bin", FileMode.Append))
            {
                using (BinaryWriter bw = new BinaryWriter(stream))
                {
                    position = stream.Position;
                    byte[] bytes = MRowToByteArray(row);
                    bw.Write(bytes.Length);
                    bw.Write(bytes);
                }
            }

            foreach (var item in _indexes)
            {
                var index = item.Value;
                index.AddProperty(value, position);
                index.Save();
            }

            return 1;
        }

        public IMTable<T> CreateIndex(string indexName, Func<T, string> column)
        {
            var ind = new Index<T>(indexName, column);

            _indexes.Add(indexName, ind);
            return this;
        }

        public int Delete(Predicate<T> predicate)
        {
            var allRows = GetAllRows();

            var forDeleted = allRows
                .Where(i => predicate(i.Payload));

            forDeleted.ToList()
                .ForEach(i =>
                {
                    i.Metadata.IsDeleted = true;
                    i.Metadata.DeletedAt = DateTimeOffset.UtcNow;
                });

            foreach (var item in _indexes)
            {
                item.Value.Recreate();
            }


            using (Stream stream = new FileStream("data.bin", FileMode.Create))
            {
                using (BinaryWriter bw = new BinaryWriter(stream))
                {
                    foreach (var row in allRows)
                    {
                        long position = stream.Position;
                        byte[] bytes = MRowToByteArray(row);
                        bw.Write(bytes.Length);
                        bw.Write(bytes);

                        foreach (var item in _indexes)
                        {
                            var index = item.Value;
                            index.AddProperty(row.Payload, position);
                        }
                    }

                    foreach (var item in _indexes)
                    {
                        item.Value.Save();
                    }
                }
            }

            return forDeleted.Count();
        }

        private IEnumerable<MRow<T>> GetAllRows()
        {
            using (BinaryReader reader = new BinaryReader(File.Open("data.bin", FileMode.Open, FileAccess.Read)))
            {
                int length = (int)reader.BaseStream.Length;
                var rows = new List<MRow<T>>();
                while (reader.BaseStream.Position != length)
                {
                    int bytesToRead = reader.ReadInt32();
                    byte[] v = reader.ReadBytes(bytesToRead);
                    MRow<T> value = FromByteArray(v);
                    rows.Add(value);
                }

                return rows;
            }
        }

        private MRow<T> GetRowByPosition(long position)
        {
            using (BinaryReader reader = new BinaryReader(File.Open("data.bin", FileMode.Open, FileAccess.Read)))
            {
                reader.BaseStream.Position = position;

                int bytesToRead = reader.ReadInt32();
                byte[] v = reader.ReadBytes(bytesToRead);
                MRow<T> value = FromByteArray(v);
                return value;
            }
        }

        private IEnumerable<T> GetAllNotDeletedRows()
        {
            return GetAllRows()
                .Where(r => r.Metadata.IsDeleted == false)
                .Select(i => i.Payload);
        }

        private static MRow<T> FromByteArray(byte[] data)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream(data))
            {
                object obj = bf.Deserialize(ms);
                return (MRow<T>)obj;
            }
        }

        private static byte[] MRowToByteArray(MRow<T> row)
        {
            var bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, row);
                return ms.ToArray();
            }
        }

        private Dictionary<string, Index<T>> _indexes;
    }
}
