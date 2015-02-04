// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage.Streams;
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
    public sealed partial class GeoCode : Page
    {
        private readonly AMapControl aMapControl = new AMapControl();
        private readonly AMapLayer baseLayer = new AMapLayer();

        public GeoCode()
        {
            InitializeComponent();
            ContentGrid.Children.Add(aMapControl);
            aMapControl.Layers.Add(baseLayer);
            addressInputTextBox.Text = "北京站";
            aMapControl.MapTapped += aMapControl_MapTapped;
        }

        private async void aMapControl_MapTapped(AMapControl sender, AMapInputEventArgs args)
        {

            AMapReGeoCodeResults results =
                await
                    AMapReGeoCodeSearch.GeoCodeToAddress(args.Location.Longitude, args.Location.Latitude, 500, "",
                        Extensions.All);

            //todo 一下坐标存在问题
            //var error1 = new LngLat(116.251101, 39.836455);
            //AMapReGeoCodeResults results = await AMapReGeoCodeSearch.GeoCodeToAddress(error1.Longitude, error1.Latitude, 500, "", Extensions.All);

            if (results.Erro == null)
            {
                if (results.ReGeoCode == null)
                {
                    addressOutputTextblock.Text = "返回为null";
                    return;
                }


                AMapReGeoCode regeocode = results.ReGeoCode;
                Debug.WriteLine(regeocode.Address_component);
                Debug.WriteLine(regeocode.Formatted_address);
                Debug.WriteLine(regeocode.Pois);

                List<AMapPOI> pois = regeocode.Pois.ToList();
                //POI信息点
                foreach (AMapPOI poi in pois)
                {
                    Debug.WriteLine(poi.Address);
                }

                Debug.WriteLine(regeocode.Roadinters);
                Debug.WriteLine(regeocode.Roadslist);
                AMapAddressComponent addressComponent = regeocode.Address_component;

                //todo building
                //Debug.WriteLine(addressComponent.Building);

                Debug.WriteLine(addressComponent.City);
                Debug.WriteLine(addressComponent.District);

                //todo 邻居
                //Debug.WriteLine(addressComponent.Neighborhood);

                Debug.WriteLine(addressComponent.Province);
                Debug.WriteLine(addressComponent.Stree_number);
                Debug.WriteLine(addressComponent.Township);
                AMapStreetNumber streetNumber = addressComponent.Stree_number;
                Debug.WriteLine(streetNumber.Direction);
                Debug.WriteLine(streetNumber.Distance);
                Debug.WriteLine(streetNumber.Location.Lat);
                Debug.WriteLine(streetNumber.Location.Lon);
                Debug.WriteLine(streetNumber.Number);
                Debug.WriteLine(streetNumber.Street);


                addressOutputTextblock.Text = regeocode.Formatted_address;
                aMapControl.TrySetViewAsync(args.Location);
            }
            else
            {
                addressOutputTextblock.Text = results.Erro.Message;
            }
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

        private async void SearchButton_OnClick(object sender, RoutedEventArgs e)
        {
            baseLayer.Clear();
            await AddressToGeoCode(addressInputTextBox.Text);
        }


        private async Task AddressToGeoCode(string address)
        {
            var azure = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/AZURE.png"));


            AMapGeoCodeResults result = await AMapGeoCodeSearch.AddressToGeoCode(address);

            if (result.Erro == null)
            {
                if (result.GeoCodeList == null || !result.GeoCodeList.Any())
                {
                    Debug.WriteLine("无查询结果");
                    return;
                }


                IEnumerable<AMapGeoCode> geocode = result.GeoCodeList;
                int i = 0;
                IList<AMapGeoCode> aMapGeoCodes = geocode as IList<AMapGeoCode> ?? geocode.ToList();
                foreach (AMapGeoCode gcs in aMapGeoCodes)
                {
                    i++;
                    Debug.WriteLine(gcs.Adcode);
                    Debug.WriteLine(gcs.Building);
                    Debug.WriteLine(gcs.City);
                    Debug.WriteLine(gcs.District);
                    Debug.WriteLine(gcs.FormattedAddress);
                    Debug.WriteLine(gcs.Province);

                    //todo township
                    //Debug.WriteLine(gcs.Township);

                    Debug.WriteLine(gcs.Location.Lon);
                    Debug.WriteLine(gcs.Location.Lat);
                    Debug.WriteLine(gcs.LevelList[0]);

                    var icon = new AMapIcon
                    {
                        Location = new LngLat(gcs.Location.Lon, gcs.Location.Lat),
                        Data = new
                        {
                            A = gcs.FormattedAddress,
                            B = gcs.District
                        },
                        Image = azure
                    };

                    await baseLayer.Add(icon);
                }
                //如果返回的geocode数大于1个，调整视图
                if (aMapGeoCodes.Count() > 1)
                {
                    var builder = new LngLatBoundingBoxBuilder();
                    foreach (IAMapOverLay overlay in baseLayer.OverLays)
                    {
                        if (overlay as AMapIcon != null)
                        {
                            builder.Include((overlay as AMapIcon).Location);
                        }
                    }

                    aMapControl.TrySetViewBoundsAsync(builder.Build(), AMapAnimationKind.Default);
                }
                else
                {
                    aMapControl.TrySetViewAsync(new LngLat(aMapGeoCodes.FirstOrDefault().Location.Lon,
                        aMapGeoCodes.FirstOrDefault().Location.Lat), null, null, null, AMapAnimationKind.Default);
                }

                Debug.WriteLine(i);
            }
            else
            {
                addressOutputTextblock.Text = result.Erro.Message;
            }
        }

        private void AddressInputTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (addressInputTextBox.Text.Equals(""))
            {
                return;
            }
            GetAmapInputTips(addressInputTextBox.Text, "", "");
        }


        private async void GetAmapInputTips(string words, string city, string types)
        {
            AMapTipResults tipResults = await AMapInputTips.InputTips(words, city, types);
            if (tipResults.Erro == null)
            {
                if (tipResults.TipList == null || !tipResults.TipList.Any())
                {
                    Debug.WriteLine("无查询结果");
                    return;
                }
                foreach (AMapTip tip in tipResults.TipList)
                {
                    Debug.WriteLine(tip.Adcode);
                    Debug.WriteLine(tip.Name);
                    Debug.WriteLine(tip.District);
                }

                //绑定列表数据
                listBox.ItemsSource = tipResults.TipList.ToList();
            }
            else
            {
                Debug.WriteLine(tipResults.Erro.Message);
            }
        }

        private void OnGotFocus(object sender, RoutedEventArgs e)
        {
            hintCanvas.Visibility = Visibility.Visible;
        }

        private void OnLostFocus(object sender, RoutedEventArgs e)
        {
            hintCanvas.Visibility = Visibility.Collapsed;
        }

        private void ListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listbox = sender as ListBox;
            var item = listbox.SelectedItem as AMapTip;
            if (item == null)
            {
                return;
            }

            addressInputTextBox.Text = item.Name;
            item = null;
        }
    }
}