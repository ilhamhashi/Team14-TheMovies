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
    internal class MovieProgramViewModel : ViewModelBase
    {
        private readonly MovieProgramFileRepository movieProgramRepository = new MovieProgramFileRepository("movieprograms.csv");

        //public ObservableCollection<Cinema> Cinemas { get; set; }

        public ObservableCollection<MovieProgram> MoviePrograms;
        public static ICollectionView MovieProgramCollectionView { get; set; }
        public static ICollectionView PrintCollectionView { get; set; }

        private Cinema selectedCinemaPrint;
        public Cinema SelectedCinemaPrint
        {
            get { return selectedCinemaPrint; }
            set
            {
                selectedCinemaPrint = value;
                OnPropertyChanged(nameof(PrintFilter));
                PrintCollectionView.Refresh();
            }
        }

        private DateTime selectedMonth;
        public DateTime SelectedMonth
        {
            get { return selectedMonth; }
            set
            {
                selectedMonth = value;
                OnPropertyChanged(nameof(PrintFilter));
                PrintCollectionView.Refresh();
            }
        }

        // Property til programvarighed
        private TimeSpan programDuration;
        public TimeSpan ProgramDuration
        {
            get { return programDuration; }
            set { programDuration = value; OnPropertyChanged(); }
        }

        // Property til forestillingstidspukt
        private DateTime programShowTime;
        public DateTime ProgramShowTime
        {
            get { return programShowTime; }
            set { programShowTime = value; OnPropertyChanged(); }
        }

        // Property til premiere dato
        private DateOnly programPremiereDate;
        public DateOnly ProgramPremiereDate
        {
            get { return programPremiereDate; }
            set { programPremiereDate = value; OnPropertyChanged(); }
        }

        // Liste af film til dropdown
        public ObservableCollection<Movie> Movies { get; set; }

        // Property til valgt film
        private Movie selectedMovie;
        public Movie SelectedMovie
        {
            get => selectedMovie;
            set
            {
                selectedMovie = value;
                OnPropertyChanged();
                // Opdater ProgramDuration når en film vælges
                ProgramDuration = selectedMovie != null
                    ? selectedMovie.movieLength.Add(TimeSpan.FromMinutes(30))
                    : TimeSpan.Zero;
            }
        }

        private MovieProgram selectedProgram;
        public MovieProgram SelectedProgram
        {
            get { return selectedProgram; }
            set { selectedProgram = value; OnPropertyChanged(); }
        }

        private Cinema cinemaProgram;
        public Cinema CinemaProgram
        {
            get { return cinemaProgram; }
            set { cinemaProgram = value; OnPropertyChanged(); }
        }



        public ICommand OpenPrintWindowCommand { get; }
        public ICommand AddMovieProgramCommand { get; }
        public ICommand UpdateMovieProgramCommand { get; }
        public ICommand RemoveMovieprogramCommand { get; }

        // Kom tilbage til dette:
        private bool CanAddMovieProgram() => SelectedProgram != null && CinemaProgram != null;
        private bool CanUpdateMovieProgram() => SelectedProgram != null;
        private bool CanRemoveMovieprogram() => SelectedProgram != null;

        public MovieProgramViewModel()
        {
            MoviePrograms = new ObservableCollection<MovieProgram>(movieProgramRepository.GetAll());
            MovieProgramCollectionView = CollectionViewSource.GetDefaultView(MoviePrograms);

            PrintCollectionView = CollectionViewSource.GetDefaultView(MoviePrograms);
            PrintCollectionView.Filter = PrintFilter;

            OpenPrintWindowCommand = new RelayCommand(_ => OpenPrintWindow(), _ => true);
            AddMovieProgramCommand = new RelayCommand(_ => AddMovieProgram(), _ => CanAddMovieProgram());
            UpdateMovieProgramCommand = new RelayCommand(_ => UpdateMovieProgram(), _ => CanUpdateMovieProgram());
            RemoveMovieprogramCommand = new RelayCommand(_ => RemoveMovieProgram(), _ => CanRemoveMovieprogram());

            // Fyld listen med film (fx fra repository)
            Movies = new ObservableCollection<Movie>(/* hent film fra repository her */);
        }
        
        private bool PrintFilter(object obj)
        {
            if (obj is MovieProgram movieProgram)
            {
                return movieProgram.Cinema.Name.Equals(SelectedCinemaPrint.ToString(), StringComparison.InvariantCultureIgnoreCase) &&
                       movieProgram.ShowTime.Month.ToString().Equals(SelectedMonth.Month.ToString(), StringComparison.InvariantCultureIgnoreCase);
            }
            return false;
        }

        private void OpenPrintWindow()
        {
            PrintView printView = new PrintView();
            printView.Show();
        }

        private void AddMovieProgram()
        {
            //opret objekt og tilføj til repository og observablecollection
            MovieProgram movieProgram = new MovieProgram(Guid.NewGuid(), ProgramShowTime, ProgramPremiereDate, SelectedMovie, CinemaProgram);
            movieProgramRepository.AddMovieProgram(movieProgram);
            MoviePrograms.Add(movieProgram);

            //vis bekræftelse
            
            MessageBox.Show($"{selectedMovie.title} tilføjet!", "Udført", MessageBoxButton.OK, MessageBoxImage.Information);

            //nulstil felter - Spørg Ilham!!!!!
            ProgramShowTime = DateTime.Now;
            ProgramPremiereDate = DateOnly.FromDateTime(DateTime.Now);
            SelectedMovie = null;
            CinemaProgram = null;
        }

        private void UpdateMovieProgram()
        {
            //opdater movie i repository
            movieProgramRepository.UpdateMovieProgram(SelectedProgram);

            //vis bekræftelse 
            MessageBox.Show($"Ændringerne er gemt", "Udført", MessageBoxButton.OK, MessageBoxImage.Information);

            //nulstil valgt movie
            SelectedProgram = null;

        }

        private void RemoveMovieProgram()
        {

            MessageBoxResult result = MessageBox.Show($"Er du sikker på, at du vil fjerne {SelectedMovie.title}?",
            "Er du enig?", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                MessageBox.Show($"{SelectedMovie.title} er fjernet fra listen.",
                                "Udført", MessageBoxButton.OK, MessageBoxImage.Information);

                //fjern movie i repository 
                movieProgramRepository.RemoveMovieProgram(SelectedProgram);
                MoviePrograms.Remove(SelectedProgram);
            }
            else
            {
                MessageBox.Show($"Filmen blev ikke fjernet fra listen.",
                                "Annulleret", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            SelectedProgram = null;
        }
    }
}
