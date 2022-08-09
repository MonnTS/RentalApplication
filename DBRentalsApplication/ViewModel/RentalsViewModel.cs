using System.Collections.ObjectModel;
using System.Windows.Input;
using DBRentalsApplication.Commands;
using DBRentalsApplication.Models;
using DBRentalsApplication.Services;
using DBRentalsApplication.Stores;

namespace DBRentalsApplication.ViewModel;

public class RentalViewModel : ViewBaseModel
{
    public RentalViewModel(DataBaseService dbService, NavigationStore navigationStore)
    {
        MakeNewRentals = new MakeNewRentalCommand(navigationStore, dbService);
        DeleteRental = new NavigateToDeleteCommand(navigationStore, dbService);
        UpdateRental = new NavigateToUpdateCommand(navigationStore, dbService);
        ExportData = new ExportDataCommand(dbService);
        RentalsInformations = dbService.GetRentalReport();
    }

    public ICommand MakeNewRentals { get; }
    public ICommand DeleteRental { get; }
    public ICommand UpdateRental { get; }
    public ICommand ExportData { get; }
    public ObservableCollection<RentalsInformation> RentalsInformations { get; }
}