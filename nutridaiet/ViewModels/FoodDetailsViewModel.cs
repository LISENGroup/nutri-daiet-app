using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Avalonia.SimpleRouter;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui.Media;
using nutridaiet.Models;

namespace nutridaiet.ViewModels
{
    public partial class FoodDetailsViewModel : ViewModelBase, IRecipient<FoodAnalysisMessage>
    {
        private readonly HistoryRouter<ViewModelBase> _router;

        private readonly HttpClient _httpClient = new();
        private string _currentFoodName = string.Empty;

        [ObservableProperty] private bool _isLoading;
        [ObservableProperty] private string _errorMessage = string.Empty;
        [ObservableProperty] private FoodDetails? _foodDetails;
        [ObservableProperty] private FoodAnalysisResult? _analysisResult = new();

        public FoodDetailsViewModel(HistoryRouter<ViewModelBase> router, IMessenger messenger)
        {
            _router = router;

            messenger.Register<FoodAnalysisMessage>(this);
        }

        // 实现消息处理方法
        public void Receive(FoodAnalysisMessage message)
        {
            FoodDetails = new FoodDetails
            {
                Name = message.Value.FoodName,
                ImagePath = message.Value.ImagePath
            };

            LoadAnalysisData();
        }

        public async Task SpeakNowDefaultSettingsAsync()
        {
            // cts = new CancellationTokenSource();
            if (AnalysisResult?.Guidance != null)
            {
                await TextToSpeech.Default.SpeakAsync(AnalysisResult.Guidance);
                // await TextToSpeech.Default.SpeakAsync("hello hello");
            }
            // This method will block until utterance finishes.
        }


        private async void LoadAnalysisData()
        {
            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;


                var payload = new
                {
                    food_name = _currentFoodName, // 确保此变量在之前已被设置
                    user_info = new
                    {
                        gender = "男",
                        username = "张三峰",
                        age = "20",
                        PA = "中",
                        usertags = new[] { "糖尿病患者", "注重精神健康" }
                    }
                };

                var jsonPayload = JsonSerializer.Serialize(payload);
                using var content = new StringContent(jsonPayload, System.Text.Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(
                    "https://lively-rich-tadpole.ngrok-free.app/ai/response",
                    content
                );

                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<AiResponse>(json)!;

                AnalysisResult = new FoodAnalysisResult
                {
                    Guidance = result.Response
                };
            }
            catch (Exception ex)
            {
                ErrorMessage = $"获取数据失败: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
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
    }
}

public class AiResponse
{
    [JsonPropertyName("ai_response")] public string Response { get; set; } = string.Empty;
}

public class FoodAnalysisResult
{
    public string Guidance { get; set; } = string.Empty;
}