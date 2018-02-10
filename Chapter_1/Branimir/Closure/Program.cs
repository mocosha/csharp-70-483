//Things start to become more complex when your lambda function starts referring to variables declared outside 
//of the lambda expression(or to the this reference). Normally, when control leaves the scope of a variable, 
//the variable is no longer valid. But what if a delegate refers to a local variable and is then returned to 
//the calling method? Now, the delegate has a longer life than the variable.To fix this, the compiler generates
//code that makes the life of the captured variable at least as long as the longest-living delegate. 
//This is called a Closure

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Closure
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--CLOSURES--");
            FilterExample();
            //MessangerExample();
            Console.ReadLine();
        }

        private static void FilterExample()
        {
            var l = Enumerable.Range(0, 9).ToList();
            ListUtil.Filter(l, i => i > 5);

            Console.WriteLine("Enter number 0..9");
            var k = Console.ReadKey().KeyChar - '0';

            Console.WriteLine();
            Console.WriteLine("Anonymous filter (>)");

            ListUtil.Filter(l, i => i > k);

            Console.WriteLine();
            Console.WriteLine("Delegate filter (<)");

            //k++;

            Func<int, bool> filterFun = 
                delegate(int i) 
                {
                    return i < k;
                };

            //k++;

            ListUtil.Filter(l, filterFun);
        }

        private static void MessangerExample()
        {
            var postMessageAlice = Messenger.CreatePostMessageAction("Alice");
            var postMessageBob = Messenger.CreatePostMessageAction("Bob");

            postMessageAlice("Hi");
            postMessageAlice("My name is Alice");
            postMessageBob("Hi Alice");
            postMessageBob("B");
            postMessageBob("o");
            postMessageBob("b");
            postMessageAlice("Don't be silly");
        }
    }
}
