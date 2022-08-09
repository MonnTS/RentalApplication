using DBRentalsApplication.Services;
using DBRentalsApplication.Stores;
using DBRentalsApplication.ViewModel;

namespace DBRentalsApplication.Commands;

public class NavigateToDeleteCommand : CommandBase
{
    private readonly DataBaseService _dbService;
    private readonly NavigationStore _navigationStore;

    public NavigateToDeleteCommand(NavigationStore navigationStore, DataBaseService dbService)
    {
        _navigationStore = navigationStore;
        _dbService = dbService;
    }

    public override void Execute(object? parameter)
    {
        _navigationStore.CurrentViewModel = new DeleteRentalViewModel(_navigationStore, _dbService);
    }
}