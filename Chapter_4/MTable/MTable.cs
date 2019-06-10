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
        public T Get(Guid id)
        {
            return GetAllNotDeletedRows()
                .Where(r => r.Id == id)
                .Select(r => r.Payload)
                .FirstOrDefault();
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

        private IEnumerable<MRow<T>> GetAllNotDeletedRows()
        {
            return GetAllRows().Where(r => r.Metadata.IsDeleted == false);
        }

        public IEnumerable<T> GetAll()
        {
            return
                GetAllNotDeletedRows()
                .Select(r => r.Payload);
        }

        public IEnumerable<T> Find(Predicate<T> predicat)
        {
            return
                 GetAllNotDeletedRows()
                .Where(i => predicat(i.Payload))
                .Select(r => r.Payload);
        }

        public int Delete(Predicate<T> predicat)
        {
            var allRows = GetAllRows();

            var forDeleted = allRows
                .Where(i => predicat(i.Payload));

            forDeleted.ToList()
                .ForEach(i =>
                {
                    i.Metadata.IsDeleted = true;
                    i.Metadata.DeletedAt = DateTimeOffset.UtcNow;
                });

            using (Stream stream = new FileStream("data.bin", FileMode.Create))
            {
                using (BinaryWriter bw = new BinaryWriter(stream))
                {
                    //position = stream.Position;
                    foreach (var row in allRows)
                    {
                        byte[] bytes = ObjectToByteArray(row);
                        bw.Write(bytes.Length);
                        bw.Write(bytes);
                    }
                }
            }

            return forDeleted.Count();
        }

        public void Delete(Guid id)
        {
            var rows = GetAllRows();
            var item = rows.FirstOrDefault(r => r.Id == id);

            if (item == null)
                throw new Exception("Item not found");

            item.Metadata.IsDeleted = true;
            item.Metadata.DeletedAt = DateTimeOffset.UtcNow;

            // TODO: improve this
            using (Stream stream = new FileStream("data.bin", FileMode.Create))
            {
                using (BinaryWriter bw = new BinaryWriter(stream))
                {
                    //position = stream.Position;
                    foreach (var row in rows)
                    {
                        byte[] bytes = ObjectToByteArray(row);
                        bw.Write(bytes.Length);
                        bw.Write(bytes);
                    }
                }
            }
        }

        public int Add(T value)
        {
            var row = new MRow<T>(value);

            //long position = default(long);
            using (Stream stream = new FileStream("data.bin", FileMode.Append))
            {
                using (BinaryWriter bw = new BinaryWriter(stream))
                {
                    //position = stream.Position;
                    byte[] bytes = ObjectToByteArray(row);
                    bw.Write(bytes.Length);
                    bw.Write(bytes);
                }
            }

            return 1;
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

        private static Dictionary<object, long> FromByteArrayToDict(byte[] data)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream(data))
            {
                object obj = bf.Deserialize(ms);
                return (Dictionary<object, long>)obj;
            }
        }

        private static byte[] ObjectsToByteArray(MRow<T> rows)
        {
            var bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, rows);
                return ms.ToArray();
            }
        }
        private static byte[] ObjectToByteArray(MRow<T> row)
        {
            var bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, row);
                return ms.ToArray();
            }
        }

        private static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }


    }
}
