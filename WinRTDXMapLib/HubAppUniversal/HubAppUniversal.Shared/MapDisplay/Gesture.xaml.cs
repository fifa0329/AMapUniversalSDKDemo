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
    public sealed partial class Gesture : Page
    {
        private readonly AMapControl aMapControl;

        public Gesture()
        {
            this.InitializeComponent();
            aMapControl = new AMapControl();
            aMapControl.AllGesturesEnabled = false;


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






        private void Move_OnChecked(object sender, RoutedEventArgs e)
        {
            aMapControl.ScrollGestureEnabled = true;

        }

        private void Move_OnUnchecked(object sender, RoutedEventArgs e)
        {
            aMapControl.ScrollGestureEnabled = false;

        }

        private void Rotate_OnChecked(object sender, RoutedEventArgs e)
        {
            aMapControl.RotateGestureEnabled = true;

        }

        private void Rotate_OnUnchecked(object sender, RoutedEventArgs e)
        {
            aMapControl.RotateGestureEnabled = false;

        }

        private void Tilt_OnChecked(object sender, RoutedEventArgs e)
        {
            aMapControl.TiltGestureEnabled = true;
        }

        private void Tilt_OnUnchecked(object sender, RoutedEventArgs e)
        {
            aMapControl.TiltGestureEnabled = false;
        }

        private void Zoom_OnUnchecked(object sender, RoutedEventArgs e)
        {
            aMapControl.ZoomGestureEnabled = false;
        }

        private void Zoom_OnChecked(object sender, RoutedEventArgs e)
        {
            aMapControl.ZoomGestureEnabled = true;
        }
    }
}
