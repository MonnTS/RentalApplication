using System;
using System.Collections.Generic;

namespace DBRentalsApplication.Models
{
    public partial class Car
    {
        public Car()
        {
            Rentals = new HashSet<Rental>();
        }

        public int Id { get; set; }
        public string Make { get; set; } = null!;
        public string Model { get; set; } = null!;
        public string RegistrationNumber { get; set; } = null!;

        public virtual ICollection<Rental> Rentals { get; set; }
    }
}
