using System;
using System.Diagnostics;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Avalonia.Platform.Storage;
using nutridaiet.Services;

namespace nutridaiet.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly IFoodAnalysisService _foodAnalysisService;
    private readonly IStorageProvider _storageProvider;
    private readonly NavigationStore _navigationStore;

    [ObservableProperty] private bool isLoading;

    public MainViewModel(
        IFoodAnalysisService foodAnalysisService,
        IStorageProvider storageProvider,
        NavigationStore navigationStore)
    {
        _foodAnalysisService = foodAnalysisService;
        _storageProvider = storageProvider;
        _navigationStore = navigationStore;
    }

    public MainViewModel()
    {
        throw new NotImplementedException();
    }

    [RelayCommand]
    private async Task UploadAsync()
    {
        try
        {
            var files = await _storageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "选择图片",
                AllowMultiple = false,
                FileTypeFilter = new[]
                {
                    new FilePickerFileType("Images")
                    {
                        Patterns = new[] { "*.jpg", "*.jpeg", "*.png" }
                    }
                }
            });

            if (files.Count > 0)
            {
                IsLoading = true;

                // For demo, we're using hardcoded values
                var request = new FoodAnalysisRequest
                {
                    FoodName = "苹果",
                    UserDesc = "28岁患有心脏病"
                };

                var response = await _foodAnalysisService.AnalyzeFood(request);

                // Navigate to results page with animation
                await _navigationStore.NavigateWithAnimationAsync(() =>
                    new ResultViewModel(response.AiResponse));
            }
        }
        catch (Exception ex)
        {
            // Handle error
            Debug.WriteLine(ex);
        }
        finally
        {
            IsLoading = false;
        }
    }
}