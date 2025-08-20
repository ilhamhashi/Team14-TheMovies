using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheMovies.MVVM.Model.Classes
{
    internal class MovieProgram
    {
        public TimeSpan Duration { get; set; }
        public DateTime ShowTime { get; set; }
        public DateOnly PremiereDate { get; set; }
        public Movie Movie { get; set; }
        public Cinema Cinema { get; set; }

        public MovieProgram(TimeSpan duration, DateTime showTime, DateOnly premiereDate, Movie movie, Cinema cinema)
        {
            Duration = duration;
            ShowTime = showTime;
            PremiereDate = premiereDate;
            Movie = movie;
            Cinema = cinema;
        }

        public override string ToString()
        {
            return $"{Duration},{ShowTime},{PremiereDate},{Cinema}, {Movie}";
        }

        public static MovieProgram FromString(string input)
        {
            string[] parts = input.Split(',');
            return new MovieProgram
            (
                TimeSpan.Parse(parts[0]),
                DateTime.Parse(parts[1]),
                DateOnly.Parse(parts[2]),
                Movie.Parse(parts[3])
            );
        }
    }
}
