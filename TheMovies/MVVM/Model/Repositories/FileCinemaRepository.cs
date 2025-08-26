using System.IO;
using System.Windows.Input;
using TheMovies.MVVM.Model.Classes;

namespace TheMovies.MVVM.Model.Repositories
{
    internal class FileCinemaRepository : ICinemaRepository
    {
        private readonly string cinemaFilePath;

        public FileCinemaRepository(string filePath)
        {
            cinemaFilePath = filePath;
            if (!File.Exists(cinemaFilePath))
            {
                File.AppendAllText(filePath, "BiografId, Navn, By, Sal-antal" + Environment.NewLine);
                File.AppendAllText(cinemaFilePath, String.Join(Environment.NewLine, demoCinemas()) + Environment.NewLine);
            }

        }
        public IEnumerable<Cinema> GetAll()
        {
            try
            {
                return File.ReadAllLines(cinemaFilePath)
                    .Skip(1)
                    .Where(line => !string.IsNullOrEmpty(line))
                    .Select(Cinema.FromString)
                    .ToList();
            }
            catch (IOException ex)
            {

                Console.WriteLine($"Fejl ved læsning af fil: {ex.Message}");
                return [];


            }
        }
        private List<Cinema> demoCinemas()
        {
            List<Cinema> demoCinemas = new List<Cinema>();

            Cinema cinema1 = new Cinema(Guid.NewGuid(), "CinemaxX", "Århus", 1);
            Cinema cinema2 = new Cinema(Guid.NewGuid(), "CinemaxX", "Odense", 1);
            Cinema cinema3 = new Cinema(Guid.NewGuid(), "Nordisk Film", "Århus", 1);
            Cinema cinema4 = new Cinema(Guid.NewGuid(), "Nordisk Film", "Odense", 1);

            demoCinemas.AddRange(cinema1, cinema2, cinema3, cinema4);

            return demoCinemas;
        }
    }
}


