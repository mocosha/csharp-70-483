using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MTable
{
    public interface IMTable<T> where T : ISerializable
    {
        IEnumerable<T> Find(Predicate<T> filter);
        IEnumerable<T> GetAll();
        int Add(T value);
        int Delete(Predicate<T> filter);
    }
}
