using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace nutridaiet.Views
{
    public partial class FoodDetailsView : UserControl
    {
        public FoodDetailsView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}