// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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
    public sealed partial class BusSearch : Page
    {
        private readonly AMapControl aMapControl = new AMapControl();
        private readonly AMapLayer baseLayer = new AMapLayer();

        public BusSearch()
        {
            this.InitializeComponent();
            ContentGrid.Children.Add(aMapControl);
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


        private async void LineIdSearch_OnClick(object sender, RoutedEventArgs e)
        {
            baseLayer.Clear();
            string crashid = " 440100014234";

            string id = "110100013752";


            AMapBusLineResults busLines = await AMapBusSearch.BusLineIDSearch(id);

            if (busLines.Erro == null)
            {
                if (busLines.BusLineList == null || busLines.BusLineList.Count == 0)
                {
                    Debug.WriteLine("无查询结果");
                    return;
                }

                List<AMapBusLine> busLine = busLines.BusLineList.ToList();

                //Id 搜索 只会搜索到一个
                AMapBusLine bl = busLine.FirstOrDefault();

                Debug.WriteLine(bl.Name);
                List<LngLat> latlng = LngLatsFromString(bl.Polyline);

                AMapPolyline polyline = new AMapPolyline
                {
                    Path = new AGeopath(latlng),
                    StrokeColor = Colors.Blue,
                    StrokeThickness = 10
                };

                AMapIcon start = new AMapIcon
                {
                    Image =
                        RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/bus_start_pic.png",
                            UriKind.RelativeOrAbsolute)),
                    Location = latlng.FirstOrDefault()
                };

                AMapIcon end = new AMapIcon
                {
                    Image =
                        RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/bus_end_pic.png",
                            UriKind.RelativeOrAbsolute)),
                    Location = latlng.LastOrDefault()
                };

                await baseLayer.Add(start);
                await baseLayer.Add(end);

                //todo 纹理已经录入，但是为什么会crash
                await baseLayer.Add(polyline);

            }
            else
            {
                Debug.WriteLine(busLines.Erro.Message);
            }
        }

        private List<LngLat> LngLatsFromString(string polyline)
        {
            string[] arrystring = polyline.Split(new[] { ';' });
            return arrystring.Select(str => str.Split(new[] { ',' })).Select(lnglatds => new LngLat(Double.Parse(lnglatds[0]), Double.Parse(lnglatds[1]))).ToList();
        }

        private async void BusStopSearch_OnClick(object sender, RoutedEventArgs e)
        {
            baseLayer.Clear();
            var bus = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/bus.png",
                UriKind.RelativeOrAbsolute));

            string keywords = "北里";
            uint offset = 20;
            uint page = 1;
            string city = "北京";


            AMapBusStopResults busStoprs = await AMapBusSearch.BusStopKeyWords(keywords, offset, page, city);
            if (busStoprs.Erro == null)
            {
                if (busStoprs.BusStopList.Count == 0 || busStoprs.BusStopList == null)
                {
                    Debug.WriteLine("无查询结果");
                    return;
                }

                var busStops = busStoprs.BusStopList;
                List<LngLat> latLngs = new List<LngLat>();

                foreach (AMapBusStop bs in busStops)
                {
                    Debug.WriteLine(bs.Id);
                    Debug.WriteLine(bs.Name);
                    Debug.WriteLine(bs.Location.Lat);
                    Debug.WriteLine(bs.Location.Lon);

                    latLngs.Add(new LngLat(bs.Location.Lon, bs.Location.Lat));
                }

                //绘制公交站
                foreach (LngLat latlng in latLngs)
                {

                    var icon = new AMapIcon { Location = latlng, Image = bus };
                    await baseLayer.Add(icon);

                }
                LngLatBoundingBoxBuilder builder = new LngLatBoundingBoxBuilder(latLngs);

                aMapControl.TrySetViewBoundsAsync(builder.Build(), AMapAnimationKind.Default);
            }
            else
            {
                Debug.WriteLine(busStoprs.Erro.Message);
            }



        }

        private async void BusLineSearch_OnClick(object sender, RoutedEventArgs e)
        {
            baseLayer.Clear();


            var busRef = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/bus.png",
                UriKind.RelativeOrAbsolute));

            string keywords = "404";
            uint offset = 20;
            uint page = 1;
            string city = "北京";

            AMapBusLineResults busLines = await AMapBusSearch.BusLineKeyWords(keywords, offset, page, city, Extensions.All);

            if (busLines.Erro == null)
            {
                if (busLines.BusLineList.Count == 0 || busLines.BusLineList == null)
                {
                    Debug.WriteLine("无查询结果");
                    return;
                }

                IEnumerable<AMapBusLine> bLines = busLines.BusLineList;


                AMapIcon start = new AMapIcon
                {
                    Location =
                        new LngLat(bLines.FirstOrDefault().Bus_stops[0].Location.Lon,
                            bLines.FirstOrDefault().Bus_stops[0].Location.Lat),
                    Image =
                        RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/bus_start_pic.png",
                            UriKind.RelativeOrAbsolute))
                };



                AMapIcon end = new AMapIcon
                {
                    Location =
                        new LngLat(bLines.FirstOrDefault().Bus_stops.Last().Location.Lon,
                            bLines.FirstOrDefault().Bus_stops.Last().Location.Lat),
                    Image =
                        RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/bus_end_pic.png",
                            UriKind.RelativeOrAbsolute))
                };

                await baseLayer.Add(start);
                await baseLayer.Add(end);

                List<LngLat> lnglats = LngLatsFromString(bLines.FirstOrDefault().Polyline);

                AMapPolyline polyline = new AMapPolyline { Path = new AGeopath(lnglats) };

                await baseLayer.Add(polyline);

                List<LngLat> busstops = new List<LngLat>();

                //添加途径公交站
                foreach (AMapBusStop bs in bLines.FirstOrDefault().Bus_stops)
                {
                    busstops.Add(new LngLat(bs.Location.Lon, bs.Location.Lat));

                }

                //去除起始站和终点站
                busstops.RemoveAt(0);
                busstops.RemoveAt(busstops.Count - 1);

                foreach (LngLat lnglat in busstops)
                {

                    var bus = new AMapIcon
                    {
                        Location = lnglat,
                        Image = busRef
                    };
                    await baseLayer.Add(bus);
                }


                aMapControl.TrySetViewBoundsAsync(new LngLatBoundingBox(lnglats), AMapAnimationKind.Default);
            }
            else
            {
                Debug.WriteLine(busLines.Erro.Message);
            }
        }


        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

        }
    }
}
