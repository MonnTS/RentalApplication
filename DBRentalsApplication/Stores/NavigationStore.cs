using System;
using DBRentalsApplication.ViewModel;

namespace DBRentalsApplication.Stores;

public class NavigationStore
{
    private ViewBaseModel _currentViewModel;
    public event Action? OnNavigationComplete;
   
    public ViewBaseModel CurrentViewModel
    {
        get => _currentViewModel;
        set
        { 
            _currentViewModel = value;
            OnNavigationCompleted();
        }
    }
    
    private void OnNavigationCompleted()
    {
        OnNavigationComplete?.Invoke();
    }
}