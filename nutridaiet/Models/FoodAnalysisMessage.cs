using CommunityToolkit.Mvvm.Messaging.Messages;

namespace nutridaiet.Models;

public class FoodAnalysisMessage : ValueChangedMessage<(string FoodName, string ImagePath)>
{
    public FoodAnalysisMessage((string FoodName, string ImagePath) value) : base(value) { }
}
