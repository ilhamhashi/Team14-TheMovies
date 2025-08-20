
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using TheMovies.MVVM.Model.Classes;
using TheMovies.MVVM.Model.Repositories;

namespace TheMovies.MVVM.ViewModel
{
    internal class MainWindowViewModel : ViewModelBase
    {
        private readonly FileMovieRepository movieRepository = new FileMovieRepository("movies.csv");

		public ObservableCollection<Movie> Movies;
        public static ICollectionView MoviesCollectionView { get; set; }

        private Guid movieId;
		public Guid MovieId
		{
			get { return movieId; }
			set { movieId = value; OnPropertyChanged(); }
		}

		private string	movieTitle;
		public string MovieTitle
		{
			get { return movieTitle; }
			set { movieTitle = value; OnPropertyChanged(); }
		}

		private string movieGenre;
		public string MovieGenre
		{
			get { return movieGenre; }
			set { movieGenre = value; OnPropertyChanged(); }
		}

		private TimeSpan movieLength;
        public TimeSpan MovieLength
		{
			get { return movieLength; }
			set { movieLength = value; OnPropertyChanged(); }
		}

		private Movie selectedMovie;
		public Movie SelectedMovie
		{
			get { return selectedMovie; }
			set { selectedMovie = value; OnPropertyChanged(); }
		}

        private string searchTerm = string.Empty;
        public string SearchTerm
        {
            get { return searchTerm; }
            set
            {
                searchTerm = value;
                OnPropertyChanged(nameof(MoviesFilter));
                MoviesCollectionView.Refresh();
            }
        }


        public ICommand AddMovieCommand { get; }
        public ICommand UpdateMovieCommand { get; }
        public ICommand RemoveMovieCommand { get; }

        private bool CanAddMovie() => !string.IsNullOrWhiteSpace(MovieTitle) && !string.IsNullOrWhiteSpace(MovieGenre) && MovieLength != null;
        private bool CanUpdateMovie() => SelectedMovie != null;
        private bool CanRemoveMovie() => SelectedMovie != null;

        public MainWindowViewModel()
        {
            /* Movie movie1 = new Movie(MovieId, "1917", "Drama", TimeSpan.FromHours(2));
			movieRepository.AddMovie(movie1);
            Movie movie2 = new Movie(MovieId, "TEST!", "Thriller", TimeSpan.FromHours(1,45));
			movieRepository.AddMovie(movie2); */
			
            Movies = new ObservableCollection<Movie>(movieRepository.GetAll());
            MoviesCollectionView = CollectionViewSource.GetDefaultView(Movies);
            MoviesCollectionView.Filter = MoviesFilter;


            AddMovieCommand = new RelayCommand(_ => AddMovie(), _ => CanAddMovie());
            UpdateMovieCommand = new RelayCommand(_ => UpdateMovie(), _ => CanUpdateMovie());
            RemoveMovieCommand = new RelayCommand(_ => RemoveMovie(), _ => CanRemoveMovie());
        }

		void AddMovie()
		{
			//opret objekt og tilføj til repository og observablecollection
			Movie movie = new Movie(Guid.NewGuid(), MovieTitle, MovieGenre, MovieLength);
			movieRepository.AddMovie(movie);
			Movies.Add(movie);

			//vis bekræftelse
            MessageBox.Show($"{movieTitle} tilføjet!", "Udført", MessageBoxButton.OK, MessageBoxImage.Information);

			//nulstil felter
			MovieTitle = string.Empty;
			MovieGenre = string.Empty;
			MovieLength = TimeSpan.Zero;
        }

        void UpdateMovie()
        {
			//opdater movie i repository
            movieRepository.UpdateMovie(SelectedMovie);

			//vis bekræftelse 
            MessageBox.Show($"Ændringerne er gemt", "Udført", MessageBoxButton.OK, MessageBoxImage.Information);

			//nulstil valgt movie
            SelectedMovie = null;

        }

        void RemoveMovie()
        {
			//fjern moviw i repository 
			movieRepository.RemoveMovie(SelectedMovie);
            Movies.Remove(SelectedMovie);

            //vis bekræftelse 
            MessageBox.Show($"Ændringerne er gemt", "Udført", MessageBoxButton.OK, MessageBoxImage.Information);

            //nulstil valgt movie
            SelectedMovie = null;
        }

        private bool MoviesFilter(object obj)
        {
            if (obj is Movie movie)
            {
                return movie.title.Contains(SearchTerm, StringComparison.InvariantCultureIgnoreCase) ||
                    movie.genre.Contains(SearchTerm, StringComparison.InvariantCultureIgnoreCase) || 
					movie.movieLength.ToString().Contains(SearchTerm, StringComparison.InvariantCultureIgnoreCase);
            }
            return false;
        }

    }
}
