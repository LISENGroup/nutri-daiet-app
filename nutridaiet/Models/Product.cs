using System.Text.Json.Serialization;

namespace nutridaiet.Models;

public class Product
{
    [JsonPropertyName("name")] public string Name { get; set; } = string.Empty;
    [JsonPropertyName("description")] public string Description { get; set; } = string.Empty;

    [JsonPropertyName("price")] public decimal Price { get; set; }
    [JsonPropertyName("imageurl")] public string ImageUrl { get; set; } = "";
}