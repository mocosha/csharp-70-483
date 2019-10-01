﻿using System.Collections.Generic;

namespace MTable
{
    interface IIndex<T>
    {
        void AddProperty(T value, long position);
        void Save();
        void Recreate();
        List<long> GetPositions(string value);
    }
}
