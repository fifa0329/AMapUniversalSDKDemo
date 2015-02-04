using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace HubAppUniversal.MapDisplay
{
    public sealed partial class InfoWindowControl : UserControl
    {

        public InfoWindowControl(Color color, string text)
        {
            this.InitializeComponent();

            ColorGrid.Background = new SolidColorBrush(color);
            textBlock.Text = text;
        }
    }
}
