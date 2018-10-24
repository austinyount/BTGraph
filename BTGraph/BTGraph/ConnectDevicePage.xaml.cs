using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.BLE;

using System.Diagnostics;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.BLE.Abstractions.Contracts;
using System.Collections.ObjectModel;

namespace BTGraph
{
	// [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ConnectDevicePage : ContentPage
	{
        IBluetoothLE ble;
        IAdapter adapter;
        ObservableCollection<IDevice> deviceList;
        StackLayout availableDevices = new StackLayout();


        public ConnectDevicePage ()
		{
			InitializeComponent ();
            ble = CrossBluetoothLE.Current;
            adapter = CrossBluetoothLE.Current.Adapter;
            deviceList = new ObservableCollection<IDevice>();
            deviceList.Clear();

            adapter.DeviceDiscovered += (s, a) =>
            {
                deviceList.Add(a.Device);
            };
            adapter.StartScanningForDevicesAsync();

            // populateBluetoothDevices();
            Content = new StackLayout
            {
                Children =
                {
                    //new ScrollView
                    //{
                        //VerticalOptions = LayoutOptions.FillAndExpand,
                        //Content = deviceList
                    //}
                }
            };
		}
        void populateBluetoothDevices()
        {
            for (int i = 0; i < 10; i++)
            {
                availableDevices.Children.Add(new Button
                {
                    Text = $"Device {i}"
                });
                
            }
            foreach(Button button in availableDevices.Children)
            {
                button.Clicked += OnDeviceClicked;
            }

        }
        void OnDeviceClicked(object sender, EventArgs e)
        {
            Button senderButton = (Button)sender;
            //Debug.WriteLine("Clicked Button");
            MessagingCenter.Send<ConnectDevicePage, string>(this, "Device Name", senderButton.Text);
            Application.Current.MainPage.Navigation.PopAsync();
        }
	};
}