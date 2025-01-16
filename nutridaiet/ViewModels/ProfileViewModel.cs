using System.Collections.Generic;
using Avalonia.SimpleRouter;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace nutridaiet.ViewModels
{
    public partial class ProfileViewModel : ViewModelBase
    {
        private readonly HistoryRouter<ViewModelBase> _router;

        [ObservableProperty] private string userName = "膳食家";

        [ObservableProperty] private int age = 27;

        [ObservableProperty] private List<string> tags = new()
        {
            "爱吃面食",
            "注重健康",
            "心脏病患者"
        };

        public ProfileViewModel(HistoryRouter<ViewModelBase> router)
        {
            _router = router;

            EditProfileCommand = new RelayCommand(EditProfile);
            ShareCommand = new RelayCommand(Share);
            CustomerServiceCommand = new RelayCommand(ContactCustomerService);
        }

        public IRelayCommand EditProfileCommand { get; }
        public IRelayCommand ShareCommand { get; }
        public IRelayCommand CustomerServiceCommand { get; }

        private void EditProfile()
        {
            // Implement edit profile logic
        }

        private void Share()
        {
            // Implement share logic
        }

        private void ContactCustomerService()
        {
            // Implement customer service logic
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
                    _router.GoTo<ShopViewModel>();
                    break;
                case "Notification":
                    _router.GoTo<NotificationViewModel>();
                    break;
                case "Profile":
                    // _router.GoTo<ProfileViewModel>();
                    break;
            }
        }
    }
}