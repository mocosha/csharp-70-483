using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatterns
{
    class Program
    {
        static void SingletonSample()
        {
            Singleton.Instance.Log("Singeton sample method started");
        }

        static void Main(string[] args)
        {
            SingletonSample();
            Singleton.Instance.Log("test entry");;

            Console.ReadLine();
        }
    }
}
