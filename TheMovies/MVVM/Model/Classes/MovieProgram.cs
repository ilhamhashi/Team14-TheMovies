using System.Windows;

namespace TheMovies.MVVM.Model.Classes
{
    internal class MovieProgram
    {
        public Guid Id { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime ShowTime { get; set; }
        public DateTime PremiereDate { get; set; }
        public Movie Movie { get; set; }
        public Cinema Cinema { get; set; }

        public MovieProgram(Guid id, TimeSpan duration, DateTime showTime, DateTime premiereDate, Movie movie, Cinema cinema)
        {
            Id = id;
            Duration = movie.movieLength.Add(TimeSpan.FromMinutes(30));
            ShowTime = showTime;
            PremiereDate = premiereDate;
            Movie = movie;
            Cinema = cinema;
        }

        public override string ToString()
        {
            return $"{Id},{Duration},{ShowTime},{PremiereDate},{Movie},{Cinema}";
        }

        public static MovieProgram FromString(string input)
        {
            string[] parts = input.Split(',');
            return new MovieProgram
            (
                Guid.Parse(parts[0]),
                TimeSpan.Parse(parts[1]),
                DateTime.Parse(parts[2]),
                DateTime.Parse(parts[3]),
                new Movie (Guid.Parse(parts[4]), parts[5], parts[6], parts[7], TimeSpan.Parse(parts[8])), /* Behøver kun kan kalde på konstruktør */
                new Cinema (Guid.Parse(parts[9]), parts[10], parts[11], int.Parse(parts[12])) /* Behøver kun kan kalde på konstruktør */
            );
        }
    }
}
