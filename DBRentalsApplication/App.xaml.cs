﻿using System.Configuration;
using System.Windows;
using DBRentalsApplication.Models;
using DBRentalsApplication.Services;
using DBRentalsApplication.Stores;
using DBRentalsApplication.ViewModel;
using DBRentalsApplication.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace DBRentalsApplication;

public partial class App : Application
{
    private readonly ServiceProvider _serviceProvider;

    public App()
    {
        var services = new ServiceCollection();
        ConfigureServices(services);
        _serviceProvider = services.BuildServiceProvider();
    }

    private ILogger Logger { get; set; }

    private void ConfigureServices(IServiceCollection services)
    {
        Logger = new LoggerConfiguration()
            .WriteTo.File("Logs\\DBRentals.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        Log.Debug("Logger configured");

        services.AddDbContext<ProdContext>(options =>
        {
            options.UseSqlServer(ConfigurationManager.ConnectionStrings["RentalDataBase"].ConnectionString);
        });
        services.AddTransient(x =>
        {
            var navigationStore = x.GetRequiredService<NavigationStore>();
            var mainWindow = new MainWindow
            {
                DataContext = new MainViewModel(navigationStore)
            };
            return mainWindow;
        });
        services.AddTransient<DeleteRentalView>();
        services.AddSingleton<DataBaseService>();
        services.AddSingleton(x =>
        {
            var navigationStore = new NavigationStore();
            var dbService = x.GetRequiredService<DataBaseService>();
            navigationStore.CurrentViewModel = new RentalViewModel(dbService, navigationStore);
            return navigationStore;
        });
        services.AddSingleton<CreateRentalViewModel>();
        services.AddLogging(x => { x.AddSerilog(Logger, true); });
    }

    private void OnStartup(object sender, StartupEventArgs e)
    {
        var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        _serviceProvider.Dispose();
        Log.Information("Application closed");
    }
}