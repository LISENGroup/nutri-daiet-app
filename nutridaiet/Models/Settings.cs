using System;
using System.Text.Json.Serialization;

namespace nutridaiet.Models;

public class Settings
{
    [JsonPropertyName("id")] public int UserId { get; set; }
    [JsonPropertyName("access_token")] public string AccessToken { get; set; } = string.Empty;

    [JsonPropertyName("username")] public string Username { get; set; } = string.Empty;
    [JsonPropertyName("email")] public string Email { get; set; } = string.Empty;

    [JsonPropertyName("created_at")] public DateTime CreateTime { get; set; }
}