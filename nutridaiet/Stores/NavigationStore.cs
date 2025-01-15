using CommunityToolkit.Mvvm.ComponentModel;
using nutridaiet.ViewModels;

namespace nutridaiet.Stores
{
    public class NavigationStore : ObservableObject
    {
        private ViewModelBase _currentViewModel;

        public ViewModelBase CurrentViewModel
        {
            get => _currentViewModel;
            set => SetProperty(ref _currentViewModel, value);
        }
    }
}