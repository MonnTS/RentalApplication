using System.Linq;
using System.Windows.Forms;
using DBRentalsApplication.Services;
using DBRentalsApplication.ViewModel;

namespace DBRentalsApplication.Commands;

public class ReturnCarCommand : CommandBase
{
    private readonly DataBaseService _dbService;
    private readonly ReturnRentalViewModel _returnRentalViewModel;

    public ReturnCarCommand(DataBaseService dbService, ReturnRentalViewModel returnRentalViewModel)
    {
        _dbService = dbService;
        _returnRentalViewModel = returnRentalViewModel;
    }

    public override void Execute(object? parameter)
    {
        _dbService.ReturnCar(_returnRentalViewModel.Id, _returnRentalViewModel.ReturnDate,
            _returnRentalViewModel.Comments);

        if (_dbService.IsCompleted)
        {
            MessageBox.Show("Car returned successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            foreach (var item in _returnRentalViewModel.RentedCars.ToList().Where(x => x.Id == _returnRentalViewModel.Id))
            {
                _returnRentalViewModel.RentedCars.Remove(item);
            }
            
            return;
        }
        
        MessageBox.Show("Car returned unsuccessfully", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}