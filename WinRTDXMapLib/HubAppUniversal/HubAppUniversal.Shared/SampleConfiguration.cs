using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;
using HubAppUniversal.Data;
using HubAppUniversal.MapDisplay;
using HubAppUniversal.MapSearch;
using OfficalDemo.MapDisplay;

namespace HubAppUniversal
{
    public partial class HubPage : Page
    {
        private readonly SampleDataGroup mapDemoGroup = new SampleDataGroup("map")
        {
            Items = new ObservableCollection<SampleDataItem>
            {
                new SampleDataItem {Title = "显示地图", ClassType = typeof (DisplayMap),Description = "简单描述如何显示一个地图"},
                new SampleDataItem {Title = "手势开关", ClassType = typeof (Gesture),Description = "是否开启缩放，旋转，倾斜，拖动的手势控制"},
                new SampleDataItem {Title = "前台控件开关", ClassType = typeof (ForeGroundControls),Description = "是否让缩放按钮，指南针，比例尺可见"},
                new SampleDataItem {Title = "地图个性化图层开关", ClassType = typeof (CustomDisplay),Description = "是否可见建筑，是否打开实时交通，是否打开卫星地图"},
                new SampleDataItem {Title = "定位", ClassType = typeof (Locate),Description = "获取单次位置，以及持续定位"},
                new SampleDataItem {Title = "视角变化", ClassType = typeof (ViewChange),Description = "视角变化时候，如何获得回调"},
                new SampleDataItem {Title = "地图事件回调", ClassType = typeof (MapEvent),Description = "点击地图，双击地图，hold地图，点击icon的回调函数"},
                new SampleDataItem {Title = "地图测量工具", ClassType = typeof (MapUtil),Description = "测量距离，以及面积"},
                new SampleDataItem {Title = "Overlay添加", ClassType = typeof (OverlayManagement),Description = "如何添加各种各样的overlay"},
                new SampleDataItem {Title = "地图Layer管理", ClassType = typeof (LayerManagement),Description = "如何实现多层地图管理"},
                new SampleDataItem {Title = "自定义窗体", ClassType = typeof (CustomInfoWindows),Description = "如何显示自定义的窗口控件"},
            }
        };


        private readonly SampleDataGroup searchDemoGroup = new SampleDataGroup("search")
        {
            Items = new ObservableCollection<SampleDataItem>
            {
                new SampleDataItem {Title = "地理正反编码与文字提示", ClassType = typeof (GeoCode),Description = "如何进行正反编码，以及实现文字变化的时候有输入提示"},
                new SampleDataItem {Title = "POI查询", ClassType = typeof (POISearch),Description = "搜索poi点，id搜索，周边搜索，关键字搜索，多边形搜索"},
                new SampleDataItem {Title = "公交查询", ClassType = typeof (BusSearch),Description = "公交站名搜索，公交ID搜索，公交线路搜索"},
                new SampleDataItem {Title = "导航", ClassType = typeof (Navigation),Description = "公交导航，车行导航，步行导航"},
                new SampleDataItem {Title = "实验", ClassType = typeof (ManyIcons),Description = "如何添加大量icon"},
            }
        };

        private SampleDataGroup MapDemoGroup
        {
            get { return mapDemoGroup; }
        }

        private SampleDataGroup SearchDemoGroup
        {
            get { return searchDemoGroup; }
        }
    }
}