// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Com.AMap.Api.Maps;
using Com.AMap.Api.Maps.Model;

namespace HubAppUniversal.MapDisplay
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CustomDisplay : Page
    {

        private readonly AMapControl aMapControl = new AMapControl();

        public CustomDisplay()
        {
            this.InitializeComponent();

            aMapControl.LandmarksVisible = false;
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

        private void Traffic_OnChecked(object sender, RoutedEventArgs e)
        {
            aMapControl.TrafficFlowVisible = true;
        }

        private void Traffic_OnUnchecked(object sender, RoutedEventArgs e)
        {
            aMapControl.TrafficFlowVisible = false;
        }

        private void ShowBuilding_OnChecked(object sender, RoutedEventArgs e)
        {
            aMapControl.LandmarksVisible = true;
        }

        private void ShowBuilding_OnUnchecked(object sender, RoutedEventArgs e)
        {
            aMapControl.LandmarksVisible = false;
        }

        private void Maptype_OnChecked(object sender, RoutedEventArgs e)
        {
            aMapControl.MapType = MapType.Aerial;
        }

        private void Maptype_OnUnchecked(object sender, RoutedEventArgs e)
        {
            aMapControl.MapType = MapType.Road;
        }
    }
}
