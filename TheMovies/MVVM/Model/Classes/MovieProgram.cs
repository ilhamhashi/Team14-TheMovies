namespace TheMovies.MVVM.Model.Classes
{
    public class MovieProgram
    {
        public Guid Id { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime PremiereDate { get; set; }
        public Movie Movie { get; set; }
        public Cinema Cinema { get; set; }

        public MovieProgram(Guid id, TimeSpan duration, DateTime premiereDate, Movie movie, Cinema cinema)
        {
            Id = id;
            Duration = movie.movieLength.Add(TimeSpan.FromMinutes(30));
            PremiereDate = premiereDate;
            Movie = movie;
            Cinema = cinema;
        }

        public override string ToString()
        {
            return $"{Id},{Duration},{PremiereDate},{Movie},{Cinema}";
        }

        public static MovieProgram FromString(string input)
        {
            string[] parts = input.Split(',');
            return new MovieProgram
            (
                Guid.Parse(parts[0]),
                TimeSpan.Parse(parts[1]),
                DateTime.Parse(parts[2]),
                new Movie (Guid.Parse(parts[3]), parts[4], parts[5], parts[6], TimeSpan.Parse(parts[7])),
                new Cinema (Guid.Parse(parts[8]), parts[9], parts[10], int.Parse(parts[11]))
            );
        }
    }
}
