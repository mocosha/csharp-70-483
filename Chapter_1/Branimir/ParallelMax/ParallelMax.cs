using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelMax
{
    public static partial class Enumerable
    { 
        public static int Max(this IEnumerable<int> source)
        {
            if (source == null) throw new ArgumentNullException("source");
            int value = 0;
            bool hasValue = false;
            foreach (int x in source)
            {
                if (hasValue)
                {
                    if (x > value) value = x;
                }
                else
                {
                    value = x;
                    hasValue = true;
                }
            }
            if (hasValue) return value;
            throw new Exception("No Elements");
        }
    }
}
