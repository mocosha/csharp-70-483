using System;
using System.Runtime.Serialization;

namespace MTable
{
    public class Metadata
    {
        public string Application { get; set; }
        public bool IsDeleted { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }

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
    }
}
