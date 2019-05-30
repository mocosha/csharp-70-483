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
            return GetAllRow()
                .Where(r => r.Id == id && r.Metadata.IsDeleted == false)
                .Select(r => r.Payload)
                .FirstOrDefault();
        }

        private IEnumerable<MRow<T>> GetAllRow()
        {
            using (BinaryReader reader = new BinaryReader(File.Open("data.bin", FileMode.Open)))
            {
                int length = (int)reader.BaseStream.Length;
                while (reader.BaseStream.Position != length)
                {
                    int bytesToRead = reader.ReadInt32();
                    byte[] v = reader.ReadBytes(bytesToRead);
                    MRow<T> value = FromByteArray(v);
                    yield return value;
                }
            }
        }
        public IEnumerable<T> GetAll()
        {
            return GetAllRow()
                .Where(i=>i.Metadata.IsDeleted == false)
                .Select(r => r.Payload);
        }

        public IEnumerable<T> Find(Predicate<T> filter)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Add(T value)
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

            //var dict = new Dictionary<object, long>();

            //if (File.Exists("index.bin"))
            //{
            //    var bytes = File.ReadAllBytes("index.bin");
            //    dict = FromByteArrayToDict(bytes);
            //}

            //using (Stream stream = new FileStream("index.bin", FileMode.Append))
            //{
            //    using (BinaryWriter bw = new BinaryWriter(stream))
            //    {
            //        // TODO: how to get PK from T object
            //        var key = GetPropValue(value, "Id");
            //        Console.WriteLine(key);

            //        dict.Add(key, position);
            //        // byte[] bytes = ObjectToByteArray(dict);
            //        //bw.Write(bytes);
            //    }
            //}
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
