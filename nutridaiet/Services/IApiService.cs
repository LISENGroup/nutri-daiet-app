using System.Threading.Tasks;
using nutridaiet.Models;

namespace nutridaiet.Services;

public interface IApiService
{
    public Task<LoginResponse> LoginAsync(string email, string password);
}

public class LoginResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public int Code { get; set; }
    public string Message { get; set; } = string.Empty;
    public bool Success { get; set; }
    public User User { get; set; } = new User();
}

public class User
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
}