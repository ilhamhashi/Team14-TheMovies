using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheMovies.MVVM.Model.Classes;

namespace TheMovies.MVVM.Model.Repositories
{
    interface IBookingRepository
    {
        IEnumerable<Booking> GetAll();
        void AddBooking(Booking booking);
        void UpdateBooking(Booking booking);
        void RemoveBooking(Booking booking);
    }
}
