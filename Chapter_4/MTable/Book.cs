using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace MTable
{
    [Serializable]
    class Book : ISerializable
    {
        public string Title { get; set; }
        public string Author { get; set; }

        public Book()
        {
        }

        public override string ToString()
        {
            return $"{Author} - {Title}";
        }

        protected Book(SerializationInfo info, StreamingContext context)
        {
            Title = info.GetString("Value1");
            Author = info.GetString("Value2");
        }

        //[NonSerialized]
        //private bool _isDirty = false;

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Value1", Title);
            info.AddValue("Value2", Author);
        }
    }
}
