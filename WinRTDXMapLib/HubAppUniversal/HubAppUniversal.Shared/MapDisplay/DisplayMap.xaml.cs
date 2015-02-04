// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556
using System.Diagnostics;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Com.AMap.Api.Maps;

namespace HubAppUniversal.MapDisplay
{
    /// <summary>
    /// 1.App.xaml.cs文件中，在构造函数中加入AMapConfig.Key = "[你在高德官网申请的key]";http://lbs.amap.com/console/
    /// 安全码：AppID = Package.appxmanifest->Package->PackageName，测试环节
    /// 安全码：AppID = 你的应用页面的AppId，上线环节
    /// 
    ///     /// </summary>
    public sealed partial class DisplayMap : Page
    {

        private readonly AMapControl aMapControl = new AMapControl();
        public DisplayMap()
        {
            this.InitializeComponent();
            ContentGrid.Children.Add(aMapControl);

            

            aMapControl.LoadingStatusChanged += aMapControl_LoadingStatusChanged;
        }



        private void aMapControl_LoadingStatusChanged(AMapControl sender, AMapLoadingStatus args)
        {
            if (args==AMapLoadingStatus.Loaded)
            {
                Debug.WriteLine("Loaded!");
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
