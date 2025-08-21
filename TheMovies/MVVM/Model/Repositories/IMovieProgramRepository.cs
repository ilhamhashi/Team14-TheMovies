using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheMovies.MVVM.Model.Classes;

namespace TheMovies.MVVM.Model.Repositories
{
    interface IMovieProgramRepository
    {
        IEnumerable<MovieProgram> GetAll();
        void AddMovieProgram(MovieProgram movieProgram);
        //void RemoveMovieProgram(MovieProgram movieProgram);
        //void UpdateMovieProgram(MovieProgram movieProgram);
    }
}
