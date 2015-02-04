// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556
using System;
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
    public sealed partial class MapUtil : Page
    {

        private readonly AMapControl aMapControl = new AMapControl();
        private readonly AMapLayer baseLayer = new AMapLayer();
        private LngLat[] pointsOfLine;
        private LngLat[] pointsOfRect;

        public MapUtil()
        {
            this.InitializeComponent();
            ContentGrid.Children.Add(aMapControl);
            aMapControl.Layers.Add(baseLayer);

            aMapControl.Center = new LngLat(116.450764, 39.96041);
            aMapControl.ZoomLevel = 13;





        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            AMapPolyline line = new AMapPolyline();
            pointsOfLine = new[] { new LngLat(116.48236, 39.987326), new LngLat(116.450712, 39.93041) };
            line.Path = new AGeopath(pointsOfLine);
            line.StrokeColor = Colors.Red;
            baseLayer.Add(line);

            AMapPolygon rectRectangle = new AMapPolygon();

            pointsOfRect = new[] { new LngLat(116.438764, 39.96041), new LngLat(116.438764, 39.963728), new LngLat(116.453712, 39.963728), new LngLat(116.453712, 39.96041) };
            rectRectangle.Path = new AGeopath(pointsOfRect);

            baseLayer.Add(rectRectangle);

        }

        private async void Distance(object sender, RoutedEventArgs e)
        {
            
            float distance = AMapUtils.CalculateLineDistance(pointsOfLine[0], pointsOfLine[1]);
            MessageDialog messageDialog = new MessageDialog("线长" + distance + "m");
            await messageDialog.ShowAsync();
        }

        private async void Area(object sender, RoutedEventArgs e)
        {
            float area = AMapUtils.CalculateArea(AMapUtils.GetSouthWest(pointsOfRect), AMapUtils.GetNorthEast(pointsOfRect));

            MessageDialog messageDialog = new MessageDialog("面积" + area + "m^2");
            await messageDialog.ShowAsync();
        }
    }
}
