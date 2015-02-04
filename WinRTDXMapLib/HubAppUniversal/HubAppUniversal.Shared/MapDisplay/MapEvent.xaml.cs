// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Com.AMap.Api.Maps;
using Com.AMap.Api.Maps.OverLay;

namespace HubAppUniversal.MapDisplay
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MapEvent : Page
    {
        private readonly AMapControl aMapControl = new AMapControl();
        private readonly AMapLayer baselayer = new AMapLayer();

        public MapEvent()
        {
            this.InitializeComponent();
            ContentGrid.Children.Add(aMapControl);
            aMapControl.MapTapped += aMapControl_MapTapped;
            aMapControl.MapDoubleTapped += aMapControl_MapDoubleTapped;
            aMapControl.MapHolding += aMapControl_MapHolding;

            var icon = new AMapIcon();
            icon.Location = aMapControl.Center;

            aMapControl.Layers.Add(baselayer);
            baselayer.Add(icon);
        }

        void aMapControl_MapHolding(AMapControl sender, AMapInputEventArgs args)
        {
            eventTextBlock.Text = "event is holding";
            lon.Text = args.Location.Longitude.ToString();
            lat.Text = args.Location.Latitude.ToString();
        }

        void aMapControl_MapDoubleTapped(AMapControl sender, AMapInputEventArgs args)
        {
            eventTextBlock.Text = "event is doubletapped";
            lon.Text = args.Location.Longitude.ToString();
            lat.Text = args.Location.Latitude.ToString();
        }

        void aMapControl_MapTapped(AMapControl sender, AMapInputEventArgs args)
        {
            eventTextBlock.Text = "event is tapped";
            lon.Text = args.Location.Longitude.ToString();
            lat.Text = args.Location.Latitude.ToString();

            if (args.AMapIcon != null)
            {
                mapiconTestblock.Text = "点击到了icon，他的经纬度为" + args.AMapIcon.Location;
            }
            else
            {
                mapiconTestblock.Text = "没有点击到icon";
            }
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
    }
}
