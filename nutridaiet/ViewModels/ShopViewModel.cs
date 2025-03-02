using System.Collections.Generic;
using Avalonia.SimpleRouter;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using nutridaiet.Models;

namespace nutridaiet.ViewModels;

public partial class ShopViewModel : ViewModelBase
{
    private HistoryRouter<ViewModelBase> _router;
    private static readonly HttpClient _httpClient = new();

    [ObservableProperty] private ObservableCollection<Product> _products = new();


    public ShopViewModel(HistoryRouter<ViewModelBase> router)
    {
        _router = router;
        InitializeAsync();
    }

    private async void InitializeAsync()
    {
        var response = await _httpClient.GetAsync(
            "https://lively-rich-tadpole.ngrok-free.app/api/market/product");

        if (!response.IsSuccessStatusCode)
        {
            return;
        }

        var json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<ApiResponse>(json)!;

        if (result.Code != 200)
        {
            return;
        }

        Products = new ObservableCollection<Product>(result.Data);
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

public class ApiResponse
{
    [JsonPropertyName("code")] public int Code { get; set; }

    [JsonPropertyName("data")] public List<Product> Data { get; set; } = new();
}