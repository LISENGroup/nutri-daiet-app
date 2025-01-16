using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Avalonia.SimpleRouter;

namespace nutridaiet.ViewModels;

public partial class HomeViewModel : ViewModelBase
{
    private readonly HistoryRouter<ViewModelBase> _router;


    [ObservableProperty] private bool _isUploading;

    public HomeViewModel(HistoryRouter<ViewModelBase> router)
    {
        _router = router;
        UploadCommand = new AsyncRelayCommand(UploadImageAsync, () => !IsUploading);
    }

    [RelayCommand]
    private void Navigate(string parameter = "")
    {
        switch (parameter)
        {
            case "Home":
                // _router.GoTo<HomeViewModel>();
                break;
            case "Shop":
                _router.GoTo<ShopViewModel>();
                break;
            case "Notification":
                _router.GoTo<NotificationViewModel>();
                break;
            case "Profile":
                _router.GoTo<ProfileViewModel>();
                break;
        }
    }


    public IAsyncRelayCommand UploadCommand { get; }

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
                    // Navigate to details view
                    // _router.GoTo<FoodDetailsViewModel>();
                }
            }
        }
    }
}