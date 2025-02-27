using System;
using System.Diagnostics;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Avalonia.SimpleRouter;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using nutridaiet.Models;
using nutridaiet.Services;

namespace nutridaiet.ViewModels;

public partial class LoginViewModel : ViewModelBase
{
    private readonly HistoryRouter<ViewModelBase> _router;
    private readonly ISettingsService _settingsService;
    private readonly IApiService _apiService;

    [ObservableProperty] private string _username = string.Empty; // 绑定到账号/邮箱输入框

    [ObservableProperty] private string _password = string.Empty; // 绑定到密码输入框

    [ObservableProperty] private bool _isLoading; // 控制登录按钮的加载状态

    [ObservableProperty] private string _errorMessage = string.Empty; // 错误提示信息

    public LoginViewModel(HistoryRouter<ViewModelBase> router, ISettingsService settingsService,
        IApiService apiService)
    {
        _router = router;
        _settingsService = settingsService;
        _apiService = apiService;
    }

    [RelayCommand(AllowConcurrentExecutions = false)]
    private async Task LoginAsync()
    {
        if (IsLoading) return;

        try
        {
            IsLoading = true;
            ErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "账号和密码不能为空";
                return;
            }

            var response = await _apiService.LoginAsync(Username, Password);

            // 保存配置
            _settingsService.SaveSettings(new Settings
            {
                UserId = response.User.Id,
                AccessToken = response.AccessToken,
                Email = response.User.Email,
                Username = response.User.Username,
                CreateTime = response.User.CreateTime
            });


            _router.GoTo<HomeViewModel>();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"登录失败: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }


    [RelayCommand]
    private void NavigateToRegister()
    {
        // _router.GoTo<RegisterViewModel>(); // 假设存在 RegisterViewModel
    }

    [RelayCommand]
    private void OpenPrivacyPolicy() => OpenUrl("https://yourdomain.com/privacy");

    [RelayCommand]
    private void OpenUserAgreement() => OpenUrl("https://yourdomain.com/agreement");

    private static void OpenUrl(string url)
    {
        // 使用 Avalonia 的浏览器打开链接
        Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
    }
}