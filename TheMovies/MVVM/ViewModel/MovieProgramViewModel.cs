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
        private readonly FileCinemaRepository cinemaRepository = new FileCinemaRepository("cinemas.csv");

        public ObservableCollection<MovieProgram> MoviePrograms;
        public ObservableCollection<Cinema> Cinemas;
        public ICollectionView MovieProgramCollectionView { get; set; }
        public ICollectionView MoviesCollectionView { get; set; }
        public ICollectionView CinemasCollectionView { get; set; }

        private TimeSpan duration;
        public TimeSpan Duration
        {
            get { return duration; }
            set { duration = value; OnPropertyChanged(); }
        }

        private DateTime showTime;
        public DateTime ShowTime
        {
            get { return showTime; }
            set { showTime = value; OnPropertyChanged(); }
        }

        private DateTime premiereDate;
        public DateTime PremiereDate
        {
            get { return premiereDate; }
            set { premiereDate = value; OnPropertyChanged(); }
        }

        private Movie movie;
        public Movie Movie
        {
            get { return movie; }
            set { movie = value; OnPropertyChanged(); }
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
        
        private string searchTerm = string.Empty;
        public string SearchTerm
        {
            get { return searchTerm; }
            set
            {
                searchTerm = value;
                OnPropertyChanged(nameof(MovieProgramsFilter));
                MovieProgramCollectionView.Refresh();
            }
        }
        public ICommand AddMovieProgramCommand { get; }
        public ICommand UpdateMovieProgramCommand { get; }
        public ICommand RemoveMovieprogramCommand { get; }
        public ICommand OpenPrintWindowCommand { get; }

        private bool CanAddMovieProgram() => Cinema != null && Movie != null;
        private bool CanUpdateMovieProgram() => SelectedMovieProgram != null;
        private bool CanRemoveMovieprogram() => SelectedMovieProgram != null;

        public MovieProgramViewModel()
        {
            MoviePrograms = new ObservableCollection<MovieProgram>(movieProgramRepository.GetAll());
            Cinemas = new ObservableCollection<Cinema>(cinemaRepository.GetAll());

            MovieProgramCollectionView = CollectionViewSource.GetDefaultView(MoviePrograms);
            CinemasCollectionView = CollectionViewSource.GetDefaultView(Cinemas);
            MoviesCollectionView = CollectionViewSource.GetDefaultView(MainWindowViewModel.MoviesCollectionView);
            MovieProgramCollectionView.Filter = MovieProgramsFilter;


            OpenPrintWindowCommand = new RelayCommand(_ => OpenPrintWindow(), _ => true);
            AddMovieProgramCommand = new RelayCommand(_ => AddMovieProgram(), _ => CanAddMovieProgram());
            UpdateMovieProgramCommand = new RelayCommand(_ => UpdateMovieProgram(), _ => CanUpdateMovieProgram());
            RemoveMovieprogramCommand = new RelayCommand(_ => RemoveMovieProgram(), _ => CanRemoveMovieprogram());        
        }

        private void OpenPrintWindow()
        {
            PrintView printView = new PrintView();
            printView.Show();
        }

        private void AddMovieProgram()
        {
            //opret objekt og tilføj til repository og observablecollection
            MovieProgram movieProgram = new MovieProgram(Guid.NewGuid(), Duration, ShowTime, PremiereDate, Movie, Cinema);
            movieProgramRepository.AddMovieProgram(movieProgram);
            MoviePrograms.Add(movieProgram);

            //vis bekræftelse
            MessageBox.Show($"Program for {Movie.title} i biograf {Cinema.Name}, {Cinema.City} tilføjet!", "Udført", MessageBoxButton.OK, MessageBoxImage.Information);

            //nulstil felter
            ShowTime = DateTime.Now;
            PremiereDate = DateTime.Today;
            Movie = null;
            Cinema = null;
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

            MessageBoxResult result = MessageBox.Show($"Er du sikker på, at du vil fjerne program for {SelectedMovieProgram.Movie.title} i biograf {SelectedMovieProgram.Cinema.Name}, {SelectedMovieProgram.Cinema.City}?",
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
                       movieProgram.PremiereDate.ToString().Contains(SearchTerm, StringComparison.InvariantCultureIgnoreCase) ||
                       movieProgram.ShowTime.ToString().Contains(SearchTerm, StringComparison.InvariantCultureIgnoreCase) ||
                       movieProgram.Cinema.Name.Contains(SearchTerm, StringComparison.InvariantCultureIgnoreCase) ||
                       movieProgram.Cinema.City.Contains(SearchTerm, StringComparison.InvariantCultureIgnoreCase);
                // Skal kunne søge efter showtime, premieredato, biografer, filmtitler, filminstruktører og genre.
            }
            return false;
        }
    }
}
