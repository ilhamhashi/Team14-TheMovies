using TheMovies.MVVM.Model.Classes;

namespace TheMovies.MVVM.Model.Repositories
{
    interface ICinemaRepository
    {
        IEnumerable<Cinema> GetAll();
    }
}
