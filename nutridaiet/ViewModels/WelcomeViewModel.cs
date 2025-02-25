using System;
using System.Threading.Tasks;
using Avalonia.SimpleRouter;

namespace nutridaiet.ViewModels;

public partial class WelcomeViewModel : ViewModelBase
{
    private readonly HistoryRouter<ViewModelBase> _router;

    public WelcomeViewModel(HistoryRouter<ViewModelBase> router)
    {
        _router = router;
        InitializeAsync(); // 调用异步初始化方法
    }

    private async void InitializeAsync()
    {
        try
        {
            // 等待 2 秒
            await Task.Delay(2000);
            // 确保导航操作在 UI 线程执行（Avalonia 通常会自动处理）
            // _router.GoTo<HomeViewModel>();
            _router.GoTo<LoginViewModel>();
        }
        catch (Exception ex)
        {
            // 处理异常（例如日志记录）
            Console.WriteLine($"导航失败: {ex.Message}");
        }
    }
}