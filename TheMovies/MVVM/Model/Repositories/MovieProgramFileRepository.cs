using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheMovies.MVVM.Model.Classes;

namespace TheMovies.MVVM.Model.Repositories
{
    class MovieProgramFileRepository : IMovieProgramRepository
    {
        private readonly string movieProgramFilePath;

        public MovieProgramFileRepository(string mpFilePath)
        {
            movieProgramFilePath = mpFilePath;

            //Hvis filen ikke eksisterer i forvejen oprettes en ny med kolonnetitler. 
            if (!File.Exists(movieProgramFilePath))
            {
                File.AppendAllText(movieProgramFilePath, "FilmprogramID, Spilletid, Forestillingstid, Premieredato, Film, Biograf" + Environment.NewLine);
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

        // Tilføjer et filmprogram til filen. Skal også kunne tilføje 15x2 min på begge sider af spilletiden. 
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
                List<string> moviePrgramsList = moviePrograms.Select(mp => mp.ToString()).ToList();
                moviePrgramsList.Insert(0, "FilmprogramID, Spilletid, Forestillingstid, Premieredato, Film, Biograf");
                File.WriteAllLines(movieProgramFilePath, moviePrgramsList);
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Fejl ved skrivning til fil: {ex.Message}");
            }
        }
    }
}
