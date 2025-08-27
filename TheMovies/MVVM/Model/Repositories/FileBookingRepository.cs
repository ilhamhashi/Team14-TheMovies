using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheMovies.MVVM.Model.Classes;

namespace TheMovies.MVVM.Model.Repositories
{
    class FileBookingRepository : IBookingRepository
    {
        private readonly string bookingFilePath;

        public FileBookingRepository(string filePath)
        {
            bookingFilePath = filePath;

            //Hvis filen ikke eksisterer i forvejen oprettes en ny med kolonnetitler. 
            if (!File.Exists(bookingFilePath))

            {
                File.AppendAllText(bookingFilePath, "BookingId, Billetantal, Dato, Kunde, Forestilling" + Environment.NewLine);
            }
        }

        public IEnumerable<Booking> GetAll()
        {
            try
            {
                return File.ReadAllLines(bookingFilePath)
                           .Skip(1)
                           .Where(line => !string.IsNullOrEmpty(line)) // Undgå tomme linjer
                           .Select(Booking.FromString)
                           .ToList();
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Fejl ved læsning af fil: {ex.Message}");
                return [];
            }
        }

        public void AddBooking(Booking booking)
        {
            try
            {
                File.AppendAllText(bookingFilePath, booking.ToString() + Environment.NewLine);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fejl ved skrivning til fil: {ex.Message}");
            }
        }

        public void RemoveBooking(Booking booking)
        {
            List<Booking> bookings = GetAll().ToList();
            bookings.RemoveAll(b => b.Id == booking.Id);
            RewriteFile(bookings);
        }

        public void UpdateBooking(Booking booking)
        {
            List<Booking> bookings = GetAll().ToList();
            int index = bookings.FindIndex(b => b.Id == booking.Id);
            if (index != -1)
            {
                bookings[index] = booking;
                RewriteFile(bookings);
            }
        }

        private void RewriteFile(List<Booking> bookings)
        {
            try
            {
                List<string> bookingList = bookings.Select(b => b.ToString()).ToList();
                bookingList.Insert(0, "BookingId, Billetantal, Dato, Kunde, Forestilling");
                File.WriteAllLines(bookingFilePath, bookingList);
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Fejl ved skrivning til fil: {ex.Message}");
            }
        }

        
    }
}
