namespace TheMovies.MVVM.Model.Classes
{
    public class MovieProgram
    {
        public Guid Id { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime PremiereDate { get; set; }
        public Movie Movie { get; set; }
        public CinemaHall Hall { get; set; }

        public MovieProgram(Guid id, TimeSpan duration, DateTime premiereDate, Movie movie, CinemaHall cinemaHall)
        {
            Id = id;
            Duration = movie.movieLength.Add(TimeSpan.FromMinutes(30));
            PremiereDate = premiereDate;
            Movie = movie;
            Hall = cinemaHall;
        }

        public override string ToString()
        {
            return $"{Id},{Duration},{PremiereDate},{Movie},{Hall}";
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
                new CinemaHall (Guid.Parse(parts[8]), parts[9], int.Parse(parts[10]),new Cinema(Guid.Parse(parts[11]), parts[12], parts[13]))
            );
        }
    }
}
