
using TheMovies.MVVM.Model.Classes;

namespace TheMovies.MVVM.Model.Repositories
{
    interface IMovieRepository
    {
        IEnumerable<Moviepr> GetAll();
        void AddMovie(Moviepr movie);
        void RemoveMovie(Moviepr movie);
        void UpdateMovie(Moviepr movie);

    }
}
