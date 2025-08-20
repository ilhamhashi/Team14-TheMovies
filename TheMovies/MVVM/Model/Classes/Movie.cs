namespace TheMovies.MVVM.Model.Classes
{
    class Movie
    {
        //ændre gerne til private properties (encapsulation)
        public Guid id { get; set; }
        public string title { get; set; }
        public string director { get; set; }
        public string genre { get; set; }
        public TimeSpan movieLength { get; set; }

        public Movie(Guid id, string title, string director, string genre, TimeSpan movieLength)
        {
            this.id = id;
            this.title = title;
            this.director = director;
            this.genre = genre;
            this.movieLength = movieLength;
        }

        public override string ToString()
        {
            return $"{id},{title},{director},{genre},{movieLength}";
        }

        public static Movie FromString(string input)
        {
            string[] parts = input.Split(',');
            return new Movie
            (                
                Guid.Parse(parts[0]),
                parts[1],
                parts[2],
                parts[3],
                TimeSpan.Parse(parts[4])
            );
        }
    }
}
