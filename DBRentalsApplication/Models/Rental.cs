using System;

namespace DBRentalsApplication.Models
{
    public partial class Rental
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        public int DriverId { get; set; }
        public DateTime RentDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string? Comments { get; set; }

        public virtual Car Car { get; set; } = null!;
        public virtual Driver Driver { get; set; } = null!;
    }
}
