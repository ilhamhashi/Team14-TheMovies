using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheMovies.MVVM.Model.Classes;

namespace TheMovies.MVVM.Model.Repositories
{
    class FileCustomerRepository : ICustomerRepository
    {
        private readonly string customerFilePath;

        public FileCustomerRepository(string filePath)
        {
            customerFilePath = filePath;
            if (!File.Exists(customerFilePath))
            {
                File.AppendAllText(filePath, "Id, Navn, Email, Nummer" + Environment.NewLine);
                File.AppendAllText(customerFilePath, String.Join(Environment.NewLine, demoCustomers() + Environment.NewLine));
            }

        }
        public IEnumerable<Customer> GetAll()
        {
            try
            {
                return File.ReadAllLines(customerFilePath)
                    .Skip(1)
                    .Where(line => !string.IsNullOrEmpty(line))
                    .Select(Customer.FromString)
                    .ToList();
            }
            catch (IOException ex)
            {

                Console.WriteLine($"Fejl ved læsning af fil: {ex.Message}");
                return [];


            }
        }

        public static List<Customer> demoCustomers()
        {
            List<Customer> demoCustomers = new List<Customer>();

            Customer customer1 = new Customer(Guid.NewGuid(), "Jens Pedersen", "jens_11@outlook.dk", 12345678);
            Customer customer2 = new Customer(Guid.NewGuid(), "Marie Lund", "mimihot3@gmail.com", 87654321);

            demoCustomers.AddRange(customer1, customer2);

            return demoCustomers;
        }
    }
}
