using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bojan
{
    class Animal { }
    class Dogs : Animal { }

    class Program
    {
        public delegate void SimpleDel(string str);

        
        public delegate Animal HandlerMethod();

        public static Animal AnimalHandler()
        {
            return null;
        }

        public static Dogs DogsHandler()
        {
            return null;
        }

        public static void Hello(string msg)
        {
            Console.WriteLine("Hello, {0}!", msg);
        }

        public static void Goodbye(string msg)
        {
            Console.WriteLine("Goodbye, {0}!", msg);
        }

        static void Main(string[] args)
        {
            //MULTICAST DELEGAT
            //https://docs.microsoft.com/en-us/dotnet/api/system.multicastdelegate?view=netframework-4.7.1
            //SimpleDel del = Hello;
            //del += Goodbye;
            //SimpleDel hello = Hello;
            //SimpleDel goodbye = Goodbye;
            //SimpleDel del = (SimpleDel)Delegate.Combine(hello, goodbye);
            //ShowAllMethods(del);
            //del("Bojan");

            //TODO: get all results?

            //Covariance and Contravariance 
            //http://www.tutorialsteacher.com/csharp/csharp-covariance-and-contravariance
            HandlerMethod handlerAnimal = AnimalHandler;
            // Covariance enables this assignment.  
            HandlerMethod handlerDogs = DogsHandler;

            Console.ReadKey();
        }

        private static void ShowAllMethods(Delegate del)
        {
            int i = 1;
            foreach (var item in del.GetInvocationList())
            {
                Console.WriteLine("Method {0}: {1}", i, item.Method.Name);
                i++;
            }
        }
    }
}
