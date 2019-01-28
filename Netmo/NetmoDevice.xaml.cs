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
        private bool refreshing = false;                                // Show Refresh Loading Symbol?
        private DispatcherTimer loadingTimer;                           // Timer for Refresh Loading Symbol rotation

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
            DisplayData();
        }

        /// <summary>
        /// Called when Refresh-Button was Clicked
        /// </summary>
        /// <param name="sender">Unused param</param>
        /// <param name="e">Unused param</param>
        public async void Update(object sender, RoutedEventArgs e)
        {
            // Show Loading Symbol and Update Async
            refreshing = true;
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
            refreshing = false;
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

            // Util Display
            humidity.Text = "Humidity: " + data.Humidity;
            presure.Text = "Presure: " + data.Pressure;
            co2.Text = "CO2: " + data.Co2;
        }
    }
}
