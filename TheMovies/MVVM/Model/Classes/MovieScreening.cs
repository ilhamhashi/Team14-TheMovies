namespace TheMovies.MVVM.Model.Classes
{
    public class MovieScreening
    {
        public Guid Id { get; set; }
        public DateTime MovieScreeningDateTime { get; set; }
        public Guid MovieProgramId { get; set; }
        public int AvailableTickets { get; set; }

        public MovieScreening(Guid id, DateTime movieScreeningDateTime, Guid movieProgramId, int availableTickets)
        {
            Id = id;
            MovieScreeningDateTime = movieScreeningDateTime;
            MovieProgramId = movieProgramId;
            AvailableTickets = availableTickets; // = CinemaHall.SeatCount?
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
                int.Parse(parts[3])
            );
        }
    }
}
