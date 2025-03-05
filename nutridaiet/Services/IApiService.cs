using System;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using nutridaiet.ViewModels;

namespace nutridaiet.Services;

public interface IApiService
{
    public Task<LoginResponse> LoginAsync(string email, string password);
    Task<ApiResponse> SendVerificationCodeAsync(string email);
    Task<ApiResponse> RegisterAsync(string username, string email, string password, string code);
}

public class LoginResponse
{
    [JsonPropertyName("access_token")] public string AccessToken { get; set; } = string.Empty;
    [JsonPropertyName("code")] public int Code { get; set; }
    public string Message { get; set; } = string.Empty;
    public bool Success { get; set; }
    [JsonPropertyName("user")] public User User { get; set; } = new User();
}

public class User
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("email")] public string Email { get; set; } = string.Empty;
    [JsonPropertyName("username")] public string Username { get; set; } = string.Empty;
    [JsonPropertyName("created_at")] public DateTime CreateTime { get; set; }
}