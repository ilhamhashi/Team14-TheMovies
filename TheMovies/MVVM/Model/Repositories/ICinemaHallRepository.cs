using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheMovies.MVVM.Model.Classes;

namespace TheMovies.MVVM.Model.Repositories
{
    interface ICinemaHallRepository
    {
        IEnumerable<CinemaHall> GetAll();
    }
}
