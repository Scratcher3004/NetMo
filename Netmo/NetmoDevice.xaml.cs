using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Netmo
{
    /// <summary>
    /// Interaktionslogik für NetmoDevice.xaml
    /// </summary>
    public partial class NetmoDevice : UserControl
    {
        public static List<NetmoDevice> nd = new List<NetmoDevice>();   // All device Controls stored in a static list
        private Device device;                                          // Downloaded Data for this Device
        private int deviceID;                                           // DeviceID of this Device
        private DispatcherTimer loadingTimer;                           // Timer for Refresh Loading Symbol rotation
        private List<NetmoModule> modules = new List<NetmoModule>();


        public NetmoDevice()
        {
            InitializeComponent();  // Initialize UserControl
            deviceID = nd.Count;    // Setting up the DeviceID of this DeviceControl
            nd.Add(this);           // Add this DeviceControl to the list of all DEvicesControls

            // Hide Loading Symbol
            Loading.Visibility = Visibility.Hidden;
            Loading.IsEnabled = false;
        }

        // Initialize and display stored data
        public void Init(Device device_)
        {
            device = device_;
            CreateModuleTabs(); // Create Sub-Module Tabs
            DisplayData();
        }

        /// <summary>
        /// Called when Refresh-Button was Clicked
        /// </summary>
        /// <param name="sender">Unused param</param>
        /// <param name="e">Unused param</param>
        public void Update(object sender, RoutedEventArgs e)
        {
            // Show Loading Symbol and Update Async
            Loading.Visibility = Visibility.Visible;
            Loading.IsEnabled = true;
            DownloadDataAsyncAndUpdate();

            // Initializing the Loading Animation
            Loading.RenderTransform = new RotateTransform(0);       // Reset Rotation
            Loading.RenderTransformOrigin = new Point(0.5, 0.5);    // Set Origin
            loadingTimer = new DispatcherTimer();                   // Create new Timer
            loadingTimer.Tick += RotateWhileRefeshing;              // Adding RotateWhileRefreshing to the Timer-Tick event
            loadingTimer.Interval = new TimeSpan(0, 0, 0, 0, 10);   // Setting the Timer-Interval to 10ms - Set the loading symbol by 100 degree per second
            loadingTimer.Start();                                   // Starting the Timer event
        }

        /// <summary>
        /// Download the data async
        /// </summary>
        private async void DownloadDataAsyncAndUpdate()
        {
            device = await MainWindow.mw.GetDeviceData(deviceID);   // Download Device data
            DisplayData();                                          // Display new Device data

            // Stopping Animation
            Loading.Visibility = Visibility.Hidden;
            Loading.IsEnabled = false;
            loadingTimer.Stop();
        }

        /// <summary>
        /// Rotating the Loading Symbol by 1 degree. Params are only used for Timer events.
        /// </summary>
        /// <param name="sender">Unused param</param>
        /// <param name="e">Unused param</param>
        private void RotateWhileRefeshing(object sender, EventArgs e)
        {
            if (((RotateTransform)Loading.RenderTransform).Angle >= 360)
            {
                ((RotateTransform)Loading.RenderTransform).Angle = 0;
            }
            Loading.RenderTransform = new RotateTransform(1 + ((RotateTransform)Loading.RenderTransform).Angle);
        }

        /// <summary>
        /// Displays the current Device-Data
        /// </summary>
        public void DisplayData()
        {
            DeviceDashboardData data = device.DashboardData;

            // Temperature Display
            temp.Text = "Temperature: " + data.Temperature;
            minTemp.Text = "Min Temperature: " + data.MinTemp;
            maxTemp.Text = "Max Temperature: " + data.MaxTemp;
            if (data.Temperature <= 5)
            {
                temperatureRawImage.Source = new BitmapImage(new Uri("images/042-cold.png", UriKind.Relative));
            }
            else if (data.Temperature >= 27.5)
            {
                temperatureRawImage.Source = new BitmapImage(new Uri("images/044-warm-1.png", UriKind.Relative));
            }
            else
            {
                temperatureRawImage.Source = new BitmapImage(new Uri("images/043-warm.png", UriKind.Relative));
            }

            // Util Display
            humidity.Text = "Humidity: " + data.Humidity;
            presure.Text = "Presure: " + data.Pressure;
            co2.Text = "CO2: " + data.Co2;
            noise.Text = "Noise: " + data.Noise;


            // TrendDisplay
            switch (data.TempTrend) // Temperature Trend
            {
                case "stable":
                    TempTrend.Visibility = Visibility.Hidden;
                    break;

                case "up":
                    TempTrend.Visibility = Visibility.Visible;
                    TempTrend.Kind = MaterialDesignThemes.Wpf.PackIconKind.ArrowUpBold;
                    break;

                case "down":
                    TempTrend.Visibility = Visibility.Visible;
                    TempTrend.Kind = MaterialDesignThemes.Wpf.PackIconKind.ArrowDownBold;
                    break;

                default:
                    break;
            }

            switch (data.PressureTrend) // Pressure Trend
            {
                case "stable":
                    PressureTemp.Visibility = Visibility.Hidden;
                    break;

                case "up":
                    PressureTemp.Visibility = Visibility.Visible;
                    PressureTemp.Kind = MaterialDesignThemes.Wpf.PackIconKind.ArrowUpBold;
                    break;

                case "down":
                    PressureTemp.Visibility = Visibility.Visible;
                    PressureTemp.Kind = MaterialDesignThemes.Wpf.PackIconKind.ArrowDownBold;
                    break;

                default:
                    break;
            }

            UpdateModuleTabs();
        }

        /// <summary>
        /// Updating all Module-Tabs
        /// </summary>
        private void UpdateModuleTabs()
        {
            if (modules.Count != 0) // Only Update, if there are any module-Tabs
            {
                for (int i = 0; i < modules.Count; i++)
                {
                    modules[i].Update(device.Modules[i]);
                }
            }
        }

        private void CreateModuleTabs()
        {
            if (device.Modules.Length <= 0) // Check for modules
            {
                return;                     // Return, when no modules were found
            }

            int x = 0;
            foreach (var item in device.Modules)
            {
                TabItem dev = (TabItem)moduleTabs.Items[x];
                dev.Header = item.ModuleName;
                dev.Visibility = Visibility.Visible;
                x++;
            }
            if (device.Modules.Length >= 1)
                modules.Add(UserControlDevice0);
            if (device.Modules.Length >= 2)
                modules.Add(UserControlDevice1);
            if (device.Modules.Length >= 3)
                modules.Add(UserControlDevice2);
            if (device.Modules.Length >= 4)
                modules.Add(UserControlDevice3);
            if (device.Modules.Length >= 5)
                modules.Add(UserControlDevice4);

            for (int i = 0; i < modules.Count; i++) // Initialize every module
            {
                modules[i].Init(device.Modules[i]);
            }
        }
    }
}
