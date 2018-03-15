using System;

namespace MulticastDelegate
{
    class Animal { }
    class Dog : Animal { }

    class Program
    {
        public delegate void SimpleDel(string str);

        public static void Hello(string msg)
        {
            Console.WriteLine("\tHello, {0}!", msg);
        }

        public static void Goodbye(string msg)
        {
            Console.WriteLine("\tGoodbye, {0}!", msg);
        }

        static void MulticastDelegatExample()
        {
            SimpleDel hello = Hello;
            SimpleDel goodbye = Goodbye;
            SimpleDel multicast1 = hello + goodbye;
            SimpleDel multicast2 = (SimpleDel)Delegate.Combine(hello, goodbye);

            Console.WriteLine("Invoking hello delegat");
            hello("Bojan");
            Console.WriteLine("Invoking goodbye delegat");
            goodbye("Bojan");

            Console.WriteLine("Invoking multicast1 delegat");
            multicast1("Bojan");

            Console.WriteLine("Invoking multicast2 delegat");
            multicast2("Bojan");
        }

        public delegate Animal VarianceDel(Dog dog);

        public static Dog Method1(Dog dog)
        {
            Console.WriteLine("Method1");
            return new Dog();
        }

        public static Animal Method2(Dog dog)
        {
            Console.WriteLine("Method2");
            return new Animal();
        }

        public static Animal Method3(Animal animal)
        {
            Console.WriteLine("Method3");
            return new Animal();
        }

        static void CovarianceExample()
        {
            Console.WriteLine("Covariance Example");
            VarianceDel del = Method1;
            del += Method2;
            //Thus, covariance allows you to assign a method to the delegate that has a less derived return type.
            Animal animal = del(new Dog());
        }

        static void ContravarianceExample()
        {
            Console.WriteLine("Contravariance Example");
            VarianceDel del = Method1;
            del += Method2;
            //Method3 has a parameter of Animal class whereas delegate expects a parameter of Dog class. 
            del += Method3;
            Animal animal = del(new Dog());
        }

        static void ShowAllDelegatMethodsExample()
        {
            Console.WriteLine("Show all delegat methods example");
            SimpleDel hello = Hello;
            SimpleDel goodbye = Goodbye;
            SimpleDel multicast1 = hello + goodbye;

            ShowAllMethods(multicast1);
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

        static void Main(string[] args)
        {
            //MulticastDelegatExample();
            //CovarianceExample();
            //ContravarianceExample();
            ShowAllDelegatMethodsExample();

            Console.ReadKey();
        }
    }
}
