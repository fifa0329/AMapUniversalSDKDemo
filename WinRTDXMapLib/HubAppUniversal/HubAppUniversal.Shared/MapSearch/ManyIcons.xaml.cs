// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

using System;
using Windows.ApplicationModel.Store;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Com.AMap.Api.Maps;
using Com.AMap.Api.Maps.Model;
using Com.AMap.Api.Maps.OverLay;

namespace HubAppUniversal.MapSearch
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ManyIcons : Page
    {
        private readonly AMapControl aMapControl = new AMapControl();
        private readonly AMapLayer baseLayer = new AMapLayer();

        public ManyIcons()
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

        /// <summary>
        /// todo 要告诉开发者怎么样用快，怎么样用慢
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ExistedLayerAdd(object sender, RoutedEventArgs e)
        {

            RandomAccessStreamReference rsa =
                RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/man.png",
                    UriKind.RelativeOrAbsolute));

            for (int i = 0; i < 1000; i++)
            {
                var icon = new AMapIcon
                {
                    //Image =
                    //    RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/man.png",
                    //        UriKind.RelativeOrAbsolute)),

                    Image = rsa,
                    Location = new LngLat(116.394653 + i*0.01, 39.911843 + i*0.01),
                };

                await baseLayer.Add(icon);
            }
        }

        private async void AddLayerContainIcons(object sender, RoutedEventArgs e)
        {

            var layer = new AMapLayer();


            RandomAccessStreamReference rsa =
                RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/man.png",
                    UriKind.RelativeOrAbsolute));

            for (int i = 0; i < 1000; i++)
            {
                var icon = new AMapIcon
                {
                    //Image =
                    //    RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/man.png",
                    //        UriKind.RelativeOrAbsolute)),

                    Image = rsa,
                    Location = new LngLat(116.394653 + i*0.01, 39.911843 + i*0.01),
                };

                await layer.Add(icon);

            }
            aMapControl.Layers.Add(layer);
        }

        private async void GetAppid(object sender, RoutedEventArgs e)
        {
            Guid guid = CurrentApp.AppId;
            string productID = guid.ToString();
            MessageDialog msgbox = new MessageDialog(productID);
            await msgbox.ShowAsync();   
        }
    }
}