using System.IO;
using TheMovies.MVVM.Model.Classes;

namespace TheMovies.MVVM.Model.Repositories
{
    class FileMovieRepository : IMovieRepository
    {
        private readonly string movieFilePath;
        private int movieIdCounter = 1;

        public FileMovieRepository(string filePath)
        {
            movieFilePath = filePath;

            //Hvis filen ikke eksisterer i forvejen oprettes en ny med kolonnetitler. 
            if (!File.Exists(movieFilePath))
            {
                File.AppendAllText(movieFilePath, "Filmtitel,Filmgenre,Filmvarighed" + Environment.NewLine);
            }
        }       

        public IEnumerable<Movie> GetAll()
        {
            try
            {
                return File.ReadAllLines(movieFilePath)
                           .Skip(1)
                           .Where(line => !string.IsNullOrEmpty(line)) // Undgå tomme linjer
                           .Select(Movie.FromString)
                           .ToList();
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Fejl ved læsning af fil: {ex.Message}");
                return [];
            }
        }

        public void AddMovie(Movie movie)
        {
            try
            {
                movie.id = movieIdCounter++;
                File.AppendAllText(movieFilePath, movie.ToString() + Environment.NewLine);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fejl ved skrivning til fil: {ex.Message}");
            }
        }

        public void RemoveMovie(Movie movie)
        {
            List<Movie> movies = GetAll().ToList();
            movies.RemoveAll(m => m.id  == movie.id);
            RewriteFile(movies);
        }

        public void UpdateMovie(Movie movie)
        {
            List<Movie> movies = GetAll().ToList();
            int index = movies.FindIndex(m => m.id == movie.id);
            if (index != -1)
            {
                movies[index] = movie;
                RewriteFile(movies);
            }
        }

        private void RewriteFile(List<Movie> movies)
        {
            try
            {
                File.WriteAllLines(movieFilePath, movies.Select(m => m.ToString()));
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Fejl ved skrivning til fil: {ex.Message}");
            }
        }
    }
}
