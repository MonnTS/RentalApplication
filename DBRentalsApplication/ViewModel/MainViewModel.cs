using DBRentalsApplication.Stores;

namespace DBRentalsApplication.ViewModel;

public class MainViewModel : ViewBaseModel
{
    private readonly NavigationStore _navigationStore;

    public MainViewModel(NavigationStore navigationStore)
    {
        _navigationStore = navigationStore;
        _navigationStore.OnNavigationComplete += () => OnPropertyChanged(nameof(CurrentViewModel));
    }

    public ViewBaseModel CurrentViewModel => _navigationStore.CurrentViewModel;
}