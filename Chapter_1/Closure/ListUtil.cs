using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Closure
{
    class ListUtil
    {
        public static IEnumerable<int> Filter(List<int> l, Func<int, bool> filter)
        {
            var f = l.Where(filter);
            Console.WriteLine(string.Join(", ", f));
            return f;
        }
    }
}
