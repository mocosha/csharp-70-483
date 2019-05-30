using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace MTable
{
    [Serializable]
    public class Metadata
    {
        public string Application { get; set; }
        public bool IsDeleted { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }

    [Serializable]
    public class MRow<T> where T : ISerializable
    {
        public MRow(T payload)
        {
            Id = Guid.NewGuid();
            Payload = payload;
            Metadata = new Metadata
            {
                Application = "surovi",
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow,
                IsDeleted = false
            };
        }

        public Guid Id { get; set; }
        public T Payload { get; set; }
        public Metadata Metadata { get; set; }

        //protected MRow(SerializationInfo info, StreamingContext context)
        //{
        //    Title = info.GetString("Value1");
        //    Author = info.GetString("Value2");
        //    Summary = info.GetString("Value3");
        //}

        //[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        //public void GetObjectData(SerializationInfo info, StreamingContext context)
        //{
        //    info.AddValue("Value1", Title);
        //    info.AddValue("Value2", Author);
        //    info.AddValue("Value3", Summary);
        //}
    }
}
