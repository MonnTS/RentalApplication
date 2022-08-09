using DBRentalsApplication.Services;
using DBRentalsApplication.Stores;
using DBRentalsApplication.ViewModel;

namespace DBRentalsApplication.Commands;

public class NavigateToMenuCommand : CommandBase
{
    private readonly DataBaseService _dataBaseService;
    private readonly NavigationStore _navigationStore;

    public NavigateToMenuCommand(DataBaseService dbService, NavigationStore navigationStore)
    {
        _dataBaseService = dbService;
        _navigationStore = navigationStore;
    }

    public override void Execute(object? parameter)
    {
        _navigationStore.CurrentViewModel = new RentalViewModel(_dataBaseService, _navigationStore);
    }
}