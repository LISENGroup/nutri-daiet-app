using System.Threading.Tasks;

namespace nutridaiet.Services;
public interface IFoodAnalysisService
{
    Task<FoodAnalysisResponse> AnalyzeFood(FoodAnalysisRequest request);
}