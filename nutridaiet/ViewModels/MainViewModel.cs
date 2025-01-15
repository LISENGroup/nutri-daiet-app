using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace nutridaiet.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    public MainViewModel()
    {
        UploadCommand = new AsyncRelayCommand(UploadImageAsync);
    }

    public IAsyncRelayCommand UploadCommand { get; }

    private async Task UploadImageAsync()
    {
        var storageProvider = GetStorageProvider();
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
                var filePath = result[0].Path.LocalPath;
                // Implement your image upload logic here
            }
        }
    }

    private IStorageProvider GetStorageProvider()
    {
        if (App.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            return desktop.MainWindow.StorageProvider;
        }
        else if (App.Current.ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            // 检查 MainView 是否实现了 IStorageProvider 接口
            if (singleViewPlatform.MainView is IStorageProvider mainViewStorageProvider)
            {
                return mainViewStorageProvider;
            }
        }

        return null;
    }
}