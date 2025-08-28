namespace TheMovies.MVVM.Model.Classes
{
    public class MovieScreening
    {
        public Guid Id { get; set; }
        public DateTime MovieScreeningDateTime { get; set; }
        public int AvailableTickets { get; set; }
        public MovieProgram MovieProgram { get; set; }

        public MovieScreening(Guid id, DateTime movieScreeningDateTime, int availableTickets, MovieProgram movieProgram)
        {
            Id = id;
            MovieScreeningDateTime = movieScreeningDateTime;
            AvailableTickets = availableTickets;
            MovieProgram = movieProgram;
        }

        public override string ToString()
        {
            return $"{Id},{MovieScreeningDateTime},{AvailableTickets},{MovieProgram}";
        }

        public static MovieScreening FromString(string input)
        {
            string[] parts = input.Split(',');
            return new MovieScreening
            (
                Guid.Parse(parts[0]),
                DateTime.Parse(parts[1]),
                int.Parse(parts[2]),
                new MovieProgram(Guid.Parse(parts[3]), TimeSpan.Parse(parts[4]), DateTime.Parse(parts[5]), new Movie(Guid.Parse(parts[6]), parts[7], parts[8], parts[9], TimeSpan.Parse(parts[10])), new CinemaHall(Guid.Parse(parts[11]), parts[12], int.Parse(parts[13]), new Cinema(Guid.Parse(parts[14]), parts[15], parts[16])))
            );
        }
    }
}
