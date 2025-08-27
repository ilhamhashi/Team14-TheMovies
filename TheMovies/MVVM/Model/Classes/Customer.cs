using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheMovies.MVVM.Model.Classes
{
    class Customer
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int PhoneNumber { get; set; }

        public Customer(Guid id, string name, string email, int phoneNumber)
        {
            Id = id;
            Name = name;
            Email = email;
            PhoneNumber = phoneNumber;
        }

        public override string ToString()
        {
            return $"{Id},{Name},{Email}, {PhoneNumber}";
        }

        public static Customer FromString(string input)
        {
            string[] parts = input.Split(',');
            return new Customer
            (
                Guid.Parse(parts[0]),
                parts[1],
                parts[2],
                int.Parse(parts[3])
             );
        }
    }
}
