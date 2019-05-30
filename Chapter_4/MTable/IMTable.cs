using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MTable
{
    public interface IMTable<T> where T : ISerializable
    {
        T Get(Guid id);
        IEnumerable<T> Find(Predicate<T> filter);
        IEnumerable<T> GetAll();

        void Add(T value);
        void Delete(Guid id);
    }
}
