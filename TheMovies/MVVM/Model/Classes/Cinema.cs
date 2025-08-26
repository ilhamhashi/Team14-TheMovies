using System.IO;

namespace TheMovies.MVVM.Model.Classes
{
    public class Cinema
    {
        public Guid Id {  get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public int CinemaScreenCount { get; set; }

        public Cinema(Guid id, string name, string city, int cinemaScreenCount)
        {
            Id = id;
            Name = name;
            City = city;
            CinemaScreenCount = cinemaScreenCount;
        }

        public override string ToString()
        {
            return $"{Id},{Name},{City},{CinemaScreenCount}";
        }

        public static Cinema FromString(string input)
        {
            string[] parts = input.Split(',');
            return new Cinema
            (
                Guid.Parse(parts[0]),
                parts[1],
                parts[2],
                int.Parse(parts[3])
            );
        }
    }
}
