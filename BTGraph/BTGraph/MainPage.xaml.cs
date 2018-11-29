using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.Exceptions;
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
        IDevice selectedDevice;
        //Button button = btnConnectBluetooth;

        public MainPage()
        {
            InitializeComponent();
            ble = CrossBluetoothLE.Current;
            adapter = CrossBluetoothLE.Current.Adapter;
            var state = ble.State;
            deviceList = new ObservableCollection<IDevice>();
            lv.ItemsSource = deviceList;
            ble.StateChanged += (s, e) =>
            {
                DisplayAlert("Notice", $"Bluetooth: {e.NewState}", "OK");
            };
            adapter.ScanTimeoutElapsed += (s, e) =>
            {
                DisplayAlert("Notice", "timeout elapsed", "OK");
                btnConnectBluetooth.Text = "Tap to scan for devices";
            };
            adapter.DeviceConnected += (s, a) =>
            {
                btnConnectBluetooth.Text = "Tap to scan for devices";
                DisplayAlert("Notice", "Connected!", "OK");
            };
            adapter.DeviceDiscovered += (s, a) =>
            {
                deviceList.Add(a.Device);
            };
        }

        private async void lv_ItemSelected(object sender, EventArgs e)
        {
            if (lv.SelectedItem == null)
            {
                await DisplayAlert("Notice", "No Device selected", "OK");
                return;
            }
            else
            {
                selectedDevice = lv.SelectedItem as IDevice;
                try
                {
                    await adapter.ConnectToDeviceAsync(selectedDevice);
                }
                catch(DeviceConnectionException ex)
                {
                    await DisplayAlert("Notice", "Error connecting to device!", "OK");
                }
                catch(ArgumentNullException ex)
                {
                    await DisplayAlert("Notice", "Selected device is null!", "OK");
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Notice", "Unknown exception!", "OK");
                }
            }
        }

        private async void OnButtonClicked(object sender, EventArgs args)
        {
            //Button button = (Button)sender;
            deviceList.Clear();

            if(!ble.Adapter.IsScanning)
            {
                btnConnectBluetooth.Text = "Scanning... tap to stop";
                adapter.ScanTimeout = 30000;
                await adapter.StartScanningForDevicesAsync();
            }
            else
            {
                btnConnectBluetooth.Text = "Tap to scan for devices";
                await adapter.StopScanningForDevicesAsync();
            }

        }
    };
}
