using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Branimir
{
    class Maxx
    {
        public Maxx(IEnumerable<int> data)
        {
            var max = int.MinValue;

            foreach (var item in data)
                max = max > item ? max : item;
        }
    }
}
