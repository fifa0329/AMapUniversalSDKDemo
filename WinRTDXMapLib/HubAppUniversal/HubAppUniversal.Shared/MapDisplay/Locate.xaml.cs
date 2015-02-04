// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556
using System;
using Windows.Foundation;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Com.AMap.Api.Maps;
using Com.AMap.Api.Maps.Model;
using Com.AMap.Api.Maps.OverLay;

namespace HubAppUniversal.MapDisplay
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Locate : Page
    {
        readonly AMapControl aMapControl = new AMapControl();
        private readonly AMapLayer baseLayer = new AMapLayer();
        private AMapIcon trackingIcon;
        private AMapGeolocator trackingGeolocator;
        private AMapCircle amapcircle;

        public Locate()
        {
            this.InitializeComponent();

            contentGrid.Children.Add(aMapControl);
            aMapControl.Layers.Add(baseLayer);
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        /// <summary>
        /// 注意：要开启定位权限
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void LocateClick(object sender, RoutedEventArgs e)
        {
            var aMapGeolocator = new AMapGeolocator();
            LngLat myLngLat = await aMapGeolocator.GetLngLatAsync();
            var icon = new AMapIcon { Location = myLngLat };
            baseLayer.Add(icon);
            await aMapControl.TrySetViewAsync(myLngLat, 15, 0, 0, AMapAnimationKind.Default);
        }

        private async void TrackLocationClick(object sender, RoutedEventArgs e)
        {

            MessageDialog messageDialog = new MessageDialog("启动tracking");
            await messageDialog.ShowAsync();

            trackingGeolocator = new AMapGeolocator();
            trackingGeolocator.PositionChanged += aMapGeolocator_PositionChanged;

            //模拟器不能获取经纬度
            var currentLocation = await trackingGeolocator.GetLngLatAsync();

            if (trackingIcon == null)
            {
                trackingIcon = new AMapIcon
                {
                    Image =
                        RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/marker_gps_no_sharing.png",
                            UriKind.RelativeOrAbsolute)),
                    Location = currentLocation,
                    NormalizedAnchorPoint = new Point(0.5, 0.5)
                };
            }

            if (amapcircle == null)
            {
                amapcircle = new AMapCircle
                {
                    FillColor = Color.FromArgb(80, 100, 150, 255),
                    StrokeThickness = 2,
                    StrokeColor = Color.FromArgb(80, 0, 0, 255),
                    Center = currentLocation,
                    Radius = 500
                };
            }

            bool circleContained = baseLayer.OverLays.Contains(amapcircle);
            bool trackingIconContained = baseLayer.OverLays.Contains(trackingIcon);
            if (!trackingIconContained)
                baseLayer.Add(trackingIcon);


            if (!circleContained)
                baseLayer.Add(amapcircle);


            aMapControl.TrySetViewAsync(currentLocation, 15, null, null, AMapAnimationKind.Default);
            trackingGeolocator.Start();
        }

        void aMapGeolocator_PositionChanged(AMapGeolocator sender, AMapPositionChangedEventArgs args)
        {
            trackingIcon.Location = args.LngLat;

            amapcircle.Center = args.LngLat;
            amapcircle.Radius = (float)args.Accuracy;



        }

        private async void StopTrack_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog messageDialog = new MessageDialog("停止tracking");
            await messageDialog.ShowAsync();
            trackingGeolocator.Stop();
            trackingGeolocator.PositionChanged -= aMapGeolocator_PositionChanged;
        }
    }
}
