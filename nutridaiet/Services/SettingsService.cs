using Avalonia.Platform.Storage;
using System;
using System.IO;
using System.Text.Json;
using nutridaiet.Models;

namespace nutridaiet.Services;

public class SettingsService : ISettingsService
{
    private static readonly string SettingsPath = GetSettingsPath();

    private static string GetSettingsPath()
    {
        if (OperatingSystem.IsAndroid())
        {
            // 安卓平台使用文件系统的应用私有存储目录
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "nutridaiet",
                "settings.json");
        }
        else
        {
            // 桌面平台使用 ApplicationData
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "nutridaiet",
                "settings.json");
        }
    }

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
        catch (Exception ex)
        {
            Console.WriteLine($"加载配置失败: {ex.Message}");
            throw;
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