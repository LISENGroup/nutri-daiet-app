using System;

namespace nutridaiet.Models;

public class Settings
{
    public int UserId { get; set; }
    public string AccessToken { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime TokenExpiry { get; set; }
}