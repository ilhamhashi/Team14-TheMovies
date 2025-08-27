using System.IO;
using TheMovies.MVVM.Model.Classes;

namespace TheMovies.MVVM.Model.Repositories
{
    class FileMovieProgramRepository : IMovieProgramRepository
    {
        private readonly string movieProgramFilePath;

        public FileMovieProgramRepository(string mpFilePath)
        {
            movieProgramFilePath = mpFilePath;

            //Hvis filen ikke eksisterer i forvejen oprettes en ny med kolonnetitler. 
            if (!File.Exists(movieProgramFilePath))

            {
                File.AppendAllText(movieProgramFilePath, "FilmprogramID, Spilletid, Forestillingstid, Premieredato, Film, Biograf" + Environment.NewLine);
                File.AppendAllText(movieProgramFilePath, String.Join(Environment.NewLine, demoMoviePrograms()) + Environment.NewLine);
            }
        }

        public IEnumerable<MovieProgram> GetAll()
        {
            try
            {
                return File.ReadAllLines(movieProgramFilePath)
                           .Skip(1)
                           .Where(line => !string.IsNullOrEmpty(line)) // Undgå tomme linjer
                           .Select(MovieProgram.FromString)
                           .ToList();
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Fejl ved læsning af fil: {ex.Message}");
                return [];
            }
        }

        public void AddMovieProgram(MovieProgram movieProgram)
        {
            try
            {
                File.AppendAllText(movieProgramFilePath, movieProgram.ToString() + Environment.NewLine);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fejl ved skrivning til fil: {ex.Message}");
            }
        }

        public void RemoveMovieProgram(MovieProgram movieProgram)
        {
            List<MovieProgram> moviePrograms = GetAll().ToList();
            moviePrograms.RemoveAll(mp => mp.Id == movieProgram.Id);
            RewriteFile(moviePrograms);
        }

        public void UpdateMovieProgram(MovieProgram movieProgram)
        {
            List<MovieProgram> moviePrograms = GetAll().ToList();
            int index = moviePrograms.FindIndex(mp => mp.Id == movieProgram.Id);
            if (index != -1)
            {
                moviePrograms[index] = movieProgram;
                RewriteFile(moviePrograms);
            }
        }

        private void RewriteFile(List<MovieProgram> moviePrograms)
        {
            try
            {
                List<string> movieProgramList = moviePrograms.Select(mp => mp.ToString()).ToList();
                movieProgramList.Insert(0, "FilmprogramID, Spilletid, Forestillingstid, Premieredato, Film, Biograf");
                File.WriteAllLines(movieProgramFilePath, movieProgramList);
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Fejl ved skrivning til fil: {ex.Message}");
            }
        }

        public static List<MovieProgram> demoMoviePrograms()
        {
            List<MovieProgram> demoMoviePrograms = new List<MovieProgram>();

            MovieProgram movieProgram1 = new MovieProgram (
                Guid.NewGuid(), 
                TimeSpan.FromHours(2), 
                DateTime.Now, 
                FileMovieRepository.demoMovies()[0], 
                FileCinemaHallRepository.demoCinemaHalls()[2]
                );

            demoMoviePrograms.Add(movieProgram1);

            return demoMoviePrograms;
        }
    }
}

