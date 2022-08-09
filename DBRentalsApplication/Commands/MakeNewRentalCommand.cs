using DBRentalsApplication.Services;
using DBRentalsApplication.Stores;
using DBRentalsApplication.ViewModel;

namespace DBRentalsApplication.Commands;

public class MakeNewRentalCommand : CommandBase
{
    private readonly DataBaseService _dbService;
    private readonly NavigationStore _navigationStore;

    public MakeNewRentalCommand(NavigationStore navigationStore, DataBaseService dbService)
    {
        _dbService = dbService;
        _navigationStore = navigationStore;
    }

    public override void Execute(object? parameter)
    {
        _navigationStore.CurrentViewModel = new CreateRentalViewModel(_navigationStore, _dbService);
    }
}