using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Avalonia.SimpleRouter;

namespace nutridaiet.ViewModels;

public  partial class MainViewModel: ViewModelBase
{
    [ObservableProperty] private ViewModelBase _content = default;

    public MainViewModel(HistoryRouter<ViewModelBase> router)
    {
        router.CurrentViewModelChanged += viewModel => Content = viewModel;
        router.GoTo<WelcomeViewModel>();

    }

}