using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheMovies.MVVM.Model.Classes
{
    public class Booking
    {
        public Guid Id { get; set; }
        public int TicketCount { get; set; }
        public DateTime BookingDate { get; set; }
        public Customer Customer { get; set; }
        public MovieScreening MovieScreening { get; set; }

        public Booking(Guid id, int ticketCount, DateTime bookingDate, Customer customer, MovieScreening movieScreening)
        {
            Id = id;
            TicketCount = ticketCount;
            BookingDate = bookingDate;
            Customer = customer;
            MovieScreening = movieScreening;
        }

        public override string ToString()
        {
            return $"{Id},{TicketCount},{BookingDate}, {Customer}, {MovieScreening}";
        }

        public static Booking FromString(string input)
        {
            string[] parts = input.Split(',');
            return new Booking
            (
                Guid.Parse(parts[0]),
                int.Parse(parts[1]),
                DateTime.Parse(parts[2]),
                new Customer(Guid.Parse(parts[3]), parts[4], parts[5],int.Parse(parts[6])),
                new MovieScreening(Guid.Parse(parts[7]), DateTime.Parse(parts[8]), Guid.Parse(parts[9]))
            );
        }
    }
}
