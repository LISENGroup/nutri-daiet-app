using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Avalonia.SimpleRouter;
using Microsoft.Maui.Media;
using Microsoft.Maui.Storage;
using nutridaiet.Models;

namespace nutridaiet.ViewModels;

public partial class HomeViewModel : ViewModelBase
{
    private readonly HistoryRouter<ViewModelBase> _router;
    private readonly HttpClient _httpClient = new();
    private readonly IMessenger _messenger;

    [ObservableProperty] private bool _isUploading;
    [ObservableProperty] private string _uploadResult = string.Empty;
    [ObservableProperty] private bool _isCameraMode = true;
    public string UploadButtonText => "拍照或选择图片上传";

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
        try
        {
            IsUploading = true;
            UploadResult = "正在处理...";

            FileResult fileResult;
            if (IsCameraMode)
            {
                fileResult = await CaptureImageAsync();
            }
            else
            {
                fileResult = await SelectImageFileAsync();
            }

            if (fileResult == null)
            {
                UploadResult = IsCameraMode ? "未能捕获图像" : "未选择文件";
                return;
            }

            // 上传文件
            var result = await UploadToServer(fileResult);
            HandleUploadResult(result, fileResult);
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

    /// <summary>
    /// 使用 MAUI MediaPicker 拍摄照片，并返回 FileResult 对象。
    /// </summary>
    private async Task<FileResult?> CaptureImageAsync()
    {
        if (MediaPicker.Default.IsCaptureSupported)
        {
            try
            {
                FileResult photo = await MediaPicker.Default.CapturePhotoAsync();
                return photo; // 返回拍摄结果（FileResult 包含文件名及访问流的方法）
            }
            catch (Exception ex)
            {
                UploadResult = $"拍照异常: {ex.Message}";
            }
        }
        else
        {
            UploadResult = "当前平台不支持摄像头捕获";
        }

        return null;
    }

    /// <summary>
    /// 使用 MAUI FilePicker 选择图片文件，并返回 FileResult 对象。
    /// </summary>
    private async Task<FileResult?> SelectImageFileAsync()
    {
        try
        {
            var fileResult = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "请选择图片",
                FileTypes = FilePickerFileType.Images,
            });
            return fileResult;
        }
        catch (Exception ex)
        {
            UploadResult = $"文件选择异常: {ex.Message}";
            return null;
        }
    }

    /// <summary>
    /// 通过上传文件流到服务器，解析返回结果。
    /// </summary>
    private async Task<UploadResult> UploadToServer(FileResult fileResult)
    {
        using var fileStream = await fileResult.OpenReadAsync();
        using var content = new MultipartFormDataContent
        {
            { new StreamContent(fileStream), "image", fileResult.FileName }
        };

        var response = await _httpClient.PostAsync(
            "https://lively-rich-tadpole.ngrok-free.app/image/res",
            content
        );

        var responseString = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<UploadResult>(responseString)!;
    }

    /// <summary>
    /// 根据服务器返回结果处理上传反馈，并发送消息到其他模块。
    /// </summary>
    private async void HandleUploadResult(UploadResult result, FileResult fileResult)
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
            var imagePath = await SaveTempImage(fileResult);
            var foodName = result.Result;
            _messenger.Send(new FoodAnalysisMessage((foodName, imagePath)));
        }
        else
        {
            UploadResult = "未知响应格式";
        }
    }

    /// <summary>
    /// 将选择或拍摄的图片保存为临时文件，并返回文件路径。
    /// </summary>
    private async Task<string> SaveTempImage(FileResult fileResult)
    {
        var tempPath = Path.Combine(FileSystem.CacheDirectory, fileResult.FileName);
        using var stream = await fileResult.OpenReadAsync();
        using var fileStream = File.Create(tempPath);
        await stream.CopyToAsync(fileStream);
        return tempPath;
    }
}

public class UploadResult
{
    [JsonPropertyName("result")] public string Result { get; set; } = string.Empty;

    [JsonPropertyName("error")] public string Error { get; set; } = string.Empty;
}