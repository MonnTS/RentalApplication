using System.Collections.Generic;

namespace DBRentalsApplication.Models
{
    public partial class Driver
    {
        public Driver()
        {
            Rentals = new HashSet<Rental>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string? Email { get; set; }
        public string PhoneNumber { get; set; } = null!;

        public virtual ICollection<Rental> Rentals { get; set; }
    }
}
