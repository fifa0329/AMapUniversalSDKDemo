// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Com.AMap.Api.Maps;

namespace HubAppUniversal.MapDisplay
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ForeGroundControls : Page
    {
        private readonly AMapControl aMapControl;
        public ForeGroundControls()
        {
            this.InitializeComponent();
            aMapControl = new AMapControl();
            aMapControl.ScaleControlEnabled = false;
            aMapControl.ZoomControlEnabled = false;
            aMapControl.CompassControlEnabled = false;

            ContentGrid.Children.Add(aMapControl);
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void ZoomBar_OnChecked(object sender, RoutedEventArgs e)
        {
            aMapControl.ZoomControlEnabled = true;
        }

        private void ZoomBar_OnUnchecked(object sender, RoutedEventArgs e)
        {
            aMapControl.ZoomControlEnabled = false;

        }

        private void Compass_OnChecked(object sender, RoutedEventArgs e)
        {
            aMapControl.CompassControlEnabled = true;
        }

        private void Compass_OnUnchecked(object sender, RoutedEventArgs e)
        {
            aMapControl.CompassControlEnabled = false;

        }

        private void Scale_OnChecked(object sender, RoutedEventArgs e)
        {
            aMapControl.ScaleControlEnabled = true;

        }

        private void Scale_OnUnchecked(object sender, RoutedEventArgs e)
        {
            aMapControl.ScaleControlEnabled = false;

        }
    }
}
