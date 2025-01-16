using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace nutridaiet.ViewModels;

public partial class HomeViewModel : ViewModelBase
{
    private readonly IStorageProvider _storageProvider;

    [ObservableProperty] private bool _isUploading;

    public HomeViewModel(IStorageProvider storageProvider)
    {
        _storageProvider = storageProvider;
        UploadCommand = new AsyncRelayCommand(UploadImageAsync, () => !IsUploading);
    }

  

    public IAsyncRelayCommand UploadCommand { get; }

    private async Task UploadImageAsync()
    {
        try
        {
            IsUploading = true;

            var result = await _storageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                AllowMultiple = false,
                FileTypeFilter = new List<FilePickerFileType>
                {
                    new("Images") { Patterns = new[] { "*.png", "*.jpg", "*.jpeg" } }
                }
            });

            if (result != null && result.Count > 0)
            {
                // 直接导航到结果页
                WeakReferenceMessenger.Default.Send(new NavigateToMessage(typeof(FoodDetailsViewModel)));
            }
        }
        catch (Exception ex)
        {
            // 处理或记录错误
            Console.WriteLine($"Error during file upload: {ex.Message}");
        }
        finally
        {
            IsUploading = false;
        }
    }
}

// 保持 NavigateToMessage 类不变
public class NavigateToMessage
{
    public Type DestinationViewModel { get; }

    public NavigateToMessage(Type destinationViewModel)
    {
        DestinationViewModel = destinationViewModel;
    }
}