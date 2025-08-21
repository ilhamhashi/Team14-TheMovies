using System.IO;
using TheMovies.MVVM.Model.Classes;

namespace TheMovies.MVVM.Model.Repositories
{
    class FileMovieRepository : IMovieRepository
    {
        private readonly string movieFilePath;

        public FileMovieRepository(string filePath)
        {
            movieFilePath = filePath;

            //Hvis filen ikke eksisterer i forvejen oprettes en ny med kolonnetitler. 
            if (!File.Exists(movieFilePath))
            {
                File.AppendAllText(movieFilePath, "FilmID, Filmtitel,Filmgenre,Filmvarighed" + Environment.NewLine);
            }
        }       

        public IEnumerable<Moviepr> GetAll()
        {
            try
            {
                return File.ReadAllLines(movieFilePath)
                           .Skip(1)
                           .Where(line => !string.IsNullOrEmpty(line)) // Undgå tomme linjer
                           .Select(Moviepr.FromString)
                           .ToList();
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Fejl ved læsning af fil: {ex.Message}");
                return [];
            }
        }

        public void AddMovie(Moviepr movie)
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

        public void RemoveMovie(Moviepr movie)
        {
            List<Moviepr> movies = GetAll().ToList();
            movies.RemoveAll(m => m.id  == movie.id);
            RewriteFile(movies);
        }

        public void UpdateMovie(Moviepr movie)
        {
            List<Moviepr> movies = GetAll().ToList();
            int index = movies.FindIndex(m => m.id == movie.id);
            if (index != -1)
            {
                movies[index] = movie;
                RewriteFile(movies);
            }
        }

        private void RewriteFile(List<Moviepr> movies)
        {
            try
            {
                List<string> moviesList = movies.Select(m => m.ToString()).ToList();
                moviesList.Insert(0, "Filmnummer, Filmtitel, Filmgenre, Filmvarighed");
                File.WriteAllLines(movieFilePath, moviesList);
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Fejl ved skrivning til fil: {ex.Message}");
            }
        }
    }
}
