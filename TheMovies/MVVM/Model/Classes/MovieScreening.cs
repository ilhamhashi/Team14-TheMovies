namespace TheMovies.MVVM.Model.Classes
{
    public class MovieScreening
    {
        public Guid Id { get; set; }
        public DateTime MovieScreeningDateTime { get; set; }
        public Guid MovieProgramId { get; set; }

        public MovieScreening(Guid id, DateTime movieScreeningDateTime, Guid movieProgramId)
        {
            Id = id;
            MovieScreeningDateTime = movieScreeningDateTime;
            MovieProgramId = movieProgramId;
        }

        public override string ToString()
        {
            return $"{Id},{MovieScreeningDateTime},{MovieProgramId}";
        }

        public static MovieScreening FromString(string input)
        {
            string[] parts = input.Split(',');
            return new MovieScreening
            (
                Guid.Parse(parts[0]),
                DateTime.Parse(parts[1]),
                Guid.Parse(parts[2])
            );
        }
    }
}
