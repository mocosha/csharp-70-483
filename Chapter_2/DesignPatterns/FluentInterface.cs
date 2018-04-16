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

        public override string ToString()
        {
            return $"Reservation #{ConfirmationNumber} at {PickupDateTime}\n\tPickup: {Pickup}\n\tDropoff: {DropOff}\n\tPax: {Pax}";
        }
    }

    public class ReservationBuilder
    {
        private Reservation _reservation = new Reservation();
        
        public ReservationBuilder CreateNew(string confirmationNumber)
        {
            _reservation.ConfirmationNumber = confirmationNumber;
            return this;
        }

        public ReservationBuilder WithPickupDateTime(DateTime pickupDateTime)
        {
            _reservation.PickupDateTime = pickupDateTime;
            return this;
        }

        public ReservationBuilder WithPickup(string pickup)
        {
            _reservation.Pickup = pickup;
            return this;
        }

        public ReservationBuilder WithDropoff(string dropoff)
        {
            _reservation.DropOff = dropoff;
            return this;
        }

        public ReservationBuilder WithPax(string pax)
        {
            _reservation.Pax = pax;
            return this;
        }

        public static implicit operator Reservation(ReservationBuilder rb)
        {
            return rb._reservation;
        }

    }
}
