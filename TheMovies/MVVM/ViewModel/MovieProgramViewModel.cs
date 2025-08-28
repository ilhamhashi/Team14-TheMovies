using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using TheMovies.MVVM.Model.Classes;
using TheMovies.MVVM.Model.Repositories;
using TheMovies.MVVM.View;

namespace TheMovies.MVVM.ViewModel
{
    public class MovieProgramViewModel : ViewModelBase
    {
        private readonly FileMovieProgramRepository movieProgramRepository = new FileMovieProgramRepository("movieprograms.csv");
        private readonly FileMovieScreeningRepository movieScreeningRepository = new FileMovieScreeningRepository("moviescreenings.csv");
        private readonly FileCinemaRepository cinemaRepository = new FileCinemaRepository("cinemas.csv");
        private readonly FileCinemaHallRepository cinemaHallRepository = new FileCinemaHallRepository("cinemaHalls.csv");

        public ObservableCollection<MovieProgram> MoviePrograms;
        public ObservableCollection<MovieScreening> MovieScreenings;
        public ObservableCollection<Cinema> Cinemas;
        public ObservableCollection<CinemaHall> Halls;
        public static ICollectionView MovieProgramsCollectionView { get; set; }
        public static ICollectionView MovieScreeningsCollectionView { get; set; }
        public ICollectionView MoviesCollectionView { get; set; }
        public ICollectionView HallsCollectionView { get; set; }

        private TimeSpan duration;
        public TimeSpan Duration
        {
            get { return duration; }
            set { duration = value; OnPropertyChanged(); }
        }

        private DateTime movieScreeningTime;
        public DateTime MovieScreeningTime
        {
            get { return movieScreeningTime; }
            set { movieScreeningTime = value; OnPropertyChanged(); }
        }

        private DateTime premiereDate;
        public DateTime PremiereDate
        {
            get { return premiereDate; }
            set { premiereDate = value; OnPropertyChanged(); }
        }
        private int availableTickets;
        public int AvailableTickets
        {
            get { return availableTickets; }
            set { availableTickets = value; OnPropertyChanged(); } 
        }
        private Movie movie;
        public Movie Movie
        {
            get { return movie; }
            set { movie = value; OnPropertyChanged(); }
        }
        private CinemaHall hall;

        public CinemaHall Hall
        {
            get { return hall; }
            set { hall = value; }
        }

        private Cinema cinema;
        public Cinema Cinema
        {
            get { return cinema; }
            set { cinema = value; OnPropertyChanged(); }
        }

        private MovieProgram selectedMovieProgram;
        public MovieProgram SelectedMovieProgram
        {
            get { return selectedMovieProgram; }
            set { selectedMovieProgram = value; OnPropertyChanged(); }
        }

        private MovieScreening selectedMovieScreening;
        public MovieScreening SelectedMovieScreening
        {
            get { return selectedMovieScreening; }
            set { selectedMovieScreening = value; }
        }


        private string searchTerm = string.Empty;
        public string SearchTerm
        {
            get { return searchTerm; }
            set
            {
                searchTerm = value;
                OnPropertyChanged(nameof(MovieProgramsFilter));
                MovieProgramsCollectionView.Refresh();
            }
        }
        public ICommand AddMovieProgramCommand { get; }
        public ICommand UpdateMovieProgramCommand { get; }
        public ICommand RemoveMovieprogramCommand { get; }
        public ICommand OpenPrintWindowCommand { get; }
        public ICommand OpenBookingWindowCommand { get; }

        private bool CanAddMovieProgram() => Hall != null && Movie != null;
        private bool CanUpdateMovieProgram() => SelectedMovieProgram != null;
        private bool CanRemoveMovieprogram() => SelectedMovieProgram != null;

