using System.Collections.Generic;

namespace nutridaiet.Models
{
    public class FoodDetails
    {
        public string Name { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;
        public string NutritionalInfo { get; set; } = string.Empty;
        public List<string> VitaminContent { get; set; } = new();
        public List<string> HealthBenefits { get; set; } = new();
        public string DietarySuggestions { get; set; } = string.Empty;
    }
}