using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Closure
{

    class Messenger
    {
        private static int totalCounter = 0;

        public static Func<string, string> CreatePostMessageAction(string name)
        {
            int counter = 0;
           
            return delegate (string message)
            {
                // Yes, it could be done in one statement; 
                // but it is clearer like this.
                counter++;
                totalCounter++;
                var result = string.Format("[{0}:{1} of {2}]> {3}", name, counter, totalCounter, message);
                Console.WriteLine(result);
                return result;
            };
        }

    }
}
