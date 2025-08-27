using TheMovies.MVVM.ViewModel;

namespace TheMovies.Tests
{
    [TestClass]
    public sealed class FileMovieRepositoryAndListTests
    {
        [TestMethod]
        public void AddMovieCommand_ShouldAddMovieToRepositoryAndList()
        {
            // Arrange: Opretter en instans af MainWindowViewModel og tildeler properties værdier
            var viewModel = new MainWindowViewModel();
            viewModel.MovieTitle = "MovieTitle";
            viewModel.MovieDirector = "MovieDirector";
            viewModel.MovieGenre = "MovieGenre";
            viewModel.MovieLength = TimeSpan.FromHours(1,50);

            // Act: Kalder AddMovieCommand fra MainWindowViewModel
            viewModel.AddMovieCommand.CanExecute(null);
            viewModel.AddMovieCommand.Execute(null);

            // Assert: Tester angivne værdier i FileMovieRepository og ObservableCollection Movies fra MainWindowViewModel
            Assert.AreEqual(5, viewModel.movieRepository.GetAll().ToList().Count);
            Assert.AreEqual(5, viewModel.Movies.Count);
            Assert.AreEqual("MovieTitle", viewModel.movieRepository.GetAll().ToList()[3].title);
            Assert.AreEqual("MovieDirector", viewModel.movieRepository.GetAll().ToList()[3].director);
            Assert.AreEqual("MovieGenre", viewModel.movieRepository.GetAll().ToList()[3].genre);
            Assert.AreEqual(TimeSpan.FromHours(1,50), viewModel.movieRepository.GetAll().ToList()[3].movieLength);
        }

        public void UpdateMovieCommand_ShouldUpdateMovieInRepositoryAndList()
        {
            // Arrange: Opretter en instans af MainWindowViewModel og tildeler properties værdier

            // Act: Kalder UpdateMovieCommand fra MainWindowViewModel

            // Assert: Tester angivne værdier i FileMovieRepository og ObservableCollection Movies fra MainWindowViewModel
        }

        public void RemoveMovieCommand_ShouldRemoveMovieInRepositoryAndList()
        {
            // Arrange: Opretter en instans af MainWindowViewModel og tildeler properties værdier

            // Act: Kalder RemoveMovieCommand fra MainWindowViewModel

            // Assert: Tester angivne værdier i FileMovieRepository og ObservableCollection Movies fra MainWindowViewModel
        }

    }
}
