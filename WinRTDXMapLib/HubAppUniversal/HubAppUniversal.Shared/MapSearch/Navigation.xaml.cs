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
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Navigation : Page
    {
        private readonly AMapControl aMapControl = new AMapControl();
        private readonly AMapLayer baseLayer = new AMapLayer();
        private readonly LngLat end = new LngLat(116.40302, 39.884717);
        private readonly LngLat start = new LngLat(116.481028, 39.989643);

        public Navigation()
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

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {

            base.OnNavigatedFrom(e);
        }

        private async void Walk_OnClick(object sender, RoutedEventArgs e)
        {

            baseLayer.Clear();

            var man =
                RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/man.png",
                    UriKind.RelativeOrAbsolute));

            AMapRouteResults rts =
                await
                    AMapNavigationSearch.WalkingNavigation(start.Longitude, start.Latitude, end.Longitude, end.Latitude,
                        0);
            if (rts.Erro == null)
            {
                if (rts.Count == 0)
                {
                    Debug.WriteLine("无查询结果");
                    return;
                }

                AMapRoute route = rts.Route;
                List<AMapPath> paths = route.Paths.ToList();

                foreach (AMapPath item in paths)
                {
                    Debug.WriteLine("起点终点距离:" + item.Distance);
                    Debug.WriteLine("预计耗时:" + item.Duration / 60 + "分钟");
                    Debug.WriteLine("导航策略:" + item.Strategy);

                    //画路线
                    List<AMapStep> steps = item.Steps.ToList();
                    foreach (AMapStep st in steps)
                    {
                        var icon = new AMapIcon
                        {
                            Location = LngLatsFromString(st.Polyline).FirstOrDefault(),
                            Image = man
                        };


                        Debug.WriteLine(st.Instruction);

                        var polyline = new AMapPolyline { Path = new AGeopath(LngLatsFromString(st.Polyline)) };

                        await baseLayer.Add(icon);
                        await baseLayer.Add(polyline);
                    }
                }
            }
            else
            {
                Debug.WriteLine(rts.Erro.Message);
            }
        }

        private async void Car_OnClick(object sender, RoutedEventArgs e)
        {
            baseLayer.Clear();

            //116.481028, 39.989643, 116.465302, 40.004717
            AMapRouteResults rts =
                await
                    AMapNavigationSearch.DrivingNavigation(start.Longitude, start.Latitude, end.Longitude, end.Latitude);

            #region 线段是一段段给的
            //if (rts.Erro == null)
            //{
            //    if (rts.Count == 0)
            //    {
            //        Debug.WriteLine("无查询结果");
            //        return;
            //    }

            //    AMapRoute route = rts.Route;
            //    List<AMapPath> paths = route.Paths.ToList();

            //    foreach (AMapPath item in paths)
            //    {
            //        Debug.WriteLine("起点终点距离:" + item.Distance);
            //        Debug.WriteLine("预计耗时:" + item.Duration/60 + "分钟");
            //        Debug.WriteLine("导航策略:" + item.Strategy);

            //        //绘制驾车路线
            //        List<AMapStep> steps = item.Steps.ToList();
            //        foreach (AMapStep st in steps)
            //        {
            //            Debug.WriteLine(st.Instruction);

            //            var icon = new AMapIcon
            //            {
            //                Location = LngLatsFromString(st.Polyline).FirstOrDefault(),
            //                Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/car.png",
            //                    UriKind.RelativeOrAbsolute))
            //            };


            //            IEnumerable<LngLat> lnglats = LngLatsFromString(st.Polyline);

            //            var polyline = new AMapPolyline {Path = new AGeopath(lnglats)};

            //            await baseLayer.Add(polyline);
            //            await baseLayer.Add(icon);
            //        }
            //    }
            //}
            //else
            //{
            //    Debug.WriteLine(rts.Erro.Message);
            //}


            #endregion


            if (rts.Erro == null)
            {
                if (rts.Count == 0)
                {
                    Debug.WriteLine("无查询结果");
                    return;
                }

                AMapRoute route = rts.Route;


                List<LngLat> alLngLats = new List<LngLat>();

                List<AMapPath> paths = route.Paths.ToList();

                foreach (AMapPath item in paths)
                {
                    Debug.WriteLine("起点终点距离:" + item.Distance);
                    Debug.WriteLine("预计耗时:" + item.Duration / 60 + "分钟");
                    Debug.WriteLine("导航策略:" + item.Strategy);

                    //绘制驾车路线
                    List<AMapStep> steps = item.Steps.ToList();

                    foreach (AMapStep st in steps)
                    {
                        IEnumerable<LngLat> lnglats = LngLatsFromString(st.Polyline);
                        alLngLats.AddRange(lnglats);
                    }
                }

                AMapPolyline polyline = new AMapPolyline();
                polyline.Path = new AGeopath(alLngLats);

                await baseLayer.Add(polyline);


            }
            else
            {
                Debug.WriteLine(rts.Erro.Message);
            }
        }

        private async void Bus_OnClick(object sender, RoutedEventArgs e)
        {
            baseLayer.Clear();

            string city = "北京";
            AMapRouteResults rts =
                await
                    AMapNavigationSearch.BusNavigation(start.Longitude, start.Latitude, end.Longitude, end.Latitude, 0,
                        false, city);
            if (rts.Erro == null)
            {
                if (rts.Count == 0)
                {
                    Debug.WriteLine("无查询结果");
                    return;
                }

                AMapRoute route = rts.Route;

                List<AMapTransit> transits = route.Transits.ToList();
                List<AMapSegment> segments = transits.FirstOrDefault(p => p.Segments != null).Segments.ToList();




                var busEnd = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/bus_end_pic.png",
                    UriKind.RelativeOrAbsolute));

                var busStart = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/bus_start_pic.png",
                    UriKind.RelativeOrAbsolute));


                foreach (AMapSegment item in segments)
                {
                    var destination = new AMapIcon
                    {
                        Location = new LngLat(item.Walking.Destination.Lon, item.Walking.Destination.Lat),
                        Image = busEnd

                    };


                    var origin = new AMapIcon
                    {
                        Location = new LngLat(item.Walking.Origin.Lon, item.Walking.Origin.Lat),
                        Image = busStart

                    };

                    await baseLayer.Add(origin);
                    await baseLayer.Add(destination);


                    //绘制步行路径
                    foreach (AMapStep sp in item.Walking.Steps)
                    {
                        var polyline = new AMapPolyline
                        {
                            Path = new AGeopath(LngLatsFromString(sp.Polyline)),
                            StrokeColor = Colors.Blue
                        };

                        await baseLayer.Add(polyline);

                        //Debug.WriteLine(line.Points.Count);
                        //Debug.WriteLine("Walking.Steps:" + sp.Polyline);
                    }
                    //绘制公交路径
                    foreach (AMapBusLine busLine in item.BusLine)
                    {
                        var polyline = new AMapPolyline
                        {
                            Path = new AGeopath(LngLatsFromString(busLine.Polyline)),
                            StrokeColor = Colors.Green
                        };



                        await baseLayer.Add(polyline);

                        if (busLine.Via_stops == null)
                        {
                            continue;
                        }


                        RandomAccessStreamReference bus =
                            RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/bus.png",
                                UriKind.RelativeOrAbsolute));


                        foreach (AMapBusStop busstop in busLine.Via_stops)
                        {
                            var busIcon = new AMapIcon
                            {
                                Location = new LngLat(busstop.Location.Lon, busstop.Location.Lat),
                                Image = bus
                            };
                            await baseLayer.Add(busIcon);
                        }
                    }
                }
            }
            else
            {
                Debug.WriteLine(rts.Erro.Message);
            }
        }


        private IEnumerable<LngLat> LngLatsFromString(string polyline)
        {
            string[] arrystring = polyline.Split(new[] { ';' });
            return
                arrystring.Select(str => str.Split(new[] { ',' }))
                    .Select(lnglatds => new LngLat(Double.Parse(lnglatds[0]), Double.Parse(lnglatds[1])))
                    .ToList();
        }
    }
}