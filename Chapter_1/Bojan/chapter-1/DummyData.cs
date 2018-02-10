using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bojan
{
    class DummyData
    {
        public static int[] Get()
        {
            int[] items = new int[10000];

            Random randNum = new Random();
            for (int i = 0; i < items.Length; i++)
            {
                items[i] = randNum.Next(0, 10000);
            }

            return items;
        }
    }
}
