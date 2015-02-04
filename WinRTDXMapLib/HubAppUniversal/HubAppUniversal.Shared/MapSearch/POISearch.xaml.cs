// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Com.AMap.Api.Maps;
using Com.AMap.Api.Maps.Model;
using Com.AMap.Api.Maps.OverLay;
using Com.AMap.Api.Services;
using Com.AMap.Api.Services.Results;

namespace HubAppUniversal.MapSearch
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class POISearch : Page
    {

        private readonly AMapControl aMapControl = new AMapControl();
        private readonly AMapLayer baselayer = new AMapLayer();
        private List<AMapIcon> icons;

        public POISearch()
        {
            this.InitializeComponent();
            ContentGrid.Children.Add(aMapControl);
            aMapControl.Layers.Add(baselayer);
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private async void IdSearch_OnClick(object sender, RoutedEventArgs e)
        {
            baselayer.Clear();
            aMapControl.HideInfoWindow();

            string id = "B000A07060";


            AMapPOIResults results = await AMapPOISearch.POIID(id);
            if (results.Erro == null)
            {
                if (results.POIList == null || results.POIList.Count == 0)
                {
                    Debug.WriteLine("无查询结果");
                    return;
                }

                foreach (AMapPOI item in results.POIList)
                {

                    var icon = new AMapIcon { Location = new LngLat(item.Location.Lon, item.Location.Lat) };

                    icon.Tapped += icon_Tapped;

                    icon.Data = item.Address;

                    await baselayer.Add(icon);
                }

                aMapControl.TrySetViewAsync(
                    new LngLat(results.POIList[0].Location.Lon, results.POIList[0].Location.Lat), null, null, null,
                    AMapAnimationKind.Default);
            }
            else
            {
                Debug.WriteLine(results.Erro.Message);
            }

        }

        void icon_Tapped(object sender, AMapIcon e)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Text = e.Data.ToString();
            textBlock.FontSize = 20;
            textBlock.Foreground = new SolidColorBrush(Colors.Black);

            Grid info = new Grid();
            info.Width = 100;
            info.Height = 100;
            info.Background = new SolidColorBrush(Colors.White);
            info.Children.Add(textBlock);

            e.ShowInfoWindow(info);
        }

        private async void KeywordSearch_OnClick(object sender, RoutedEventArgs e)
        {
            baselayer.Clear();
            aMapControl.HideInfoWindow();

            bool groupbuy = false;
            bool discount = false;

            string keywords = "肯德基";

            string types = "快餐厅";

            string city = "北京";

            AMapFilterOption aMapFilterOption = new AMapFilterOption { Groupbuy = groupbuy, Discount = discount };

            AMapPOIResults results = await AMapPOISearch.POIKeyWords(keywords, types, aMapFilterOption, 0, 20, 1, Extensions.All, city);
            if (results.Erro == null)
            {

                if (results.POIList == null || results.POIList.Count == 0)
                {
                    Debug.WriteLine("无查询结果");
                    return;
                }

                IEnumerable<AMapPOI> pois = results.POIList;
                List<LngLat> lngLats = new List<LngLat>();
                foreach (AMapPOI item in pois)
                {
                    lngLats.Add(new LngLat(item.Location.Lon, item.Location.Lat));

                    var icon = new AMapIcon { Location = new LngLat(item.Location.Lon, item.Location.Lat) };

                    icon.Tapped += icon_Tapped;

                    icon.Data = item.Address;

                    await baselayer.Add(icon);
                }

                LngLatBoundingBoxBuilder builder = new LngLatBoundingBoxBuilder(lngLats);

                aMapControl.TrySetViewBoundsAsync(builder.Build(), AMapAnimationKind.Default);
            }
            else
            {
                Debug.WriteLine(results.Erro.Message);
            }


        }

        private async void NearbySearch_OnClick(object sender, RoutedEventArgs e)
        {
            baselayer.Clear();
            aMapControl.HideInfoWindow();


            LngLat currentLocation = await (new AMapGeolocator()).GetLngLatAsync();


            double centerX = currentLocation.Longitude;
            double centerY = currentLocation.Latitude;

            string keywords = "肯德基";

            string types = "快餐厅";

            uint radius = 500;

            string city = "北京";


            AMapPOIResults result = await AMapPOISearch.POIAround(centerX, centerY, keywords, types, null, radius, 0, 20, 1, Extensions.All, city);

            if (result.Erro == null)
            {
                if (result.POIList == null || result.POIList.Count == 0)
                {
                    Debug.WriteLine("无查询结果");
                    return;
                }
                IEnumerable<AMapPOI> pois = result.POIList;

                foreach (AMapPOI item in pois)
                {

                    var icon = new AMapIcon { Location = new LngLat(item.Location.Lon, item.Location.Lat) };

                    icon.Tapped += icon_Tapped;

                    icon.Data = item.Address;

                    await baselayer.Add(icon);
                }

                aMapControl.TrySetViewAsync(currentLocation, 13, 0, 0, AMapAnimationKind.Default);

            }
            else
            {
                Debug.WriteLine(result.Erro.Message);

            }

        }

        private async void PolygonSearch_OnClick(object sender, RoutedEventArgs e)
        {
            baselayer.Clear();
            aMapControl.HideInfoWindow();


            string keywords = "肯德基";
            string types = "快餐厅";
            uint offset = 50;
            string city = "北京";

            var lngLats = new List<LngLat>
            {
                new LngLat(aMapControl.Center.Longitude + 0.03, aMapControl.Center.Latitude + 0.02),
                new LngLat(aMapControl.Center.Longitude - 0.03, aMapControl.Center.Latitude + 0.03),
                new LngLat(aMapControl.Center.Longitude - 0.03, aMapControl.Center.Latitude - 0.026),
                new LngLat(aMapControl.Center.Longitude + 0.035, aMapControl.Center.Latitude - 0.04),
                new LngLat(aMapControl.Center.Longitude + 0.045, aMapControl.Center.Latitude - 0.046)
            };

            var polygon = new AMapPolygon
            {
                FillColor = Color.FromArgb(30, 255, 0, 255),
                Path = new AGeopath(lngLats),
                StrokeColor = Color.FromArgb(255, 102, 136, 255),
                StrokeThickness = 2
            };

            await baselayer.Add(polygon);


            SearchPolygon searchpolygon = new SearchPolygon();

            icons = new List<AMapIcon>();
            //多边形
            searchpolygon.Points = new List<AMapLocation>
            {
                new AMapLocation() {Lon = lngLats[0].Longitude, Lat = lngLats[0].Latitude},
                new AMapLocation() {Lon = lngLats[1].Longitude, Lat = lngLats[1].Latitude},
                new AMapLocation() {Lon = lngLats[2].Longitude, Lat = lngLats[2].Latitude},
                new AMapLocation() {Lon = lngLats[3].Longitude, Lat = lngLats[3].Latitude},
                new AMapLocation() {Lon = lngLats[4].Longitude, Lat = lngLats[4].Latitude}
            };

            AMapPOIResults result = await AMapPOISearch.POIPolygon(keywords, types, searchpolygon, null, 0, offset, 1, Extensions.All, city);

            if (result.Erro == null)
            {
                if (result.POIList == null || result.POIList.Count == 0)
                {
                    Debug.WriteLine("无查询结果");
                    return;
                }
                IEnumerable<AMapPOI> pois = result.POIList;
                foreach (AMapPOI item in pois)
                {

                    var icon = new AMapIcon { Location = new LngLat(item.Location.Lon, item.Location.Lat) };

                    icon.Tapped += icon_Tapped;

                    icon.Data = item.Address;

                    await baselayer.Add(icon); ;
                }

                LngLatBoundingBoxBuilder builder = new LngLatBoundingBoxBuilder(lngLats);


                aMapControl.TrySetViewBoundsAsync(builder.Build(), AMapAnimationKind.Default);

            }
            else
            {
                Debug.WriteLine(result.Erro.Message);
            }




        }
    }
}
