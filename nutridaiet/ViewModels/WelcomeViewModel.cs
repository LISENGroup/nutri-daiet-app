using System;
using System.Threading.Tasks;
using Avalonia.SimpleRouter;
using nutridaiet.Services;

namespace nutridaiet.ViewModels;

public partial class WelcomeViewModel : ViewModelBase
{
    private readonly HistoryRouter<ViewModelBase> _router;
    private readonly ISettingsService _settingsService;

    public WelcomeViewModel(HistoryRouter<ViewModelBase> router, ISettingsService settingsService)
    {
        _router = router;
        _settingsService = settingsService;
        InitializeAsync(); // 调用异步初始化方法
    }

    private async void InitializeAsync()
    {
        try
        {
            // 等待 2 秒
            await Task.Delay(2000);
            // 确保导航操作在 UI 线程执行（Avalonia 通常会自动处理）
            // 启动时检测登录状态
            CheckLoginStatus();
        }
        catch (Exception ex)
        {
            // 处理异常（例如日志记录）
            Console.WriteLine($"导航失败: {ex.Message}");
        }
    }

    private void CheckLoginStatus()
    {
        var settings = _settingsService.LoadSettings();
        if (string.IsNullOrEmpty(settings.AccessToken))
        {
            _router.GoTo<LoginViewModel>();
        }
        else
        {
            // 已登录则正常进入主页
            _router.GoTo<HomeViewModel>();
        }
    }
}