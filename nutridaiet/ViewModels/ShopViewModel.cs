using Avalonia.SimpleRouter;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using nutridaiet.Models;

namespace nutridaiet.ViewModels;

public partial class ShopViewModel : ViewModelBase
{
    private HistoryRouter<ViewModelBase> _router;

    // 商品列表
    [ObservableProperty] private ObservableCollection<Product> _products;

    public ShopViewModel(HistoryRouter<ViewModelBase> router)
    {
        _router = router;
        // 初始化商品数据（后续可以从在线API获取）
        Products = new ObservableCollection<Product>
        {
            new Product
            {
                Name = "羽衣甘蓝",
                Description = "根据您视力情况推荐",
                Price = 166.9m,
                ImageUrl = "https://pic.nximg.cn/file/20230820/34599220_200841306105_2.jpg"
            },
            new Product
            {
                Name = "有机苹果",
                Description = "新鲜采摘，健康美味",
                Price = 99.9m,
                ImageUrl = "https://th.bing.com/th/id/OIP.kzo22vGPqcf7d5w_QJhrfQHaFj?rs=1&pid=ImgDetMain"
            },
            new Product
            {
                Name = "有机苹果",
                Description = "新鲜采摘，健康美味",
                Price = 99.9m,
                ImageUrl = "https://th.bing.com/th/id/OIP.kzo22vGPqcf7d5w_QJhrfQHaFj?rs=1&pid=ImgDetMain"
            },
        };
    }


    [RelayCommand]
    private void Navigate(string parameter = "")
    {
        switch (parameter)
        {
            case "Home":
                _router.GoTo<HomeViewModel>();
                break;
            case "Shop":
                // _router.GoTo<ShopViewModel>();
                break;
            case "Notification":
                _router.GoTo<NotificationViewModel>();
                break;
            case "Profile":
                _router.GoTo<ProfileViewModel>();
                break;
        }
    }
}