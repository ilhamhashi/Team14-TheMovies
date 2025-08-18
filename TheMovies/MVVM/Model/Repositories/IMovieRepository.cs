
using TheMovies.MVVM.Model.Classes;

namespace TheMovies.MVVM.Model.Repositories
{
    interface IMovieRepository
    {
        IEnumerable<Movie> GetAll();
        void AddMovie(Movie movie);
        void RemoveMovie(Movie movie);
        void UpdateMovie(Movie movie);

    }
}
