using System.IO;
using TheMovies.MVVM.Model.Classes;

namespace TheMovies.MVVM.Model.Repositories
{
    class FileMovieScreeningRepository : IMovieScreeningRepository
    {
        private readonly string movieScreeningFilePath;

        public FileMovieScreeningRepository(string msFilePath)
        {
            movieScreeningFilePath = msFilePath;

            //Hvis filen ikke eksisterer i forvejen oprettes en ny med kolonnetitler. 
            if (!File.Exists(movieScreeningFilePath))

            {
                File.AppendAllText(movieScreeningFilePath, "ForestillignsId, Forestillingstid" + Environment.NewLine);
                File.AppendAllText(movieScreeningFilePath, String.Join(Environment.NewLine, demoMovieScreenings()) + Environment.NewLine);
            }
        }

        public IEnumerable<MovieScreening> GetAll()
        {
            try
            {
                return File.ReadAllLines(movieScreeningFilePath)
                           .Skip(1)
                           .Where(line => !string.IsNullOrEmpty(line)) // Undgå tomme linjer
                           .Select(MovieScreening.FromString)
                           .ToList();
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Fejl ved læsning af fil: {ex.Message}");
                return [];
            }
        }

        public void AddMovieScreening(MovieScreening movieScreening)
        {
            try
            {
                File.AppendAllText(movieScreeningFilePath, movieScreening.ToString() + Environment.NewLine);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fejl ved skrivning til fil: {ex.Message}");
            }
        }

        public void RemoveMovieScreening(MovieScreening movieScreening)
        {
            List<MovieScreening> movieScreenings = GetAll().ToList();
            movieScreenings.RemoveAll(ms => ms.Id == movieScreening.Id);
            RewriteFile(movieScreenings);
        }

        public void UpdateMovieScreening(MovieScreening movieScreening)
        {
            List<MovieScreening> movieScreenings = GetAll().ToList();
            int index = movieScreenings.FindIndex(ms => ms.Id == movieScreening.Id);
            if (index != -1)
            {
                movieScreenings[index] = movieScreening;
                RewriteFile(movieScreenings);
            }
        }

        private void RewriteFile(List<MovieScreening> movieScreenings)
        {
            try
            {
                List<string> movieScreeningList = movieScreenings.Select(ms => ms.ToString()).ToList();
                movieScreeningList.Insert(0, "ForestillingsId, Forestillingstid");
                File.WriteAllLines(movieScreeningFilePath, movieScreeningList);
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Fejl ved skrivning til fil: {ex.Message}");
            }
        }

        private List<MovieScreening> demoMovieScreenings()
        {
            List<MovieScreening> demoMovieScreenings = new List<MovieScreening>();

            MovieScreening movieScreening1 = new MovieScreening
                (Guid.NewGuid(), DateTime.Now, 50, FileMovieProgramRepository.demoMoviePrograms()[0]);

            demoMovieScreenings.Add(movieScreening1);

            return demoMovieScreenings;
        }
    }
}
