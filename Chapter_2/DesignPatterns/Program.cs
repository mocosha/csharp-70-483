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

        static void FluentInterfaceSample()
        {
            ReservationBuilder rb = new ReservationBuilder();

            Reservation reservation = rb.CreateNew("17001212")
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

            FluentInterfaceSample();

            Console.ReadLine();
        }
    }
}
