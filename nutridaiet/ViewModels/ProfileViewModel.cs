using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace nutridaiet.ViewModels
{
    public partial class ProfileViewModel : ViewModelBase
    {
        [ObservableProperty] private string userName = "膳食家";

        [ObservableProperty] private int age = 27;

        [ObservableProperty] private List<string> tags = new()
        {
            "爱吃面食",
            "注重健康",
            "心脏病患者"
        };

        public ProfileViewModel()
        {
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
    }
}