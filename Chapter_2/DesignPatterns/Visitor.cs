using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatterns
{
    /// <summary>
    /// Visitor pattern links:
    /// https://www.codeproject.com/Articles/588882/TheplusVisitorplusPatternplusExplained
    /// http://www.dofactory.com/net/design-patterns    
    /// https://stackoverflow.com/questions/23321669/abstract-tree-with-visitors
    /// </summary>
    public abstract class CarType
    {
        public double Price { get; set; }
        public abstract void Accept(IVisitor visitor);

    }

    public class Sedan : CarType
    {
        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class Suv : CarType
    {
        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class Van : CarType
    {
        public double BaseRate { get; set; }
        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public interface IVisitor
    {
        void Visit(Sedan element);
        void Visit(Suv element);
        void Visit(Van element);
    }

    public class RateEngineVisitor : IVisitor
    {
        public void Visit(Sedan sedan)
        {
            var priceFromRateEngine = 15; //call rate engine to get price
            sedan.Price = priceFromRateEngine;
            Console.WriteLine("Sedan visited. Price: {0}'", sedan.Price);
        }

        public void Visit(Suv suv)
        {
            var priceFromRateEngine = 25; //call rate engine to get price
            suv.Price = priceFromRateEngine;
            Console.WriteLine("Suv visited. Price: {0}", suv.Price);
        }

        public void Visit(Van van)
        {
            var priceFromRateEngine = 25; //call rate engine to get price
            van.Price = van.BaseRate + priceFromRateEngine;

            Console.WriteLine("Van visited. Price: {0}", van.Price);
        }
    }

    public class Offer
    {
        private List<CarType> _carTypes = new List<CarType>();

        public void AddCar(CarType carType)
        {
            _carTypes.Add(carType);
        }

        public void Detach(CarType carType)
        {
            _carTypes.Remove(carType);
        }

        public void Accept(IVisitor visitor)
        {
            foreach (CarType ct in _carTypes)
            {
                ct.Accept(visitor);
            }
        }
    }

}
