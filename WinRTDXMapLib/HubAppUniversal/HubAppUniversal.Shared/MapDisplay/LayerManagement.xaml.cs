// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556
using System;
using System.Linq;
using Windows.Storage.Streams;
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
    public sealed partial class LayerManagement : Page
    {
        private readonly AMapControl aMapControl = new AMapControl();
        private readonly AMapLayer baseLayer = new AMapLayer();

        public LayerManagement()
        {
            this.InitializeComponent();
            ContentGrid.Children.Add(aMapControl);
            aMapControl.Layers.Add(baseLayer);
            aMapControl.ZoomLevel = 9;

            var icon = new AMapIcon
            {
                Location = aMapControl.Center,
                Data = "1"
            };

            baseLayer.Add(icon);
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void ClearButton_OnClick(object sender, RoutedEventArgs e)
        {
            aMapControl.Layers.Clear();
        }


        /// <summary>
        ///     在不同的层上加入不同的marker
        /// 来测试层的功能是否实现
        /// </summary>
        private async void AddMarkersAtDifferentLayer()
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


            var icon2 = new AMapIcon
            {
                Image = RandomAccessStreamReference.CreateFromUri(new Uri(
                    "ms-appx:///Assets/CYAN.png", UriKind.RelativeOrAbsolute)),
                Location = new LngLat(icon1.Location.Longitude + interval, icon1.Location.Latitude + interval),
                Data = "2"
            };


            var icon3 = new AMapIcon
            {
                Image = RandomAccessStreamReference.CreateFromUri(new Uri(
                    "ms-appx:///Assets/GREEN.png", UriKind.RelativeOrAbsolute)),
                Location = new LngLat(icon2.Location.Longitude + interval, icon2.Location.Latitude + interval),
                Data = "3"
            };

            layer1.Add(icon1);
            layer2.Add(icon2);
            layer3.Add(icon3);


            aMapControl.Layers.Add(layer1);
            aMapControl.Layers.Add(layer2);
            aMapControl.Layers.Add(layer3);


        }


        private void Add_OnClick(object sender, RoutedEventArgs e)
        {
            AddMarkersAtDifferentLayer();
        }

        private void RemoveOneLayer_OnClick(object sender, RoutedEventArgs e)
        {
            AMapLayer[] layers = aMapControl.Layers.ToArray();
            Random random = new Random();

            if (layers.Length==0)
            {
                return;
            }

            int randomIndex = random.Next(layers.Length - 1);

            aMapControl.Layers.RemoveAt(randomIndex);
        }
    }
}
