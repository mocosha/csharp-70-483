using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MTable
{
    public interface IMTable<T> where T : ISerializable
    {
        IEnumerable<T> GetAll();
        IEnumerable<T> Filter(Predicate<T> filter);
        int Add(T value);
        int Delete(Predicate<T> filter);

        IEnumerable<T> SearchByIndex(string indexName, string value);
        IMTable<T> CreateIndex(string name, Func<T, string> column);
    }
}
