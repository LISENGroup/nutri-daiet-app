using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.ObjectModel;

namespace nutridaiet.ViewModels;

public partial class MainWindowViewModel : ViewModelBase, IRecipient<NavigateToMessage>
{
    private readonly IStorageProvider _storageProvider;

    [ObservableProperty] private ViewModelBase _currentPage;
    [ObservableProperty] private ListItemTemplate? _selectedListItem;

    public MainWindowViewModel(IStorageProvider storageProvider)
    {
        _storageProvider = storageProvider;
        _currentPage = new HomeViewModel(_storageProvider);
        WeakReferenceMessenger.Default.Register<NavigateToMessage>(this);

        Items = new ObservableCollection<ListItemTemplate>
        {
            new ListItemTemplate(typeof(HomeViewModel), "/Assets/home.png", "首页"),
            new ListItemTemplate(typeof(ShopViewModel), "/Assets/shop.png", "购物"),
            new ListItemTemplate(typeof(NotificationViewModel), "/Assets/notification.png", "科普"),
            new ListItemTemplate(typeof(ProfileViewModel), "/Assets/profile.png", "我"),
        };
    }

    public ObservableCollection<ListItemTemplate> Items { get; }

    public void Receive(NavigateToMessage message)
    {
        var instance = CreateViewModel(message.DestinationViewModel);
        if (instance != null)
        {
            CurrentPage = instance;
        }
    }

    partial void OnSelectedListItemChanged(ListItemTemplate? value)
    {
        if (value is null) return;
        var instance = CreateViewModel(value.ModelType);
        if (instance != null)
        {
            CurrentPage = instance;
        }
    }

    private ViewModelBase? CreateViewModel(Type viewModelType)
    {
        if (Activator.CreateInstance(viewModelType, _storageProvider) is ViewModelBase viewModel)
        {
            return viewModel;
        }
        return null;
    }
}

public class ListItemTemplate
{
    public ListItemTemplate(Type type, string iconPath, string label)
    {
        ModelType = type;
        IconPath = iconPath;
        Label = label;
    }

    public string Label { get; }
    public Type ModelType { get; }
    public string IconPath { get; }
}

