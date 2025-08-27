using TheMovies.MVVM.Model.Classes;

namespace TheMovies.MVVM.Model.Repositories
{
    internal interface IMovieScreeningRepository
    {
        IEnumerable<MovieScreening> GetAll();
        void AddMovieScreening(MovieScreening movieScreening);
        void RemoveMovieScreening(MovieScreening movieScreening);
        void UpdateMovieScreening(MovieScreening movieScreening);
    }
}
