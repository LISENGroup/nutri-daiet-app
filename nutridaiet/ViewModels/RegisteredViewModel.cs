using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Avalonia.SimpleRouter;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using nutridaiet.Services;

namespace nutridaiet.ViewModels
{
    public partial class RegisteredViewModel : ViewModelBase
    {
        private readonly HistoryRouter<ViewModelBase> _router;
        private readonly IApiService _apiService;

        [ObservableProperty] private string _username = string.Empty;

        [ObservableProperty] [NotifyPropertyChangedFor(nameof(CanSendCode))]
        private string _email = string.Empty;

        [ObservableProperty] private string _verificationCode = string.Empty;

        [ObservableProperty] private string _password = string.Empty;

        [ObservableProperty] private bool _acceptTerms;

        [ObservableProperty] private bool _isLoading;

        [ObservableProperty] private string _errorMessage = string.Empty;

        [ObservableProperty] private int _countdownSeconds;

        public bool CanSendCode => !IsLoading && CountdownSeconds == 0;

        public RegisteredViewModel(
            HistoryRouter<ViewModelBase> router,
            IApiService apiService)
        {
            _router = router;
            _apiService = apiService;
        }

        // 发送验证码命令
        [RelayCommand(CanExecute = nameof(CanSendCode))]
        private async Task SendVerificationCodeAsync()
        {
            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;

                // 调用发送验证码API（示例）
                // await _apiService.SendVerificationCodeAsync(Email);

                // 启动60秒倒计时
                StartCountdown(60);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"发送失败: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        // 注册命令
        [RelayCommand]
        private async Task RegisterAsync()
        {
            if (IsLoading) return;

            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;

                // 输入验证
                if (!ValidateInputs())
                    return;

                // 调用注册API（示例）
                // var success = await _apiService.RegisterAsync(
                //     Username,
                //     Email,
                //     Password,
                //     VerificationCode
                // );
                var success = true;

                if (success)
                {
                    _router.GoTo<LoginViewModel>();
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"注册失败: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        // 导航到登录页面
        [RelayCommand]
        private void NavigateToLogin() => _router.GoTo<LoginViewModel>();

        // 启动倒计时
        private async void StartCountdown(int seconds)
        {
            CountdownSeconds = seconds;
            while (CountdownSeconds > 0)
            {
                await Task.Delay(1000);
                CountdownSeconds--;
            }

            OnPropertyChanged(nameof(CanSendCode)); // 倒计时结束后强制刷新
        }

        // 邮箱格式验证
        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            // 使用正则表达式直接校验
            var pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return System.Text.RegularExpressions.Regex.IsMatch(email, pattern);
        }

        // 综合输入验证
        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(Username))
            {
                ErrorMessage = "用户名不能为空";
                return false;
            }

            if (!IsValidEmail(Email))
            {
                ErrorMessage = "邮箱格式不正确";
                return false;
            }

            if (string.IsNullOrWhiteSpace(VerificationCode))
            {
                ErrorMessage = "验证码不能为空";
                return false;
            }

            if (Password.Length < 8 ||
                !System.Text.RegularExpressions.Regex.IsMatch(Password, @"^(?=.*[A-Za-z])(?=.*\d).{8,16}$"))
            {
                ErrorMessage = "密码需8-16位字母加数字组合";
                return false;
            }

            if (!AcceptTerms)
            {
                ErrorMessage = "请先同意用户协议";
                return false;
            }

            return true;
        }

        //  手动触发 CanSendCode 更新
        partial void OnEmailChanged(string value)
        {
            // 触发 CanSendCode 重新计算
            OnPropertyChanged(nameof(CanSendCode));

            // 可选：直接更新错误提示（如果需要实时显示错误）
            if (!IsValidEmail(value))
                ErrorMessage = "邮箱格式不正确";
            else
                ErrorMessage = string.Empty;
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
}