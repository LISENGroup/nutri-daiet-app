using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net;
using nutridaiet.ViewModels;

namespace nutridaiet.Services;

public class ApiService : IApiService
{
    private readonly HttpClient _httpClient;

    public ApiService()
    {
        // 配置支持自动解压的Handler
        var handler = new HttpClientHandler
        {
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
        };

        _httpClient = new HttpClient(handler);

        // 更精确的Header配置
        _httpClient.DefaultRequestHeaders.Accept.ParseAdd("application/json");
        _httpClient.DefaultRequestHeaders.Connection.ParseAdd("keep-alive");
        _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("nutridaiet/1.0");
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
            var errorContent = await response.Content.ReadAsStringAsync();
            if (errorContent ==
                "{\"message\":\"\\u627e\\u4e0d\\u5230\\u7528\\u6237\\u6216\\u8005\\u5bc6\\u7801\\u9a8c\\u8bc1\\u5931\\u8d25\"}\n")
            {
                errorContent = "找不到用户或者密码验证失败";
            }

            throw new HttpRequestException($"{response.StatusCode} - {errorContent}");
        }

        var responseJson = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<LoginResponse>(responseJson)!;
    }
    
    public async Task<ApiResponse> SendVerificationCodeAsync(string email)
    {
        var request = new { email };
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("https://lively-rich-tadpole.ngrok-free.app/api/auth/captcha/email", content);
        var responseJson = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"验证码请求失败: {response.StatusCode}, {responseJson}");
        }
        return JsonSerializer.Deserialize<ApiResponse>(responseJson)!;
    }
    
    public async Task<ApiResponse> RegisterAsync(string username, string email, string password, string captcha)
    {
        var request = new { username, email, password, captcha };
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("https://lively-rich-tadpole.ngrok-free.app/api/auth/register", content);
        var responseJson = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"注册请求失败: {response.StatusCode}, {responseJson}");
        }
        return JsonSerializer.Deserialize<ApiResponse>(responseJson)!;
    } 
}