// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Com.AMap.Api.Maps;
using Com.AMap.Api.Maps.Model;
using Com.AMap.Api.Maps.OverLay;

namespace OfficalDemo.MapDisplay
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class OverlayManagement : Page
    {
        private readonly AMapControl aMapControl = new AMapControl();
        private readonly AMapLayer baseLayer = new AMapLayer();
        private AMapGroundOverlay aMapGroundOverlay;
        private AMapIcon aMapIcon;
        private AMapPolyline polyline;
        private AMapCircle circle;
        private AMapPolygon polygon;

        public OverlayManagement()
        {
            InitializeComponent();
            ContentGrid.Children.Add(aMapControl);
            aMapControl.Layers.Add(baseLayer);
        }

        /// <summary>
        ///     Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">
        ///     Event data that describes how this page was reached.
        ///     This parameter is typically used to configure the page.
        /// </param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

        }

        private void MapIcon_Click(object sender, RoutedEventArgs e)
        {
            aMapIcon = new AMapIcon
            {
                Image =
                    RandomAccessStreamReference.CreateFromUri(new Uri(
                        "ms-appx:///Assets/AZURE.png", UriKind.RelativeOrAbsolute)),
                Location = aMapControl.Center,
                NormalizedAnchorPoint = new Point(0.5, 1),
            };

            baseLayer.Add(aMapIcon);
        }

        private void Polyline_Click(object sender, RoutedEventArgs e)
        {

            var lngLat1 = new LngLat(aMapControl.Center.Longitude - 0.06, aMapControl.Center.Longitude + 0.06);

            var lngLat2 = aMapControl.Center;

            var lngLat3 = new LngLat(aMapControl.Center.Longitude + 0.06, aMapControl.Center.Latitude + 0.06);

            var list = new List<LngLat> { lngLat1, lngLat2, lngLat3 };

            polyline = new AMapPolyline
            {
                Path = new AGeopath(list),
                StrokeColor = Colors.Red,
                Cap = Cap.BUTT,
                Joint = Joint.MITER,
                StrokeThickness = 10
            };


            baseLayer.Add(polyline);
        }

        private void Circle_Click(object sender, RoutedEventArgs e)
        {
            Color c = new Color { R = 0, G = 0, B = 255, A = 255 / 2 };

            circle = new AMapCircle
            {
                Center = aMapControl.Center,
                FillColor = c,
                Radius = 5000,
                StrokeColor = Colors.Red,
                StrokeThickness = 5
            };


            baseLayer.Add(circle);
        }

        private void Polygon_OnClick(object sender, RoutedEventArgs e)
        {
            double interval = 0.02;
            List<LngLat> lnglats = new List<LngLat>();
            lnglats.Add(new LngLat(aMapControl.Center.Longitude - interval, aMapControl.Center.Latitude + interval));
            lnglats.Add(new LngLat(aMapControl.Center.Longitude + interval, aMapControl.Center.Latitude + interval));
            lnglats.Add(new LngLat(aMapControl.Center.Longitude + interval, aMapControl.Center.Latitude - interval));
            lnglats.Add(aMapControl.Center);
            lnglats.Add(new LngLat(aMapControl.Center.Longitude - interval, aMapControl.Center.Latitude - interval));

            polygon = new AMapPolygon
            {
                FillColor = Colors.Red,
                Path = new AGeopath(lnglats),
                StrokeColor = Colors.Blue,
                Joint = Joint.CLIPPED,
                StrokeThickness = 10
            };

            baseLayer.Add(polygon);
        }

        private void GroundOverlay_OnClick(object sender, RoutedEventArgs e)
        {
            aMapControl.TrySetViewBoundsAsync(
                new LngLatBoundingBox(new LngLat(116.384377, 39.935029), new LngLat(116.388331, 39.939577)),
                AMapAnimationKind.Default);
            aMapGroundOverlay = new AMapGroundOverlay
            {
                Bound = new LngLatBoundingBox(new LngLat(116.384377, 39.935029), new LngLat(116.388331, 39.939577)),
                Image = RandomAccessStreamReference.CreateFromUri(new Uri(
                    "ms-appx:///Assets/2.png", UriKind.RelativeOrAbsolute))
            };

            baseLayer.Add(aMapGroundOverlay);
        }

        private void HideRandom_Click(object sender, RoutedEventArgs e)
        {
            var list = baseLayer.OverLays;
            IAMapOverLay[] overlays = list.ToArray();
            Random rnd = new Random();
            if (overlays.Length == 0)
            {
                return;
            }
            int radomIndex = rnd.Next(overlays.Length - 1); // creates a number between 1 and 12

            overlays[radomIndex].Visible = !overlays[radomIndex].Visible;
        }

        private void RemoveRandom_Click(object sender, RoutedEventArgs e)
        {
            var list = baseLayer.OverLays;
            IAMapOverLay[] overlays = list.ToArray();
            Random rnd = new Random();
            if (overlays.Length == 0)
            {
                return;
            }
            int radomIndex = rnd.Next(overlays.Length - 1);

            baseLayer.Remove(overlays[radomIndex]);
        }

        private void ClearAll_Click(object sender, RoutedEventArgs e)
        {
            baseLayer.Clear();
        }


        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
             
        }
    }
}