using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using TheMovies.MVVM.Model.Classes;
using TheMovies.MVVM.Model.Repositories;
using TheMovies.MVVM.View;


namespace TheMovies.MVVM.ViewModel
{
    public class BookingViewModel : ViewModelBase
    {
        private readonly FileBookingRepository bookingRepository = new FileBookingRepository("bookings.csv");

        public ObservableCollection<Booking> Bookings;
        public static ICollectionView? BookingsCollectionView { get; set; }

        private Guid bookingId;
        public Guid BookingId
        {
            get { return bookingId; }
            set { bookingId = value; }
        }
        private int ticketCount;
        public int TicketCount
        {
            get { return ticketCount; }
            set { ticketCount = value; }
        }
        private DateTime bookingDate;
        public DateTime BookingDate
        {
            get { return bookingDate; }
            set { bookingDate = value; }
        }
        private Customer newCustomer;
        public Customer NewCustomer
        {
            get { return newCustomer; }
            set { newCustomer = value; }
        }
        private MovieScreening movieScreening;
        public MovieScreening MovieScreening
        {
            get { return movieScreening; }
            set { movieScreening = value; }

        }

        private Booking selectedBooking;
        public Booking SelectedBooking
        {  
            get { return selectedBooking; }
            set { selectedBooking = value; }
        }

        public ICommand AddBookingCommand { get; }
        public ICommand UpdateBookingCommand { get; }
        public ICommand RemoveBookingCommand { get; }

        private bool CanAddBooking() => TicketCount != 0 && NewCustomer != null && MovieScreening != null;
                                     
        private bool CanUpdateBooking() => SelectedBooking != null;
        private bool CanRemoveBooking() => SelectedBooking != null;

        public MainWindowViewModel()
        {
            Bookings = new ObservableCollection<Booking>(bookingRepository.GetAll());
            BookingsCollectionView = CollectionViewSource.GetDefaultView(Bookings);
            BookingsCollectionView.Filter = BookingsFilter;

    
            AddBookingCommand = new RelayCommand(_ => AddBooking(), _ => CanAddBooking());
            UpdateBookingCommand = new RelayCommand(_ => UpdateBooking(), _ => CanUpdateBooking());
            RemoveBookingCommand = new RelayCommand(_ => RemoveBooking(), _ => CanRemoveBooking());
        }


        private void AddBooking()

        {
            Booking booking = new Booking(Guid.NewGuid(), TicketCount, BookingDate, NewCustomer, MovieScreening);
            bookingRepository.AddBooking(booking);
            Bookings.Add(booking);

            //Vis bekræftelse
            MessageBox.Show($"Reservationen er tilføjet til listen.",
                             "Udført", MessageBoxButton.OK, MessageBoxImage.Information);

            //Nulstil felter
            TicketCount = 0;
            BookingDate = DateTime.Now;
            NewCustomer = null;
            MovieScreening = null;
        }



        private void RemoveBooking()
        {

            MessageBoxResult result = MessageBox.Show($"Er du sikker på, at du vil fjerne reservation for {SelectedBooking.Customer.Name}?",
            "Er du enig?", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                //fjern movie i repository og observablecollection
                bookingRepository.RemoveBooking(SelectedBooking);
                Bookings.Remove(SelectedBooking);

                MessageBox.Show($"Reservationen er fjernet fra listen.",
                                "Udført", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show($"Reservationen blev ikke fjernet fra listen.",
                                "Annulleret", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            SelectedBooking = null;
        }

        private void AddBooking()
        {
            Booking booking = new Booking(Guid.NewGuid(), TicketCount, BookingDate, NewCustomer, MovieScreening);
            bookingRepository.AddBooking(booking);
            Bookings.Add(booking);

            //Vis bekræftelse
            MessageBox.Show($"Reservationen er tilføjet til listen.",
                                   "Udført", MessageBoxButton.OK, MessageBoxImage.Information);

            //Nulstil felter
            TicketCount = 0;
            BookingDate = DateTime.Now;
            NewCustomer = null;
            MovieScreening = null;
        }
    }

}
