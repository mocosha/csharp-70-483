namespace DesignPatterns
{
    public abstract class CarType
    {
        public abstract decimal GetPrice();
        public abstract int MaxPassengers();
    }

    class Sedan : CarType
    {
        public override decimal GetPrice() => 10.5M;

        public override int MaxPassengers()
        {
            return 3;
        }
    }

    class SUV : CarType
    {
        public override decimal GetPrice() => 20M;

        public override int MaxPassengers()
        {
            return 6;
        }
    }

    public class Decorator : CarType
    {
        private CarType _carType;

        public Decorator(CarType carType)
        {
            _carType = carType;
        }

        public override decimal GetPrice()
        {
            return _carType.GetPrice();
        }

        public override int MaxPassengers()
        {
            return _carType.MaxPassengers();
        }
    }

    class DoublePrice : Decorator
    {
        public DoublePrice(CarType carType) : base(carType)
        {
        }

        public override decimal GetPrice()
        {
            return base.GetPrice() * 2;
        }
    }

    class IncreasePassenger : Decorator
    {
        public IncreasePassenger(CarType carType) : base(carType)
        {
        }

        public override int MaxPassengers()
        {
            return base.MaxPassengers() + 1;
        }
    }
}
