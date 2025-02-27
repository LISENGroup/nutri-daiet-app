using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace nutridaiet.Services;

public class ApiService: IApiService
{
    private readonly HttpClient _httpClient;

    public ApiService()
    {
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
    }

    public async Task<LoginResponse> LoginAsync(string email, string password)
    {
        var request = new
        {
            email,
            password
        };

        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(
            "https://lively-rich-tadpole.ngrok-free.app/api/auth/login",
            content
        );

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"登录失败: {response.StatusCode}");
        }

        var responseJson = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<LoginResponse>(responseJson)!;
    } 
}