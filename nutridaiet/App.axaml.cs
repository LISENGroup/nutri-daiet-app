using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.SimpleRouter;
using Microsoft.Extensions.DependencyInjection;
using nutridaiet.Services;
using nutridaiet.ViewModels;
using nutridaiet.Views;

namespace nutridaiet;

public partial class App : Application
{
    public static TopLevel TopLevel { get; private set; }

    public static MainViewModel MainViewModel { get; private set; }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // In this example we use Microsoft DependencyInjection (instead of ReactiveUI / Splat)
        // Splat would also work, just use the according methods
        IServiceProvider services = ConfigureServices();
        var mainViewModel = services.GetRequiredService<MainViewModel>();
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = mainViewModel
            };
            TopLevel = TopLevel.GetTopLevel(desktop.MainWindow);    
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView
            {
                DataContext = mainViewModel
            };
            TopLevel = TopLevel.GetTopLevel(singleViewPlatform.MainView);
        }

        base.OnFrameworkInitializationCompleted();
    }


    private static ServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();
        // Add the HistoryRouter as a service
        services.AddSingleton<HistoryRouter<ViewModelBase>>(s =>
            new HistoryRouter<ViewModelBase>(t => (ViewModelBase)s.GetRequiredService(t)));
        
        services.AddSingleton<ISettingsService, SettingsService>();  // 配置文件服务
        services.AddTransient<IApiService, ApiService>();           // API 服务

        // Add the ViewModels as a service (Main as singleton, others as transient)
        services.AddSingleton<MainViewModel>();
        services.AddTransient<HomeViewModel>();
        services.AddTransient<ShopViewModel>();
        services.AddTransient<NotificationViewModel>();
        services.AddTransient<ProfileViewModel>();
        services.AddTransient<FoodDetailsViewModel>();
        services.AddTransient<WelcomeViewModel>();
        services.AddTransient<LoginViewModel>();
        return services.BuildServiceProvider();
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}