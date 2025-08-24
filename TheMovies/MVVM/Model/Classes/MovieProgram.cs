namespace TheMovies.MVVM.Model.Classes
{
    internal class MovieProgram
    {
        public Guid Id { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime ShowTime { get; set; }
        public DateOnly PremiereDate { get; set; }
        public Movie Movie { get; set; }
        public Cinema Cinema { get; set; }

        public MovieProgram(Guid id, DateTime showTime, DateOnly premiereDate, Movie movie, Cinema cinema)
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

        public MovieProgram FromString(string input)
        {
            string[] parts = input.Split(',');
            return new MovieProgram
            (
                Guid.Parse(parts[0]),
                DateTime.Parse(parts[1]),
                DateOnly.Parse(parts[2]),
                new Movie (Guid.Parse(parts[3]), parts[4], parts[5], parts[6], TimeSpan.Parse(parts[7])), /* Behøver kun kan kalde på konstruktør */
                new Cinema (Guid.Parse(parts[8]), parts[9], parts[10], int.Parse(parts[11])) /* Behøver kun kan kalde på konstruktør */
            );
        }
    }
}
