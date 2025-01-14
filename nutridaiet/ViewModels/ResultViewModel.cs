using CommunityToolkit.Mvvm.ComponentModel;

namespace nutridaiet.ViewModels;

public partial class ResultViewModel : ObservableObject
{
    [ObservableProperty]
    private string aiResponse;

    public ResultViewModel(string response)
    {
        AiResponse = response;
    }
}