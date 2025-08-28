using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using TheMovies.MVVM.Model.Classes;
using TheMovies.MVVM.Model.Repositories;

namespace TheMovies.MVVM.ViewModel
{
    public class BookingViewModel : ViewModelBase
    {
        private readonly FileBookingRepository bookingRepository = new FileBookingRepository("bookings.csv");

        public ObservableCollection<Booking> Bookings;
        public static ICollectionView? BookingsCollectionView { get; set; }
        public ICollectionView? MovieProgramsCollectionView { get; set; }
        public ICollectionView? MovieScreeningsCollectionView { get; set; }

        private int ticketCount;
        public int TicketCount
        {
            get { return ticketCount; }
            set { ticketCount = value; OnPropertyChanged(); }
        }
        private int availableTickets;
        public int AvailableTickets
        {
            get { return availableTickets; }
            set { availableTickets = value; OnPropertyChanged(); }
        }
        private DateTime bookingDate;
        public DateTime BookingDate
        {
            get { return bookingDate; }
            set { bookingDate = value; OnPropertyChanged(); }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; OnPropertyChanged(); }
        }

        private string email;

        public string Email
        {
            get { return email; }
            set { email = value; OnPropertyChanged(); }
        }

        private int phone;
        public int Phone
        {
            get { return phone; }
            set { phone = value; OnPropertyChanged(); }
        }

        private MovieScreening selectedMovieScreening;
        public MovieScreening SelectedMovieScreening
        {
            get { return selectedMovieScreening; }
            set { selectedMovieScreening = value; OnPropertyChanged(); }

        }
        private Booking selectedBooking;
        public Booking SelectedBooking
        {  
            get { return selectedBooking; }
            set { selectedBooking = value; OnPropertyChanged(); }
        }
        private string searchTerm = string.Empty;
        public string SearchTerm
        {
            get { return searchTerm; }
            set
            {
                searchTerm = value;
                OnPropertyChanged(nameof(BookingsFilter));
                BookingsCollectionView.Refresh();
            }
        }

        public ICommand AddBookingCommand { get; }
        public ICommand UpdateBookingCommand { get; }
        public ICommand RemoveBookingCommand { get; }

        private bool CanAddBooking() => TicketCount != 0 && SelectedMovieScreening != null && !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Email);
                                     
        private bool CanUpdateBooking() => SelectedBooking != null;
        private bool CanRemoveBooking() => SelectedBooking != null;

        public BookingViewModel()
        {
            Bookings = new ObservableCollection<Booking>(bookingRepository.GetAll());
            BookingsCollectionView = CollectionViewSource.GetDefaultView(Bookings);
            BookingsCollectionView.Filter = BookingsFilter;

            MovieProgramsCollectionView = CollectionViewSource.GetDefaultView(MovieProgramViewModel.MovieProgramsCollectionView);
            MovieScreeningsCollectionView = CollectionViewSource.GetDefaultView(MovieProgramViewModel.MovieScreeningsCollectionView);

            AddBookingCommand = new RelayCommand(_ => AddBooking(), _ => CanAddBooking());
            UpdateBookingCommand = new RelayCommand(_ => UpdateBooking(), _ => CanUpdateBooking());
            RemoveBookingCommand = new RelayCommand(_ => RemoveBooking(), _ => CanRemoveBooking());
        }

        private void AddBooking()
        {
            if (SelectedMovieScreening.AvailableTickets >= TicketCount)
            {
                Customer customer = new Customer(Guid.NewGuid(), Name, Email, Phone);
                Booking booking = new Booking(Guid.NewGuid(), TicketCount, BookingDate, customer, SelectedMovieScreening);
                bookingRepository.AddBooking(booking);
                Bookings.Add(booking);
                SelectedMovieScreening.AvailableTickets =- TicketCount;

                //Vis bekræftelse
                MessageBox.Show($"Bookingen er tilføjet til listen.",
                                 "Udført", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else 
            { 
                MessageBox.Show($"Der er ikke nok ledige billetter til denne forestilling." +
                $"\nAntal ledige billetter: {SelectedMovieScreening.AvailableTickets}", "Booking kunne ikke gennemføres",
                MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            //Nulstil felter
            TicketCount = 0;
            BookingDate = DateTime.Now;
            Name = string.Empty;
            Email = string.Empty;
            Phone = 0;
            SelectedMovieScreening = null;
        }
        private void UpdateBooking()
        {
            //opdater movie i repository
            bookingRepository.UpdateBooking(SelectedBooking);

            //vis bekræftelse 
            MessageBox.Show($"Ændringerne er gemt", "Udført", MessageBoxButton.OK, MessageBoxImage.Information);

            //nulstil valgt movie
            SelectedBooking = null;
        }

        private void RemoveBooking()
        {

            MessageBoxResult result = MessageBox.Show($"Er du sikker på, at du vil fjerne bookingen for {SelectedBooking.Customer.Name}?",
            "Er du enig?", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                
                bookingRepository.RemoveBooking(SelectedBooking);
                Bookings.Remove(SelectedBooking);

                MessageBox.Show($"Bookingen er fjernet fra listen.",
                                "Udført", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show($"Bookingen blev ikke fjernet fra listen.",
                                "Annulleret", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            SelectedBooking = null;
        }
        private bool BookingsFilter(object obj)
        {
            if (obj is Booking booking
                )
            {
                return booking.Customer.Name.Contains(SearchTerm, StringComparison.InvariantCultureIgnoreCase) ||
                       booking.Customer.Email.Contains(SearchTerm, StringComparison.InvariantCultureIgnoreCase) ||
                       booking.Customer.Phone.ToString().Contains(SearchTerm, StringComparison.InvariantCultureIgnoreCase);
                // Skal kunne søge efter bookinger ud fra kundeinfo som navn, mail og nummer
            }
            return false;
        }
        
    }

}
