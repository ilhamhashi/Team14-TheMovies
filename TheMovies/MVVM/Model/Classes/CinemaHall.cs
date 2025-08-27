namespace TheMovies.MVVM.Model.Classes
{
    public class CinemaHall
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int SeatCount { get; set; }
        public Cinema Cinema { get; set; } 

        public CinemaHall(Guid id, string name, int seatCount, Cinema cinema)
        {
            Id = id;
            Name = name;
            SeatCount = seatCount;
            Cinema = cinema;
        }
        public override string ToString()
        {
            return $"{Id},{Name},{SeatCount},{Cinema}";
        }
        public static CinemaHall FromString(string input)
        {
            string[] parts = input.Split(',');
            return new CinemaHall
            (
                Guid.Parse(parts[0]),
                parts[1],
                int.Parse(parts[2]),
                new Cinema(Guid.Parse(parts[3]), parts[4], parts[5])
             );
        }
    }

    
    }
