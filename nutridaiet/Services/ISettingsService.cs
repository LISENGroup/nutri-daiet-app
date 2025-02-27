using nutridaiet.Models;

namespace nutridaiet.Services;

public interface ISettingsService
{
    
    Settings LoadSettings();
    public void SaveSettings(Settings settings);
}
