
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
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly FileMovieRepository movieRepository = new FileMovieRepository("movies.csv");

		public ObservableCollection<Movie> Movies;
        public static ICollectionView ?MoviesCollectionView { get; set; }

		private string	movieTitle;
		public string MovieTitle
		{
			get { return movieTitle; }
			set { movieTitle = value; OnPropertyChanged(); }
		}

        private string movieDirector;
        public string MovieDirector
        {
            get { return movieDirector; }
            set { movieDirector = value; }
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
        public ICommand OpenWindowCommand { get; }


        private bool CanAddMovie() => !string.IsNullOrWhiteSpace(MovieTitle) && !string.IsNullOrWhiteSpace(MovieGenre) &&
                                      !string.IsNullOrWhiteSpace(MovieDirector) && MovieLength != TimeSpan.Zero;
        private bool CanUpdateMovie() => SelectedMovie != null;
        private bool CanRemoveMovie() => SelectedMovie != null;

        public MainWindowViewModel()
        {
            Movies = new ObservableCollection<Movie>(movieRepository.GetAll());
            MoviesCollectionView = CollectionViewSource.GetDefaultView(Movies);
            MoviesCollectionView.Filter = MoviesFilter;

            OpenWindowCommand = new RelayCommand(_ => OpenWindow1(), _ => true);
            AddMovieCommand = new RelayCommand(_ => AddMovie(), _ => CanAddMovie());
            UpdateMovieCommand = new RelayCommand(_ => UpdateMovie(), _ => CanUpdateMovie());
            RemoveMovieCommand = new RelayCommand(_ => RemoveMovie(), _ => CanRemoveMovie());
        }

		private void AddMovie()
		{
			//opret objekt og tilføj til repository og observablecollection
			Movie movie = new Movie(Guid.NewGuid(), MovieTitle, MovieDirector, MovieGenre, MovieLength);
			movieRepository.AddMovie(movie);
			Movies.Add(movie);

			//vis bekræftelse
            MessageBox.Show($"{movieTitle} tilføjet!", "Udført", MessageBoxButton.OK, MessageBoxImage.Information);

			//nulstil felter
			MovieTitle = string.Empty;
            MovieDirector = string.Empty;
			MovieGenre = string.Empty;
			MovieLength = TimeSpan.Zero;
        }

        private void UpdateMovie()
        {
			//opdater movie i repository
            movieRepository.UpdateMovie(SelectedMovie);

			//vis bekræftelse 
            MessageBox.Show($"Ændringerne er gemt", "Udført", MessageBoxButton.OK, MessageBoxImage.Information);

			//nulstil valgt movie
            SelectedMovie = null;

        }

        private void RemoveMovie()
        {

            MessageBoxResult result = MessageBox.Show($"Er du sikker på, at du vil fjerne {SelectedMovie.title}?",
            "Er du enig?", MessageBoxButton.YesNo, MessageBoxImage.Question);

			if (result == MessageBoxResult.Yes)
			{
                MessageBox.Show($"{SelectedMovie.title} er fjernet fra listen.",
                                "Udført", MessageBoxButton.OK, MessageBoxImage.Information);

                //fjern movie i repository 
                movieRepository.RemoveMovie(SelectedMovie);
                Movies.Remove(SelectedMovie);
            }
			else
			{
                MessageBox.Show($"Filmen blev ikke fjernet fra listen.",
								"Annulleret", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            SelectedMovie = null;
        }

        private bool MoviesFilter(object obj)
        {
            if (obj is Movie movie)
            {
                return movie.title.Contains(SearchTerm, StringComparison.InvariantCultureIgnoreCase) ||
                       movie.director.Contains(SearchTerm, StringComparison.InvariantCultureIgnoreCase) ||
                       movie.genre.Contains(SearchTerm, StringComparison.InvariantCultureIgnoreCase); 
            }
            return false;
        }

        private void OpenWindow1()
        {
            MovieProgramView view = new MovieProgramView();
            view.Show();
        }

    }
}
