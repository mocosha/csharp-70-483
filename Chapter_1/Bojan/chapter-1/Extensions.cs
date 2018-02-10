using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bojan
{
    public static class Extensions
    {
        public static int MyMaxx(this int[] data)
        {
            var max = int.MinValue;

            foreach (var item in data)
                max = max > item ? max : item;

            return max;
        }

        public static int[] SubArray(this int[] data, int index, int length)
        {
            int[] result = new int[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }
    }
}
