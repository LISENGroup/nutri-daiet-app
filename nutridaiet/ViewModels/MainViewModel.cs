using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using nutridaiet.Stores;

namespace nutridaiet.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;

        public MainViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            _navigationStore.CurrentViewModel = this;
            UploadCommand = new AsyncRelayCommand(UploadImageAsync);
            NavigateHomeCommand = new RelayCommand(NavigateHome);
            NavigateShopCommand = new RelayCommand(NavigateShop);
            NavigateNotificationCommand = new RelayCommand(NavigateNotification);
            NavigateProfileCommand = new RelayCommand(NavigateProfile);
        }

        public ViewModelBase CurrentViewModel
        {
            get => _navigationStore.CurrentViewModel;
            set => _navigationStore.CurrentViewModel = value;
        }

        public IAsyncRelayCommand UploadCommand { get; }
        public IRelayCommand NavigateHomeCommand { get; }
        public IRelayCommand NavigateShopCommand { get; }
        public IRelayCommand NavigateNotificationCommand { get; }
        public IRelayCommand NavigateProfileCommand { get; }

        private void NavigateHome()
        {
            CurrentViewModel = this;
        }

        private void NavigateShop()
        {
            CurrentViewModel = new ShopViewModel();
        }

        private void NavigateNotification()
        {
            CurrentViewModel = new NotificationViewModel();
        }

        private void NavigateProfile()
        {
            CurrentViewModel = new ProfileViewModel();
        }

        private async Task UploadImageAsync()
        {
            var topLevel = TopLevel.GetTopLevel(App.TopLevel);
            if (topLevel != null)
            {
                var storageProvider = topLevel.StorageProvider;
                if (storageProvider != null)
                {
                    var options = new FilePickerOpenOptions
                    {
                        AllowMultiple = false,
                        FileTypeFilter = new List<FilePickerFileType>
                        {
                            new FilePickerFileType("Images")
                            {
                                Patterns = new[] { "*.png", "*.jpg", "*.jpeg" }
                            }
                        }
                    };

                    var result = await storageProvider.OpenFilePickerAsync(options);
                    if (result != null && result.Count > 0)
                    {
                        CurrentViewModel = new FoodDetailsViewModel();
                    }
                }
            }
        }
    }
}
