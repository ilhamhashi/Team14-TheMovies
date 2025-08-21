namespace TheMovies.MVVM.Model.Classes
{
    internal class MovieProgram
    {
        public Guid Id { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime ShowTime { get; set; }
        public DateOnly PremiereDate { get; set; }
        public Moviepr Movie { get; set; }
        public Cinema Cinema { get; set; }

        public MovieProgram(Guid id, TimeSpan duration, DateTime showTime, DateOnly premiereDate, Moviepr movie, Cinema cinema)
        {
            Id = id;
            Duration = duration;
            ShowTime = showTime;
            PremiereDate = premiereDate;
            Movie = movie;
            Cinema = cinema;
        }

        public override string ToString()
        {
            return $"{Id},{Duration},{ShowTime},{PremiereDate},{Movie},{Cinema}";
        }

        // Ændret til en static metode for at kunne kalde den direkte uden en instans
        public static MovieProgram FromString(string input) 
        {
            string[] parts = input.Split(',');
            return new MovieProgram
            (
                Guid.Parse(parts[0]),
                TimeSpan.Parse(parts[1]),
                DateTime.Parse(parts[2]),
                DateOnly.Parse(parts[3]),
                new Moviepr (Guid.Parse(parts[4]), parts[5], parts[6], parts[7], TimeSpan.Parse(parts[8])), /* Behøver kun kan kalde på konstruktør */
                new Cinema (Guid.Parse(parts[9]), parts[10], parts[11], int.Parse(parts[12])) /* Behøver kun kan kalde på konstruktør */
            );
        }
    }
}
