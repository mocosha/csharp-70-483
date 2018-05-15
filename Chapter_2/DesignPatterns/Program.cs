using System;
using System.Collections.Generic;
using static DesignPatterns.CORAndSpecification;

namespace DesignPatterns
{
    class Program
    {
        static void VisitorSample()
        {
            Document doc = new Document();
            var boldText = new BoldText
            {
                Name= "foo name",
                Parts = new List<DocumentPart>
                {
                    new Hyperlink
                    {
                        Text = "link test",
                        Url = "www.test.com"
                    },
                }
            };

            var plainText = new Paragraph
            {
                Parts = new List<DocumentPart>
                {
                    new Hyperlink { Text = "link test", Url = "www.test.com"},
                    boldText
                }
            };

            doc.AddDocument(plainText);
            doc.AddDocument(new BoldText { Text = "bojan" });

            HtmlVisitor visitor = new HtmlVisitor();
            doc.Accept(visitor);
            Console.WriteLine("Html:\n" + visitor.Output);
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

        static void CORSample()
        {
            var reservations = new List<Reservation>()
            {
                new Reservation{ ConfirmationNumber= "1",  Price = 10, Car = "Suzuki" },
                new Reservation{ ConfirmationNumber= "2", Price = 15, Car = "Suzuki" },
                new Reservation{ ConfirmationNumber= "2", Price = 23, Car = "Nisan" },
                new Reservation{ ConfirmationNumber= "3", Price = 30, Car = "Toyota" },
                new Reservation{ ConfirmationNumber= "4", Price = 40, Car = "Fiat" },
                new Reservation{ ConfirmationNumber= "5", Price = 40, Car = "Ford" },
                new Reservation{ ConfirmationNumber= "6", Price = 60, Car = "Ford" },
                new Reservation{ ConfirmationNumber= "7", Price = 90, Car = "BMW" },
                new Reservation{ ConfirmationNumber= "8", Price = 80, Car = "Merzedes" },
                new Reservation{ ConfirmationNumber= "9", Price = 140, Car = "Merzedes" }
            };

            ISpecification<Reservation> lowBidget = new Specification<Reservation>(r => r.Price <= 20);
            ISpecification<Reservation> mediumBidget = new Specification<Reservation>(r => r.Price > 20 && r.Price <= 50);
            ISpecification<Reservation> highBidget = new Specification<Reservation>(r => r.Price > 50 && r.Price <= 100);
            ISpecification<Reservation> premiumBudget = new Specification<Reservation>(r => r.Price > 100);

            ISpecification<Reservation> compactCar = new Specification<Reservation>(r => new List<string> { "Suzuki", "Nisan" }.Contains(r.Car));
            ISpecification<Reservation> midSizeCar = new Specification<Reservation>(r => new List<string> { "Fiat", "Toyota" }.Contains(r.Car));
            ISpecification<Reservation> entryLevelLuxury = new Specification<Reservation>(r => new List<string> { "Ford", "Opel", "Renault" }.Contains(r.Car));
            ISpecification<Reservation> luxuryCar = new Specification<Reservation>(r => new List<string> { "BMW", "Audi", "Merzedes", "Masserati" }.Contains(r.Car));

            ISpecification<Reservation> compactLowBudget = compactCar.And(lowBidget);

            var compactLowBudgetRides = reservations.FindAll(r => compactLowBudget.IsSatisfiedBy(r));

            Console.WriteLine("Compact low budget rides: ");
            foreach (var r in compactLowBudgetRides)
            {
                Console.WriteLine("#{0}; price={1}; car={2}", r.ConfirmationNumber, r.Price, r.Car);
            }

            var basicDispatcherCenter = new BasicDispatcherCenter();
            basicDispatcherCenter.SetSpecification(
                lowBidget
                    .Or(compactCar)
                    .Or(lowBidget.And(midSizeCar))
            );

            var midDispatcherCenter = new MidDispatcherCenter();
            midDispatcherCenter.SetSpecification(
                midSizeCar.And(lowBidget.Not())
                .Or(mediumBidget)
                .Or(entryLevelLuxury.And(mediumBidget)));

            var premiumDispatcherCenter = new PremiumDispatcherCenter();
            premiumDispatcherCenter.SetSpecification(
                entryLevelLuxury.And(highBidget)
                .Or(luxuryCar)
                .Or(premiumBudget));

            basicDispatcherCenter.SetSuccessor(midDispatcherCenter);
            midDispatcherCenter.SetSuccessor(premiumDispatcherCenter);

            reservations.ForEach((r) => { basicDispatcherCenter.HandleRequest(r); });
        }

        static void Main(string[] args)
        {
            //SingletonSample();
            //Singleton.Instance.Log("test entry");;

            //FluentInterfaceSample();

            VisitorSample();

            //CORSample();

            Console.ReadLine();
        }
    }
}
