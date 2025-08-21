using System.IO;
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
                File.WriteAllText(filePath, "BiografID, Navn, By, Sal-antal\n");
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
    }
}


