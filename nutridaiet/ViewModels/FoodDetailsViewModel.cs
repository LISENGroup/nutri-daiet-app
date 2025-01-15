using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using nutridaiet.Models;

namespace nutridaiet.ViewModels
{
    public partial class FoodDetailsViewModel : ViewModelBase
    {
        [ObservableProperty]
        private FoodDetails? _foodDetails;

        public FoodDetailsViewModel()
        {
            // Sample data - in real app this would come from your service
            FoodDetails = new FoodDetails
            {
                Name = "苹果",
                ImagePath = "/Assets/apple.jpg",
                NutritionalInfo = "维生素A/C/E: 含量丰富，有助于提高免疫功能",
                VitaminContent = new List<string> 
                { 
                    "维生素A/C/E: 含量丰富，有助于提高免疫功能",
                    "钾和膳食纤维: 对心脏健康有益，可以维持正常的血压水"
                },
                HealthBenefits = new List<string>
                {
                    "抗氧化物质: 如类黄酮等，可能帮助减少心脏病风险",
                    "低升糖指数: 平稳的血糖反应（36），说明它不会快速提升血糖水平"
                },
                DietarySuggestions = "建议每天食用1-2个苹果，特别是在餐后食用更佳"
            };
        }
    }
}