using TheMovies.MVVM.Model.Classes;

namespace TheMovies.MVVM.Model.Repositories
{
    interface IMovieProgramRepository
    {
        IEnumerable<MovieProgram> GetAll();
        void AddMovieProgram(MovieProgram movieProgram);
        void RemoveMovieProgram(MovieProgram movieProgram);
        void UpdateMovieProgram(MovieProgram movieProgram);
    }
}
