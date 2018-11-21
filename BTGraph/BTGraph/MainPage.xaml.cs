using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BTGraph
{
    public partial class MainPage : ContentPage
    {
        IBluetoothLE ble;
        IAdapter adapter;
        ObservableCollection<IDevice> deviceList;
        StackLayout availableDevices = new StackLayout();

        public MainPage()
        {
            InitializeComponent();
            ble = CrossBluetoothLE.Current;
            adapter = CrossBluetoothLE.Current.Adapter;
            deviceList = new ObservableCollection<IDevice>();
            lv.ItemsSource = deviceList;
        }

        private async void lv_ItemSelected(object sender, EventArgs e)
        {
            if(lv.SelectedItem == null)
            {
                DisplayAlert("Notice", "No Device selected", "OK");
                return;
            }
            else
            {
                IDevice selectedDevice = (IDevice)lv.SelectedItem;
                await adapter.ConnectToDeviceAsync(selectedDevice);
            }
        }

        private async void OnButtonClicked(object sender, EventArgs args)
        {
            Button button = (Button)sender;
            deviceList.Clear();

            adapter.DeviceDiscovered += (s, a) =>
            {
                deviceList.Add(a.Device);
            };
            if(!ble.Adapter.IsScanning)
            {
                button.Text = "Scanning...";
                await adapter.StartScanningForDevicesAsync();
            }
        }
    };
}
