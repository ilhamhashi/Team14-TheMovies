using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheMovies.MVVM.Model.Classes
{
    public class CinemaHall
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int SeatCount { get; set; }

        public CinemaHall(Guid id, string name, int seatCount)
        {
            Id = id;
            Name = name;
            SeatCount = seatCount;
        }
        public override string ToString()
        {
            return $"{Id},{Name},{SeatCount}";
        }
        public static CinemaHall FromString(string input)
        {
            string[] parts = input.Split(',');
            return new CinemaHall
            (
                Guid.Parse(parts[0]),
                parts[1],
                int.Parse(parts[2])
             );
        }
    }

    
    }
