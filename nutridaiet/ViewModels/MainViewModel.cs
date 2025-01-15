using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace nutridaiet.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        [ObservableProperty]
        private ViewModelBase _currentViewModel;

        public MainViewModel()
        {
            _currentViewModel = this;
            UploadCommand = new AsyncRelayCommand(UploadImageAsync);
        }

        public IAsyncRelayCommand UploadCommand { get; }

        private async Task UploadImageAsync()
        {
            var topLevel = TopLevel.GetTopLevel(App.TopLevel);
            if (topLevel != null)
            {
                var storageProvider = topLevel.StorageProvider;
                if (storageProvider != null)
                {
                    var options = new FilePickerOpenOptions
                    {
                        AllowMultiple = false,
                        FileTypeFilter = new List<FilePickerFileType>
                        {
                            new FilePickerFileType("Images")
                            {
                                Patterns = new[] { "*.png", "*.jpg", "*.jpeg" }
                            }
                        }
                    };

                    var result = await storageProvider.OpenFilePickerAsync(options);
                    if (result != null && result.Count > 0)
                    {
                        // Navigate to details view
                        CurrentViewModel = new FoodDetailsViewModel();
                    }
                }
            }
        }
    }
}