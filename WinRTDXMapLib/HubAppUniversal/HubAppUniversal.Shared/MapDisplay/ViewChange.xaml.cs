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
    public sealed partial class ViewChange : Page
    {

        private LngLat beijing = new LngLat(116.334, 39.845);
        private LngLat shanghai = new LngLat(121.402, 31.079);
        private LngLat guangzhou = new LngLat(113.220, 23.011);
        private readonly AMapControl aMapControl = new AMapControl();


        public ViewChange()
        {
            this.InitializeComponent();
            ContentGrid.Children.Add(aMapControl);

            aMapControl.ZoomLevelChanged += AMapControlZoomLevelChanged;
            aMapControl.HeadingChanged += AMapControlHeadingChanged;
            aMapControl.PitchChanged += AMapControlPitchChanged;
            aMapControl.CenterChanged += AMapControlCenterChanged;
        }

        void AMapControlCenterChanged(AMapControl sender, object args)
        {
            center.Text = "center:" + aMapControl.Center;
        }

        void AMapControlPitchChanged(AMapControl sender, object args)
        {
            pitch.Text = "pitch:" + aMapControl.DesiredPitch;
        }

        void AMapControlHeadingChanged(AMapControl sender, object args)
        {
            heading.Text = "heading" + aMapControl.Heading;
        }

        void AMapControlZoomLevelChanged(AMapControl sender, object args)
        {
            zoomlevel.Text = "zoomlevel" + aMapControl.ZoomLevel;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }



        private void ToBeijing(object sender, RoutedEventArgs e)
        {
            aMapControl.TrySetViewAsync(beijing, 11, 45, 15, AMapAnimationKind.Default);
        }

        private void ToShanghai(object sender, RoutedEventArgs e)
        {
            aMapControl.TrySetViewAsync(shanghai, 10, 0, 0, AMapAnimationKind.None);
        }

        private void ToGuangzhouRegion(object sender, RoutedEventArgs e)
        {
            const double interval = 1;
            var guangzhouRegion = new LngLatBoundingBox(new LngLat(guangzhou.Longitude - interval, guangzhou.Latitude - interval), new LngLat(guangzhou.Longitude + interval, guangzhou.Latitude + interval));

            aMapControl.TrySetViewBoundsAsync(guangzhouRegion, AMapAnimationKind.Default);
        }
    }
}
