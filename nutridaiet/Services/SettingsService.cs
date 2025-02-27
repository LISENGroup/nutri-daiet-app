using Avalonia.Platform.Storage;
using System;
using System.IO;
using System.Text.Json;
using nutridaiet.Models;

namespace nutridaiet.Services;

public class SettingsService: ISettingsService
{
    private static readonly string SettingsPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "nutridaiet",
        "settings.json"
    );

    public Settings LoadSettings()
    {
        try
        {
            if (File.Exists(SettingsPath))
            {
                var json = File.ReadAllText(SettingsPath);
                return JsonSerializer.Deserialize<Settings>(json)!;
            }
        }
        catch
        {
            // 忽略读取错误
        }
        return new Settings();
    }
    
    public void SaveSettings(Settings settings)
    {
        var directory = Path.GetDirectoryName(SettingsPath)!;
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
        var json = JsonSerializer.Serialize(settings);
        File.WriteAllText(SettingsPath, json);
    }
    
}