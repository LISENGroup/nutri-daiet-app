using Avalonia.SimpleRouter;
using CommunityToolkit.Mvvm.Input;

namespace nutridaiet.ViewModels;

public partial class ShopViewModel : ViewModelBase
{
    private HistoryRouter<ViewModelBase> _router;

    public ShopViewModel(HistoryRouter<ViewModelBase> router)
    {
        _router = router;
    }

    [RelayCommand]
    private void Navigate(string parameter = "")
    {
        switch (parameter)
        {
            case "Home":
                _router.GoTo<HomeViewModel>();
                break;
            case "Shop":
                // _router.GoTo<ShopViewModel>();
                break;
            case "Notification":
                _router.GoTo<NotificationViewModel>();
                break;
            case "Profile":
                _router.GoTo<ProfileViewModel>();
                break;
        }
    }
}