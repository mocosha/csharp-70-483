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
            Document doc = new Document();
            var plainText = new PlainText();
            plainText.Text = "Header";
            plainText.Parts = new List<DocumentPart>
            {
                new Hyperlink{Text = "link test", Url = "www.test.com"},
                new BoldText { Text = "bold test"}
            };

            doc.AddCar(plainText);
            doc.AddCar(new BoldText { Text = "bojan" });

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

        static void Main(string[] args)
        {
            //SingletonSample();
            //Singleton.Instance.Log("test entry");;

            //FluentInterfaceSample();

            VisitorSample();

            Console.ReadLine();
        }
    }

    public class SpaceShip
    {
        public virtual string GetShipType()
        {
            return "SpaceShip";
        }
    }

    public class ApolloSpacecraft : SpaceShip
    {
        public override string GetShipType()
        {
            return "ApolloSpacecraft";
        }
    }

    public class Asteroid
    {
        public virtual void CollideWith(SpaceShip ship)
        {
            Console.WriteLine("Asteroid hit a SpaceShip");
        }
        public virtual void CollideWith(ApolloSpacecraft ship)
        {
            Console.WriteLine("Asteroid hit an ApolloSpacecraft");
        }
    }

    public class ExplodingAsteroid : Asteroid
    {
        public override void CollideWith(SpaceShip ship)
        {
            Console.WriteLine("ExplodingAsteroid hit a SpaceShip");
        }
        public override void CollideWith(ApolloSpacecraft ship)
        {
            Console.WriteLine("ExplodingAsteroid hit an ApolloSpacecraft");
        }
    }
}
