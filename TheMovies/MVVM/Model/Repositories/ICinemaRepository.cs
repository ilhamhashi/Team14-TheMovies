using TheMovies.MVVM.Model.Classes;

namespace TheMovies.MVVM.Model.Repositories
{
    interface ICinemaRepository
    {
        IEnumerable<Cinema> GetAll();
        /*void AddCinema(Cinema cinema);
        void RemoveCinema(Cinema cinema);
        void UpdateCinema(Cinema cinema);*/
    }
}
