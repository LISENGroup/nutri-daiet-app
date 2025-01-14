using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace nutridaiet.Services;

public class FoodAnalysisService : IFoodAnalysisService
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "http://8.210.33.109";

    public FoodAnalysisService()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(BaseUrl)
        };
    }

    public async Task<FoodAnalysisResponse> AnalyzeFood(FoodAnalysisRequest request)
    {
        var formData = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("food_name", request.FoodName),
            new KeyValuePair<string, string>("user_desc", request.UserDesc),
        });

        var response = await _httpClient.PostAsync("/ai/response", formData);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<FoodAnalysisResponse>();
    }
}