using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatterns
{
    class Program
    {
        static void VisitorSample()
        {
            Offer e = new Offer();
            e.AddCar(new Sedan());
            e.AddCar(new Van() { BaseRate = 20 });
            e.AddCar(new Suv());

            var visitor = new RateEngineVisitor();
            e.Accept(visitor);
        }

        static void SingletonSample()
        {
            Singleton.Instance.Log("Singeton sample method started");
        }

        static void FluentInterfaceSample()
        {
            ReservationBuilder rb = new ReservationBuilder();

            Reservation reservation = rb.WithConfirmationNumber("17001212")
                .WithPickupDateTime(DateTime.Now.AddDays(2))
                .WithPickup("Milutina Milankovica 22")
                .WithDropoff("Jurija Gagarina 12")
                .WithPax("Bojan Pantovic");

            Console.WriteLine(reservation);
        }

        static void Main(string[] args)
        {
            //SingletonSample();
            //Singleton.Instance.Log("test entry");;

            //FluentInterfaceSample();

            VisitorSample();

            Console.ReadLine();
        }
    }
}
