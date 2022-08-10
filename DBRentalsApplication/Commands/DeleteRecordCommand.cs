using System.Windows;
using DBRentalsApplication.Services;
using DBRentalsApplication.ViewModel;

namespace DBRentalsApplication.Commands;

public class DeleteRecordCommand : CommandBase
{
    private readonly DataBaseService _dbService;
    private readonly DeleteRentalViewModel _deleteRentalViewModel;

    public DeleteRecordCommand(DataBaseService dbService, DeleteRentalViewModel deleteRentalViewModel)
    {
        _dbService = dbService;
        _deleteRentalViewModel = deleteRentalViewModel;
    }

    public override void Execute(object? parameter)
    {
        var showDialog = MessageBox.Show("Are you sure you want to delete this record?", "Delete Record",
            MessageBoxButton.OKCancel, MessageBoxImage.Question);

        if (showDialog == MessageBoxResult.OK)
        {
            var deleteRecord = _dbService.DeleteRental(_deleteRentalViewModel.Rental.Id);
            if (deleteRecord)
            {
                _deleteRentalViewModel.RentalList.Remove(_deleteRentalViewModel.Rental);
                MessageBox.Show("Record deleted successfully", "Delete Record", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }    

            MessageBox.Show("Record not deleted", "Delete Record", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}