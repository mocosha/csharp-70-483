using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace MTable
{
    public static class Extensions
    {
        public static Dictionary<string, List<long>> SaveToFile(this Dictionary<string, List<long>> indexes, string fileName)
        {
            File.WriteAllBytes(fileName, ObjectToByteArray(indexes));
            return indexes;
        }

        private static byte[] ObjectToByteArray(object value)
        {
            var bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, value);
                return ms.ToArray();
            }
        }
    }
}
