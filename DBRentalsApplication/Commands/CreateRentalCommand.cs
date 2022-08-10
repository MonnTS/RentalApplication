using System.Linq;
using System.Windows;
using DBRentalsApplication.Services;
using DBRentalsApplication.ViewModel;

namespace DBRentalsApplication.Commands;

public class CreateRentalCommand : CommandBase
{
    private readonly DataBaseService _dbService;
    private readonly CreateRentalViewModel _viewModel;

    public CreateRentalCommand(DataBaseService dbService, CreateRentalViewModel viewModel)
    {
        _dbService = dbService;
        _viewModel = viewModel;
    }

    public override void Execute(object? parameter)
    {
        var newRental = _dbService.MakeNewRental(_viewModel.DriverId, _viewModel.CarId,
            _viewModel.RentalDate, _viewModel.Comments);

        if (newRental)
        {
            MessageBox.Show("Rental created successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            foreach (var item in _viewModel.Cars.ToList().Where(x => x.Id == _viewModel.CarId))
            {
                _viewModel.Cars.Remove(item);
            }

            return;
        }
        
        MessageBox.Show("Rental creation failed", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
    }
}