using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using nutridaiet.ViewModels;
using nutridaiet.Views;
using System.Linq;
using Avalonia.Controls;

namespace nutridaiet;

public partial class App : Application
{
    public static TopLevel TopLevel { get; private set; }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            DisableAvaloniaDataAnnotationValidation();
            var mainWindow = new MainWindow();
            desktop.MainWindow = mainWindow;
            TopLevel = TopLevel.GetTopLevel(mainWindow);
            mainWindow.DataContext = new MainWindowViewModel(TopLevel.StorageProvider);
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            var mainView = new MainWindow();
            singleViewPlatform.MainView = mainView;
            TopLevel = TopLevel.GetTopLevel(mainView);
            mainView.DataContext = new MainWindowViewModel(TopLevel.StorageProvider);
        }

        base.OnFrameworkInitializationCompleted();
    }


    private void DisableAvaloniaDataAnnotationValidation()
    {
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}