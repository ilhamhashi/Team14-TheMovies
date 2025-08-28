namespace TheMovies.MVVM.Model.Classes
{
    public class MovieScreening
    {
        public Guid Id { get; set; }
        public DateTime MovieScreeningDateTime { get; set; }
        public Guid MovieProgramId { get; set; }
        public int AvailableTickets { get; set; }
        public CinemaHall Hall { get; set; }

        public MovieScreening(Guid id, DateTime movieScreeningDateTime, Guid movieProgramId, int availableTickets, CinemaHall hall)
        {
            Id = id;
            MovieScreeningDateTime = movieScreeningDateTime;
            MovieProgramId = movieProgramId;
            AvailableTickets = availableTickets;
            Hall = hall;
        }

        public override string ToString()
        {
            return $"{Id},{MovieScreeningDateTime},{MovieProgramId}, {AvailableTickets}";
        }

        public static MovieScreening FromString(string input)
        {
            string[] parts = input.Split(',');
            return new MovieScreening
            (
                Guid.Parse(parts[0]),
                DateTime.Parse(parts[1]),
                Guid.Parse(parts[2]),
                int.Parse(parts[3]),
                new CinemaHall(Guid.Parse(parts[4]), parts[5], int.Parse(parts[6]), new Cinema(Guid.Parse(parts[7]), parts[8], parts[9]))
            );
        }
    }
}
