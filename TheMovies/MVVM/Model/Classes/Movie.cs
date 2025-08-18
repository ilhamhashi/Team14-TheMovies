namespace TheMovies.MVVM.Model.Classes
{
    class Movie
    {
        //ændre gerne til private properties (encapsulation)
        public int id { get; set; }
        public string title { get; set; }
        public string genre { get; set; }
        public TimeSpan movieLength { get; set; }

        public Movie(int id, string title, string genre, TimeSpan movieLength)
        {
            this.id = id;
            this.title = title;
            this.genre = genre;
            this.movieLength = movieLength;
        }

        public override string ToString()
        {
            return $"{id},{title},{genre},{movieLength}";
        }

        public static Movie FromString(string input)
        {
            string[] parts = input.Split(',');
            return new Movie
            (                
                int.Parse(parts[0]),
                parts[1],
                parts[2],
                TimeSpan.Parse(parts[3])
            );
        }
    }
}