        public MovieProgramViewModel()
        {
            MoviePrograms = new ObservableCollection<MovieProgram>(movieProgramRepository.GetAll());
            MovieScreenings = new ObservableCollection<MovieScreening>(movieScreeningRepository.GetAll());
            Cinemas = new ObservableCollection<Cinema>(cinemaRepository.GetAll());
            Halls = new ObservableCollection<CinemaHall>(cinemaHallRepository.GetAll());

            MovieProgramsCollectionView = CollectionViewSource.GetDefaultView(MoviePrograms);
            MovieScreeningsCollectionView = CollectionViewSource.GetDefaultView(MovieScreenings);
            HallsCollectionView = CollectionViewSource.GetDefaultView(Halls);
            MoviesCollectionView = CollectionViewSource.GetDefaultView(MovieViewModel.MoviesCollectionView);
            MovieProgramsCollectionView.Filter = MovieProgramsFilter;


            AddMovieProgramCommand = new RelayCommand(_ => AddMovieProgram(), _ => CanAddMovieProgram());
            UpdateMovieProgramCommand = new RelayCommand(_ => UpdateMovieProgram(), _ => CanUpdateMovieProgram());
            RemoveMovieprogramCommand = new RelayCommand(_ => RemoveMovieProgram(), _ => CanRemoveMovieprogram());        
        }

        private void AddMovieProgram()
        {
            //opret objekt og tilføj til repository og observablecollection
            MovieProgram movieProgram = new MovieProgram(Guid.NewGuid(), Duration, PremiereDate, Movie);
            movieProgramRepository.AddMovieProgram(movieProgram);
            MoviePrograms.Add(movieProgram);

            //vis bekræftelse
            MessageBox.Show($"Program for {Movie.title} i biografsalen {Hall.Name} tilføjet!", "Udført", MessageBoxButton.OK, MessageBoxImage.Information);

            //nulstil felter
            MovieScreeningTime = DateTime.Now;
            PremiereDate = DateTime.Today;
            Movie = null;
            Hall = null;
        }
        private void AddMovieScreening()
        {
            //opret objekt og tilføj til repository og observablecollection
            AvailableTickets = Hall.SeatCount;
            MovieScreening movieScreening = new MovieScreening(Guid.NewGuid(), MovieScreeningTime, SelectedMovieProgram.Id, AvailableTickets, Hall);
            movieScreeningRepository.AddMovieScreening(movieScreening);
            MovieScreenings.Add(movieScreening);

            //vis bekræftelse
            MessageBox.Show($"Forestilling for {Movie.title} i biografsalen {Hall.Name}, tilføjet!", "Udført", MessageBoxButton.OK, MessageBoxImage.Information);

            //nulstil felter
            MovieScreeningTime = DateTime.Now;
            PremiereDate = DateTime.Today;
            Movie = null;
            Hall = null;
        }
        private void UpdateMovieProgram()
        {
            //opdater movie i repository
            movieProgramRepository.UpdateMovieProgram(SelectedMovieProgram);

            //vis bekræftelse 
            MessageBox.Show($"Ændringerne er gemt", "Udført", MessageBoxButton.OK, MessageBoxImage.Information);

            //nulstil valgt movie
            SelectedMovieProgram = null;

        }

        private void RemoveMovieProgram()
        {

            MessageBoxResult result = MessageBox.Show($"Er du sikker på, at du vil fjerne program for {SelectedMovieProgram.Movie.title}?",
            "Er du enig?", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {                
                //fjern movie i repository og observablecollection
                movieProgramRepository.RemoveMovieProgram(SelectedMovieProgram);
                MoviePrograms.Remove(SelectedMovieProgram);

                MessageBox.Show($"Programmet er fjernet fra listen.",
                                "Udført", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show($"Programmet blev ikke fjernet fra listen.",
                                "Annulleret", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            SelectedMovieProgram = null;
        }

        private bool MovieProgramsFilter(object obj)
        {
            if (obj is MovieProgram movieProgram)
            {
                return movieProgram.Movie.title.Contains(SearchTerm, StringComparison.InvariantCultureIgnoreCase) ||
                       movieProgram.Movie.director.Contains(SearchTerm, StringComparison.InvariantCultureIgnoreCase) ||
                       movieProgram.Movie.genre.Contains(SearchTerm, StringComparison.InvariantCultureIgnoreCase) ||
                       movieProgram.PremiereDate.ToString().Contains(SearchTerm, StringComparison.InvariantCultureIgnoreCase);
                // Skal kunne søge efter showtime, premieredato, biografer, filmtitler, filminstruktører og genre.
            }
            return false;
        }
    }
}
