using System.Collections.Generic;
using Avalonia.SimpleRouter;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
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

        var json = File.ReadAllText("C:\\Users\\breeze\\RiderProjects\\nutri-daiet-app\\nutridaiet\\Assets\\Products.json");

        var products = JsonSerializer.Deserialize<List<Product>>(json);

        Products = new ObservableCollection<Product>(products);
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