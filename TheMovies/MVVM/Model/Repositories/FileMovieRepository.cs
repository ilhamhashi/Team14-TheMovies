using System.IO;
using TheMovies.MVVM.Model.Classes;

namespace TheMovies.MVVM.Model.Repositories
{
    public class FileMovieRepository : IMovieRepository
    {
        private readonly string movieFilePath;

        public FileMovieRepository(string filePath)
        {
            movieFilePath = filePath;

            //Hvis filen ikke eksisterer i forvejen oprettes en ny med kolonnetitler. 
            if (!File.Exists(movieFilePath))
            {
                File.AppendAllText(movieFilePath, "FilmId, Filmtitel, Filminstruktør, Filmgenre, Filmvarighed" + Environment.NewLine);
                File.AppendAllText(movieFilePath, String.Join(Environment.NewLine, demoMovies()) + Environment.NewLine);

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
                List<string> moviesList = movies.Select(m => m.ToString()).ToList();
                moviesList.Insert(0, "FilmId, Filmtitel, Filminstruktør, Filmgenre, Filmvarighed");
                File.WriteAllLines(movieFilePath, moviesList);
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Fejl ved skrivning til fil: {ex.Message}");
            }
        }

        public static List<Movie> demoMovies()
        {
            List<Movie> demoMovies = new List<Movie>();

            Movie movie1 = new Movie(Guid.NewGuid(), "1917", "Sam Mendes", "Drama Thriller War", TimeSpan.FromHours(1,57));
            Movie movie2 = new Movie(Guid.NewGuid(), "The Wife", "Björn Runge", "Drama", TimeSpan.FromHours(1,39));
            Movie movie3 = new Movie(Guid.NewGuid(), "Ayka", "Sergei Dvortsevoy", "Drama", TimeSpan.FromHours(1,40));
            
            demoMovies.AddRange(movie1, movie2, movie3);
            return demoMovies;
        }
    }
}
