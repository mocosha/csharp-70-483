using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;


namespace Deadlock
{
    /// <summary>
    /// Notes:
    ///   https://stackoverflow.com/questions/6029804/how-does-lock-work-exactly
    ///   https://msdn.microsoft.com/en-us/library/de0542zz.aspx?cs-save-lang=1&cs-lang=cpp#code-snippet-2
    ///   https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/threading/thread-synchronization
    ///   http://etutorials.org/Programming/.NET+Framework+Essentials/Chapter+2.+The+Common+Language+Runtime/2.3+Metadata/
    /// </summary>
    class Program
    {

        static void SimpleExample()
        {
            int n = 0;
            object _lock = new object();

            var up = Task.Run(() =>
            {
                lock (_lock)
                {
                    for (int i = 0; i < 1000000; i++)
                    {
                        n++;
                    }
                }
            });

            lock (_lock)
            {
                for (int i = 0; i < 1000000; i++)
                {
                    n--;
                }
            }

            up.Wait();
            Console.WriteLine(n);
        }

        static void DeadlockExample()
        {
            object lockA = new object();
            object lockB = new object();

            var up = Task.Run(() =>
            {
                lock (lockA)
                {
                    Thread.Sleep(3000);
                    lock (lockB)
                    {
                        Console.WriteLine("Locked A and B");
                    }
                }
            });

            lock (lockB)
            {
                lock (lockA)
                {
                    Console.WriteLine("Locked A and B");
                }
            }
            up.Wait();
        }

        static void FileSystemExample()
        {
            string path = "foo.txt";

            //  File.Create(path);

            var writter1 = new Writter(path, false);

            var t1 = Task.Run(() =>
            {
                writter1.Write("adasdasdsdasdasdasdasdasdasdas", (string text) =>
                {
                    Console.WriteLine("Task1: {0}", text);
                });
            });


            var t2 = Task.Run(() =>
            {
                writter1.Write("adasdasdsdasdasdasdasdasdasdas", (string text) =>
                {
                    Console.WriteLine("Task2: {0}", text);
                });
            });


            var t3 = Task.Run(() =>
            {
                writter1.Write("adasdasdsdasdasdasdasdasdasdas", (string text) =>
                {
                    Console.WriteLine("Task3: {0}", text);
                });
            });


            var t4 = Task.Run(() =>
            {
                writter1.Write("adasdasdsdasdasdasdasdasdasdas", (string text) =>
                {
                    Console.WriteLine("Task4: {0}", text);
                });
            });

            t1.Wait();
            t2.Wait();
            t3.Wait();
            t4.Wait();

        }

        static void Main(string[] args)
        {
            try
            {
                //SimpleExample();
                // FileSystemExample();
                DeadlockExample();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.ReadKey();
        }
    }
}
