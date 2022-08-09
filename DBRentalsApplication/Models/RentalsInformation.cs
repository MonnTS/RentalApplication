using System;

namespace DBRentalsApplication.Models;

public class RentalsInformation
{
    public string RegistrationNumber { get; set; }
    public string Make { get; set; }
    public string Model { get; set; }
    public string DriverName { get; set; }
    public string DriverSurname { get; set; }
    public DateTime RentDate { get; set; }
    public DateTime? ReturnDate { get; set; }
}