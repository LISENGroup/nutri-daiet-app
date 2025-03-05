using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Avalonia.SimpleRouter;
using Microsoft.Maui.Media;
using Microsoft.Maui.Storage;
using nutridaiet.Models;
using FilePickerFileType = Avalonia.Platform.Storage.FilePickerFileType;

namespace nutridaiet.ViewModels;

public partial class HomeViewModel : ViewModelBase
{
    private readonly HistoryRouter<ViewModelBase> _router;

    private readonly HttpClient _httpClient = new();

    private readonly IMessenger _messenger;


    [ObservableProperty] private bool _isUploading;
    [ObservableProperty] private string _uploadResult = string.Empty;

    [ObservableProperty] private bool _isCameraMode = true;

    public string UploadButtonText => IsCameraMode ? "拍照上传" : "选择图片上传";

    public HomeViewModel(HistoryRouter<ViewModelBase> router, IMessenger messenger)
    {
        _router = router;
        _messenger = messenger;
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
        if (topLevel?.StorageProvider == null) return;

        try
        {
            IsUploading = true;
            UploadResult = "正在处理...";

            IStorageFile file;
            if (IsCameraMode)
            {
                file = await CaptureImageAsync();
            }
            else
            {
                file = await SelectImageFile(topLevel.StorageProvider);
            }

            if (file == null)
            {
                UploadResult = IsCameraMode ? "未能捕获图像" : "未选择文件";
                return;
            }

            // 上传文件
            var result = await UploadToServer(file);
            HandleUploadResult(result, file);
        }
        catch (Exception ex)
        {
            UploadResult = $"处理失败: {ex.Message}";
        }
        finally
        {
            IsUploading = false;
        }
    }

    private async Task<IStorageFile?> CaptureImageAsync()
    {
        if (MediaPicker.Default.IsCaptureSupported)
        {
            FileResult photo = await MediaPicker.Default.CapturePhotoAsync();

            if (photo != null)
            {
                // save the file into local storage
                string localFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);

                using Stream sourceStream = await photo.OpenReadAsync();
                using FileStream localFileStream = File.OpenWrite(localFilePath);

                await sourceStream.CopyToAsync(localFileStream);

                // 使用 Avalonia UI 的 StorageProvider 来获取 IStorageFile
                var storageProvider = TopLevel.GetTopLevel(App.TopLevel)?.StorageProvider;
                if (storageProvider != null)
                {
                    try
                    {
                        // 尝试从本地文件路径获取 IStorageFile
                        return await storageProvider.TryGetFileFromPathAsync(localFilePath);
                    }
                    catch (Exception ex)
                    {
                        // ignored
                    }
                }
            }
        }

        // 这里应该调用摄像头 API，捕获图像，并返回一个 IStorageFile
        return null;
    }

    private async Task<IStorageFile?> SelectImageFile(IStorageProvider storageProvider)
    {
        var options = new FilePickerOpenOptions
        {
            AllowMultiple = false,
            FileTypeFilter = new List<FilePickerFileType>
            {
                new("Images") { Patterns = new[] { "*.png", "*.jpg", "*.jpeg" } }
            }
        };

        var result = await storageProvider.OpenFilePickerAsync(options);
        return result.Count > 0 ? result[0] : null;
    }

    private async Task<UploadResult> UploadToServer(IStorageFile file)
    {
        await using var fileStream = await file.OpenReadAsync();

        using var content = new MultipartFormDataContent
        {
            { new StreamContent(fileStream), "image", file.Name }
        };

        var response = await _httpClient.PostAsync(
            "https://lively-rich-tadpole.ngrok-free.app/image/res",
            content
        );

        var responseString = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<UploadResult>(responseString)!;
    }

    private async void HandleUploadResult(UploadResult result, IStorageFile file)
    {
        if (!string.IsNullOrEmpty(result.Error))
        {
            UploadResult = $"错误: {result.Error}";
        }
        else if (!string.IsNullOrEmpty(result.Result))
        {
            UploadResult = $"识别结果: {result.Result}";
            _router.GoTo<FoodDetailsViewModel>();

            await Task.Delay(100);
            var imagePath = await SaveTempImage(file); // 需要实现临时文件保存
            var foodName = result.Result;
            _messenger.Send(new FoodAnalysisMessage((foodName, imagePath)));
        }
        else
        {
            UploadResult = "未知响应格式";
        }
    }

    private async Task<string> SaveTempImage(IStorageFile file)
    {
        var tempPath = Path.Combine(Path.GetTempPath(), file.Name);
        await using var stream = await file.OpenReadAsync();
        await using var fileStream = File.Create(tempPath);
        await stream.CopyToAsync(fileStream);
        return tempPath;
    }
}

public class UploadResult
{
    [JsonPropertyName("result")] public string Result { get; set; } = string.Empty;
    [JsonPropertyName("error")] public string Error { get; set; } = string.Empty;
}