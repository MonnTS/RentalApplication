using System;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using DBRentalsApplication.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DBRentalsApplication.Services;

public class DataBaseService
{
    private readonly ProdContext _dbContext;
    private readonly ILogger<DataBaseService> _logger;
    public bool IsCompleted { get; private set; }
    
    public DataBaseService(ProdContext dbContext, ILogger<DataBaseService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <summary>
    ///     Executes a stored procedure that returns a list of all rentals.
    /// </summary>
    /// <returns></returns>
    public ObservableCollection<RentalsInformation> GetRentalReport()
    {
        var rentals = new ObservableCollection<RentalsInformation>();

        try
        {
            using var command = _dbContext.Database.GetDbConnection().CreateCommand();
            command.CommandText = "RentalsInformation";
            command.CommandType = CommandType.StoredProcedure;

            _dbContext.Database.OpenConnection();

            using var result = command.ExecuteReader();
            while (result.Read())
            {
                var rental = new RentalsInformation
                {
                    RegistrationNumber = (string)result["RegistrationNumber"],
                    Make = (string)result["Make"],
                    Model = (string)result["Model"],
                    DriverName = (string)result["DriverName"],
                    DriverSurname = (string)result["DriverSurname"],
                    RentDate = (DateTime)result["RentDate"],
                    ReturnDate = result.IsDBNull(6) ? null : (DateTime)result["ReturnDate"]
                };
                rentals.Add(rental);
            }
            
            _logger.LogInformation("Successfully retrieved information from the database.");
            _dbContext.Database.CloseConnection();
            IsCompleted = true;
        }
        catch (Exception e)
        {
            _logger.LogError("Error while getting rentals information: {ErrorMessage}", e.Message);
            IsCompleted = false;
        }
        finally
        {
            _dbContext.Database.CloseConnection();
        }

        return rentals;
    }

    /// <summary>
    ///     Inserting new rental into the database.
    /// </summary>
    /// <param name="driverId"></param>
    /// <param name="carId"></param>
    /// <param name="rentDate"></param>
    /// <param name="comments"></param>
    public void MakeNewRental(int driverId, int carId, DateTime rentDate, string? comments)
    {
        try
        {
            using var command = _dbContext.Database.GetDbConnection().CreateCommand();
            command.CommandText = "MakeNewRental";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@DriverId", driverId));
            command.Parameters.Add(new SqlParameter("@CarId", carId));
            command.Parameters.Add(new SqlParameter("@RentDate", rentDate));
            command.Parameters.Add(new SqlParameter("@Comments", comments ?? (object)DBNull.Value));
            _dbContext.Database.OpenConnection();
            command.ExecuteNonQuery();
            _dbContext.SaveChanges();
            _dbContext.Database.CloseConnection();
            IsCompleted = true;
            _logger.LogInformation("New rental has been made.");
        }
        catch (Exception e)
        {
            _logger.LogError("Error while making new rental: {ErrorMessage}", e.Message);
            IsCompleted = false;
        }
        finally
        {
            _dbContext.Database.CloseConnection();
        }
    }

    /// <summary>
    ///     Deletes all rentals from the database.
    /// </summary>
    /// <param name="id"></param>
    public void DeleteRental(int id)
    {
        try
        {
            using var command = _dbContext.Database.GetDbConnection().CreateCommand();
            command.CommandText = "DeleteRentals";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@Id", id));

            _dbContext.Database.OpenConnection();
            command.ExecuteNonQuery();
            _dbContext.SaveChanges();
            _dbContext.Database.CloseConnection();
            IsCompleted = true;
            _logger.LogInformation("Rental has been deleted.");
        }
        catch (Exception e)
        {
            _logger.LogError("Error deleting rental: {ErrorMessage}", e.Message);
            IsCompleted = false;
        }
        finally
        {
            _dbContext.Database.CloseConnection();
        }
    }

    /// <summary>
    ///     Returns all cars from rental
    /// </summary>
    /// <param name="id"></param>
    /// <param name="returnDate"></param>
    /// <param name="comments"></param>
    public void ReturnCar(int id, DateTime returnDate, string? comments)
    {
        try
        {
            _logger.LogInformation("Returning car with id {Id}", id);
            using var command = _dbContext.Database.GetDbConnection().CreateCommand();
            command.CommandText = "UpdateCarReturn";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@CarId", id));
            command.Parameters.Add(new SqlParameter("@ReturnDate", returnDate));
            command.Parameters.Add(new SqlParameter("@Comments", comments ?? (object)DBNull.Value));

            _dbContext.Database.OpenConnection();
            command.ExecuteNonQuery();
            _dbContext.SaveChanges();
            _dbContext.Database.CloseConnection();
            IsCompleted = true;
            _logger.LogInformation("Car with id {Id} returned", id);
        }
        catch (Exception e)
        {
            _logger.LogError("Error deleting rental: {ErrorMessage}", e.Message);
            IsCompleted = false;
        }
        finally
        {
            _dbContext.Database.CloseConnection();
        }
    }

    /// <summary>
    ///     Exports data to CSV File
    /// </summary>
    public void ExportDataToExel(string filePath)
    {
        using var command = _dbContext.Database.GetDbConnection().CreateCommand();
        command.CommandText = "RentalsInformation";
        command.CommandType = CommandType.StoredProcedure;

        _dbContext.Database.OpenConnection();

        using var result = command.ExecuteReader();
        var dateNow = DateTime.Now.ToString("dd-MM-yyyy");
        var output = Path.Combine(filePath, $"RentalsInformation_{dateNow}.csv");

        using var fs = new StreamWriter(output);

        for (var i = 0; i < result.FieldCount; i++)
        {
            var name = result.GetName(i);
            if (name.Contains(','))
                name = "\"" + name + "\"";

            fs.Write(name + ",");
        }

        fs.WriteLine();

        while (result.Read())
        {
            for (var i = 0; i < result.FieldCount; i++)
            {
                var value = result[i].ToString();
                if (value != null && value.Contains(','))
                    value = "\"" + value + "\"";

                fs.Write(value + ",");
            }

            fs.WriteLine();
        }

        fs.Close();
        _logger.LogInformation("Data exported to {FilePath}", output);
    }

    /// <summary>
    ///     Selects all drivers from the database.
    /// </summary>
    /// <returns></returns>
    public ObservableCollection<Driver> GetDrivers()
    {
        var rentals = new ObservableCollection<Driver>();
        try
        {
            var rentalsList = _dbContext.Drivers.ToList();

            foreach (var rental in rentalsList) rentals.Add(rental);
        }
        catch (Exception e)
        {
            _logger.LogError("Error while getting drivers information: {ErrorMessage}", e.Message);
        }

        _logger.LogInformation("Drivers have been retrieved.");
        return rentals;
    }

    /// <summary>
    ///     Selects Cars where ReturnDate from Rentals Table is null
    /// </summary>
    /// <returns></returns>
    public ObservableCollection<Car> GetAvailableCars()
    {
        var cars = new ObservableCollection<Car>();

        try
        {
            var rentals = _dbContext.Rentals.Where(r => r.ReturnDate == null).ToList();
            var carsList = _dbContext.Cars.ToList();
            foreach (var car in carsList
                         .Where(car => rentals
                             .All(r => r.CarId != car.Id)))
                cars.Add(car);
        }
        catch (Exception e)
        {
            _logger.LogError("Error while getting available cars information: {ErrorMessage}", e.Message);
        }

        _logger.LogInformation("Available cars have been retrieved.");
        return cars;
    }

    /// <summary>
    ///     Returns all rented cars from the database.
    /// </summary>
    /// <returns></returns>
    public ObservableCollection<Car> GetRentedCars()
    {
        var cars = new ObservableCollection<Car>();
        try
        {
            var rentals = _dbContext.Rentals.Where(r => r.ReturnDate == null).ToList();
            var carsList = _dbContext.Cars.ToList();
            foreach (var car in carsList
                         .Where(car => rentals.Any(r => r.CarId == car.Id)))
                cars.Add(car);
        }
        catch (Exception e)
        {
            _logger.LogError("Error while getting rented cars information: {ErrorMessage}", e.Message);
        }

        _logger.LogInformation("Rented cars have been retrieved.");
        return cars;
    }

    /// <summary>
    ///     Returns list of all rentals from the database.
    /// </summary>
    /// <returns></returns>
    public ObservableCollection<Rental> GetRental()
    {
        var rentals = new ObservableCollection<Rental>();
        try
        {
            using var command = _dbContext.Database.GetDbConnection().CreateCommand();
            command.CommandText = "GetRentals";
            command.CommandType = CommandType.StoredProcedure;

            _dbContext.Database.OpenConnection();

            using var result = command.ExecuteReader();
            while (result.Read())
            {
                var rental = new Rental
                {
                    Id = result.GetInt32(0),
                    CarId = result.GetInt32(1),
                    DriverId = result.GetInt32(2),
                    RentDate = result.GetDateTime(3),
                    ReturnDate = result.IsDBNull(4) ? null : result.GetDateTime(4),
                    Comments = result.IsDBNull(5) ? null : result.GetString(5)
                };
                rentals.Add(rental);
            }

            _dbContext.Database.CloseConnection();
        }
        catch (Exception e)
        {
            _logger.LogError("Error while getting rentals information: {ErrorMessage}", e.Message);
        }
        finally
        {
            _dbContext.Database.CloseConnection();
        }

        _logger.LogInformation("Rentals have been retrieved.");
        return rentals;
    }
}