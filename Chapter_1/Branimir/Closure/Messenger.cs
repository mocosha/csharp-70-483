using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Closure
{

    class Messenger
    {
        public static Func<string, string> CreatePostMessageAction(string name)
        {
            int counter = 0;
           
            return delegate (string message)
            {
                // Yes, it could be done in one statement; 
                // but it is clearer like this.
                counter++;
                var result = string.Format("[{0}:{1}]> {2}", name, counter, message);
                Console.WriteLine(result);
                return result;
            };
        }

    }
}
