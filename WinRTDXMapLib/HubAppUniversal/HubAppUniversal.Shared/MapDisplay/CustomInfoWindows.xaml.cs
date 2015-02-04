// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556
using System;
using Windows.Foundation;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Com.AMap.Api.Maps;
using Com.AMap.Api.Maps.Model;
using Com.AMap.Api.Maps.OverLay;
using OfficalDemo.MapDisplay;

namespace HubAppUniversal.MapDisplay
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CustomInfoWindows : Page
    {
        private readonly AMapControl aMapControl = new AMapControl();

        public CustomInfoWindows()
        {
            this.InitializeComponent();

            ContentGrid.Children.Add(aMapControl);
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var layer1 = new AMapLayer();
            var layer2 = new AMapLayer();
            var layer3 = new AMapLayer();
            double interval = 0.1;

            var icon1 = new AMapIcon
            {
                Image = RandomAccessStreamReference.CreateFromUri(new Uri(
                    "ms-appx:///Assets/YELLOW.png", UriKind.RelativeOrAbsolute)),
                Location = aMapControl.Center,
                Data = "1"
            };

            icon1.Tapped += icon1_Tapped;

            var icon2 = new AMapIcon
            {
                Image = RandomAccessStreamReference.CreateFromUri(new Uri(
                    "ms-appx:///Assets/CYAN.png", UriKind.RelativeOrAbsolute)),
                Location = new LngLat(icon1.Location.Longitude + interval, icon1.Location.Latitude + interval),
                Data = "2"
            };

            icon2.Tapped += icon2_Tapped;

            var icon3 = new AMapIcon
            {
                Image = RandomAccessStreamReference.CreateFromUri(new Uri(
                    "ms-appx:///Assets/GREEN.png", UriKind.RelativeOrAbsolute)),
                Location = new LngLat(icon2.Location.Longitude + interval, icon2.Location.Latitude + interval),
                Data = "3"
            };

            icon3.Tapped += icon3_Tapped;

            layer1.Add(icon1);
            layer2.Add(icon2);
            layer3.Add(icon3);


            aMapControl.Layers.Add(layer1);
            aMapControl.Layers.Add(layer2);
            aMapControl.Layers.Add(layer3);


            aMapControl.MapHolding += aMapControl_Holding;
        }

        private void aMapControl_Holding(AMapControl sender, AMapInputEventArgs args)
        {
            aMapControl.HideInfoWindow();

        }

        void icon3_Tapped(object sender, AMapIcon e)
        {
            e.ShowInfoWindow(new InfoWindowControl(Colors.Red, e.Data.ToString()), new Point(0.5, 1), 20);
        }

        void icon2_Tapped(object sender, AMapIcon e)
        {
            e.ShowInfoWindow(new InfoWindowControl(Colors.Green, e.Data.ToString()), new Point(1, 1), 0);
        }

        void icon1_Tapped(object sender, AMapIcon e)
        {
            e.ShowInfoWindow(new InfoWindowControl(Colors.Blue, e.Data.ToString()));
        }
    }
}
