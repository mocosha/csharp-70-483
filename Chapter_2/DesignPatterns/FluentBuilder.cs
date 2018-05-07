using System;

namespace DesignPatterns
{
    public class Reservation
    {
        public string ConfirmationNumber { get; set; }
        public DateTime PickupDateTime { get; set; }
        public string Pickup { get; set; }
        public string DropOff { get; set; }
        public string Pax { get; set; }
        public double Price { set; get; }
        public string Car { set; get; }

        public override string ToString()
        {
            return $"Reservation #{ConfirmationNumber} at {PickupDateTime}\n\tPickup: {Pickup}\n\tDropoff: {DropOff}\n\tPax: {Pax}";
        }
    }
    /// <summary>
    /// References: 
    ///     - http://www.stefanoricciardi.com/2010/04/14/a-fluent-builder-in-c/
    ///     - https://en.wikipedia.org/wiki/Fluent_interface
    /// </summary>
    public class ReservationBuilder
    {
        private string _confirmationNumber;
        private DateTime _pickupDateTime;
        private string _pickup;
        private string _dropOff;
        private string _pax;

        public ReservationBuilder WithConfirmationNumber(string confirmationNumber)
        {
            _confirmationNumber = confirmationNumber;
            return this;
        }

        public ReservationBuilder WithPickupDateTime(DateTime pickupDateTime)
        {
            _pickupDateTime = pickupDateTime;
            return this;
        }

        public ReservationBuilder WithPickup(string pickup)
        {
            _pickup = pickup;
            return this;
        }

        public ReservationBuilder WithDropoff(string dropoff)
        {
            _dropOff = dropoff;
            return this;
        }

        public ReservationBuilder WithPax(string pax)
        {
            _pax = pax;
            return this;
        }

        public static implicit operator Reservation(ReservationBuilder rb)
        {
            return new Reservation
            {
                ConfirmationNumber = rb._confirmationNumber,
                PickupDateTime = rb._pickupDateTime,
                Pickup = rb._pickup,
                DropOff = rb._dropOff,
                Pax = rb._pax
            };
        }

    }
}
