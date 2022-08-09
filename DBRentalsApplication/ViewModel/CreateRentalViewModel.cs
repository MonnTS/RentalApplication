using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using DBRentalsApplication.Commands;
using DBRentalsApplication.Models;
using DBRentalsApplication.Services;
using DBRentalsApplication.Stores;

namespace DBRentalsApplication.ViewModel;

public class CreateRentalViewModel : ViewBaseModel
{
    private int _carId;
    private string? _comments;
    private int _driverId;
    private DateTime _rentalDate = DateTime.Today;

    public CreateRentalViewModel(NavigationStore navigationStore, DataBaseService dbService)
    {
        Drivers = dbService.GetDrivers();
        Cars = dbService.GetAvailableCars();
        BackCommand = new NavigateToMenuCommand(dbService, navigationStore);
        CreateCommand = new CreateRentalCommand(dbService, this);
    }
    
    public int DriverId
    {
        get => _driverId;
        set
        {
            _driverId = value;
            OnPropertyChanged(nameof(DriverId));
        }
    }

    public int CarId
    {
        get => _carId;
        set
        {
            _carId = value;
            OnPropertyChanged(nameof(CarId));
        }
    }

    public DateTime RentalDate
    {
        get => _rentalDate;
        set
        {
            _rentalDate = value;
            OnPropertyChanged(nameof(RentalDate));
        }
    }

    public string? Comments
    {
        get => _comments;
        set
        {
            _comments = value;
            OnPropertyChanged(nameof(Comments));
        }
    }

    public ICommand BackCommand { get; }
    public ICommand CreateCommand { get; }

    public ObservableCollection<Driver> Drivers { get; }

    private ObservableCollection<Car> _cars;

    public ObservableCollection<Car> Cars
    {
        get => _cars;
        set
        {
            _cars = value;
            OnPropertyChanged(nameof(Cars));
        }
    }
}