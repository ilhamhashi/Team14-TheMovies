using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheMovies.MVVM.Model.Classes;

namespace TheMovies.MVVM.Model.Repositories
{
    class FileCinemaHallRepository : ICinemaHallRepository
    {
        private readonly string cinemaHallFilePath;

        public FileCinemaHallRepository(string filePath)
        {
            cinemaHallFilePath = filePath;
            if (!File.Exists(cinemaHallFilePath))
            {
                File.AppendAllText(filePath, "Id, Navn, AntalSæder" + Environment.NewLine);
                File.AppendAllText(cinemaHallFilePath, String.Join(Environment.NewLine, demoCinemaHalls() + Environment.NewLine));
            }

        }
        public IEnumerable<CinemaHall> GetAll()
        {
            try
            {
                return File.ReadAllLines(cinemaHallFilePath)
                    .Skip(1)
                    .Where(line => !string.IsNullOrEmpty(line))
                    .Select(CinemaHall.FromString)
                    .ToList();
            }
            catch (IOException ex)
            {

                Console.WriteLine($"Fejl ved læsning af fil: {ex.Message}");
                return [];


            }
        }

        public static List<CinemaHall> demoCinemaHalls()
        {
            List<CinemaHall> demoCinemaHalls = new List<CinemaHall>();

            CinemaHall cinemaHall1 = new CinemaHall(Guid.NewGuid(), "Lille sal", 1);
            CinemaHall cinemaHall2 = new CinemaHall(Guid.NewGuid(), "IMAX", 2);

            demoCinemaHalls.AddRange(cinemaHall1, cinemaHall2);

            return demoCinemaHalls;
        }
    }
}
