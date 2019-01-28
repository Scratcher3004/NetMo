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
        public static List<NetmoDevice> nd = new List<NetmoDevice>();
        private Device device;
        private int deviceID;
        private bool refreshing = false;
        private DispatcherTimer loadingTimer;

        public NetmoDevice()
        {
            InitializeComponent();
            deviceID = nd.Count;
            nd.Add(this);
            Loading.Visibility = Visibility.Hidden;
            Loading.IsEnabled = false;
        }

        public void Init(Device device_)
        {
            // Store device
            device = device_;
            DisplayData();
        }

        public async void Update(object sender, RoutedEventArgs e)
        {
            // Show Loading Symbol and Update Async
            refreshing = true;
            Loading.Visibility = Visibility.Visible;
            Loading.IsEnabled = true;
            DownloadDataAsyncAndUpdate();

            // Loading Animation
            Loading.RenderTransform = new RotateTransform(0);
            Loading.RenderTransformOrigin = new Point(0.5, 0.5);
            loadingTimer = new DispatcherTimer();
            loadingTimer.Tick += RotateWhileRefeshing;
            loadingTimer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            loadingTimer.Start();
        }

        private async void DownloadDataAsyncAndUpdate()
        {
            device = await MainWindow.mw.GetDeviceData(deviceID);
            DisplayData();
            refreshing = false;
            Loading.Visibility = Visibility.Hidden;
            Loading.IsEnabled = false;
            loadingTimer.Stop();
        }

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

    public enum TextBlockIdentifierType
    {
        ByText,
        ByID
    }
}
