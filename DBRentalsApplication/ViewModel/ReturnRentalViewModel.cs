using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using DBRentalsApplication.Commands;
using DBRentalsApplication.Models;
using DBRentalsApplication.Services;
using DBRentalsApplication.Stores;

namespace DBRentalsApplication.ViewModel;

public class ReturnRentalViewModel : ViewBaseModel
{
    private int _id;
    private DateTime _returnDate = DateTime.Today;
    private string? _comments;

    public ReturnRentalViewModel(DataBaseService dbService, NavigationStore navigationStore)
    {
        RentedCars = dbService.GetRentedCars();
        CommandBack = new NavigateToMenuCommand(dbService, navigationStore);
        CommandReturn = new ReturnCarCommand(dbService, this);
    }

    public int Id
    {
        get => _id;
        set
        {
            _id = value;
            OnPropertyChanged(nameof(Id));
        }
    }

    public DateTime ReturnDate
    {
        get => _returnDate;
        set
        {
            _returnDate = value;
            OnPropertyChanged(nameof(ReturnDate));
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

    public ICommand CommandBack { get; }
    public ICommand CommandReturn { get; }
    public ObservableCollection<Car> RentedCars { get; }
}