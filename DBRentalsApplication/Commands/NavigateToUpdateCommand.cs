using DBRentalsApplication.Services;
using DBRentalsApplication.Stores;
using DBRentalsApplication.ViewModel;

namespace DBRentalsApplication.Commands;

public class NavigateToUpdateCommand : CommandBase
{
    private readonly DataBaseService _dbService;
    private readonly NavigationStore _navigationStore;

    public NavigateToUpdateCommand(NavigationStore navigationStore, DataBaseService dbService)
    {
        _dbService = dbService;
        _navigationStore = navigationStore;
    }

    public override void Execute(object? parameter)
    {
        _navigationStore.CurrentViewModel = new ReturnRentalViewModel(_dbService, _navigationStore);
    }
}