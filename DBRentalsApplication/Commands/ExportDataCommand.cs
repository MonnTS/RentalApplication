using DBRentalsApplication.Services;
using Ookii.Dialogs.Wpf;

namespace DBRentalsApplication.Commands;

public class ExportDataCommand : CommandBase
{
    private readonly DataBaseService _dbService;

    public ExportDataCommand(DataBaseService dbService)
    {
        _dbService = dbService;
    }

    public override void Execute(object? parameter)
    {
        var dialog = new VistaFolderBrowserDialog();

        if (dialog.ShowDialog().GetValueOrDefault())
        {
            _dbService.ExportDataToExel(dialog.SelectedPath);
        }
    }
}