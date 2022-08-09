using System.Collections.ObjectModel;
using System.Windows.Input;
using DBRentalsApplication.Commands;
using DBRentalsApplication.Models;
using DBRentalsApplication.Services;
using DBRentalsApplication.Stores;

namespace DBRentalsApplication.ViewModel;

public class DeleteRentalViewModel : ViewBaseModel
{
    private Rental _rental;

    public Rental Rental
    {
        get => _rental;
        set
        {
            _rental = value;
            OnPropertyChanged(nameof(Rental));
        }
    }
 
    public DeleteRentalViewModel(NavigationStore navigationStore, DataBaseService dbService)
    {
        RentalList = dbService.GetRental();
        BackCommand = new NavigateToMenuCommand(dbService, navigationStore);
        DeleteRental = new DeleteRecordCommand(dbService, this);
    }
    
    public ICommand DeleteRental { get; }
    public ICommand BackCommand { get; }

    public ObservableCollection<Rental> RentalList { get; set; }
}