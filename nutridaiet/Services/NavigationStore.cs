using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace nutridaiet.Services;

public class NavigationStore
{
    public event Action CurrentViewModelChanged;

    private ObservableObject _currentViewModel;

    public ObservableObject CurrentViewModel
    {
        get => _currentViewModel;
        private set
        {
            _currentViewModel = value;
            CurrentViewModelChanged?.Invoke();
        }
    }

    public async Task NavigateWithAnimationAsync(Func<ObservableObject> createViewModel)
    {
        // Create new view model
        var newViewModel = createViewModel();

        // Trigger animation (you'll need to implement this in your view)
        await Task.Delay(300); // Simulated animation duration

        // Update current view model
        CurrentViewModel = newViewModel;
    }
}