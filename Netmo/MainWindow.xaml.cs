using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.IO;
using Newtonsoft.Json;
using System.Configuration;
using System.Security.Cryptography;
using System.Threading;



namespace Netmo
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow mw;
        public List<TabItem> TabDevices = new List<TabItem>();

        private const string URL = "https://api.netatmo.com/oauth2/token";
        private const string ENCRYPTKEY = "NetMo2019";

        //System.Net.HttpWebRequest request;
        NetmoSettings settings;
        Connector connector;
        //Token token;
        //Welcome welcome;

        public class Scope{

            public int expires_in = 0;
            public int expire_in = 0;
        }



        public MainWindow()
        {
            mw = this;
            InitializeComponent();

            settings = LoadSettings();
            if (settings != null)
            {
                settings.Password = Encryption.DecryptString(settings.Password, ENCRYPTKEY);
                this.txtDeviceId.Text = settings.DeviceId;
                this.txtName.Text = settings.Username;
                this.txtPassword.Password = settings.Password;
                this.txtClientSecret.Text = settings.ClientSecret;
                this.txtClientId.Text = settings.ClientId;
            }
            else {
                MessageBox.Show("No settings found, please configure and save the settings!");
            }

            //var nvc = new List<KeyValuePair<string, string>>();
            //nvc.Add(new KeyValuePair<string, string>("client_id", "5c18ff3c6b0affcc188bbe4a"));
            //nvc.Add(new KeyValuePair<string, string>("client_secret", "SjHyLVQnhaougeMNW9ahihewPGjbEiCl2zS"));
            //nvc.Add(new KeyValuePair<string, string>("grant_type", "password"));
            //nvc.Add(new KeyValuePair<string, string>("username", System.Configuration.ConfigurationManager.AppSettings["usr"]));
            //nvc.Add(new KeyValuePair<string, string>("password", System.Configuration.ConfigurationManager.AppSettings["pwd"]));

            connector = new Connector(settings.ClientId, settings.ClientSecret, settings.Username, settings.Password);

            int i = 0;
            foreach (var item in NetmoDevice.nd)
            {
                item.Init(connector.GetNetatmoWeatherData().Body.Devices[i]);
                i++;
            }
            var netatmodata = connector.GetNetatmoWeatherData();
            GenerateTabs(netatmodata);
        }

        public async Task<Device> GetDeviceData(int deviceID)
        {
            // Return new Information
            return (await connector.GetNetatmoWeatherDataAsync()).Body.Devices[deviceID];
        }

        private void SaveSettings(NetmoSettings _data) {
            string executingpath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            FileInfo fi = new FileInfo(executingpath);
            string pfad = fi.DirectoryName;
            using (StreamWriter file = File.CreateText(pfad + System.IO.Path.DirectorySeparatorChar + "netmo.settings"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, _data);

            }
        }

        public NetmoSettings LoadSettings()
        {
            NetmoSettings settings = null;
            string executingpath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            FileInfo fi = new FileInfo(executingpath);
            string settingsfile = fi.DirectoryName + System.IO.Path.DirectorySeparatorChar + "netmo.settings";
            if (!File.Exists(settingsfile)) return null;

            using (StreamReader sr = new StreamReader(settingsfile))
            {
                string filetext  = sr.ReadToEnd();
                settings = JsonConvert.DeserializeObject<NetmoSettings>(filetext);
            }
            return settings;
        }


        private void GenerateTabs(NetAtmoResponse netAtmoResponse)
        {
            int x = 1;
            foreach (var dev in netAtmoResponse.Body.Devices) {
                TabItem item = (TabItem)tabControl.Items[x];
                item.Header = dev.ModuleName;
                item.Visibility = Visibility.Visible;

                //var temptext = (TextBlock)UserControlDevice0.innergrid.Children[10];
                //temptext.Text = "Temperature: " + dev.DashboardData.Temperature.ToString();
                //var resgrid = (Grid)((TabItem)tabControl.Items[x]).FindName("NetatmoGrid" + (x-1).ToString());

                //GenerateGridDefenitions(resgrid, 9, 9);

                //TextBlock tb = new TextBlock();
                //tb.Text = "Temperature:";
                //resgrid.Children.Add(tb);
                //TextBlock tb1 = new TextBlock();
                //resgrid.Children.Add(tb1);
                //TextBlock tb2 = new TextBlock();
                //resgrid.Children.Add(tb2);



                //var text = (TextBlock)resgrid.Children[2];
                //text.Text = "WEl: "; //+ netAtmoResponse.Body.Devices[x-1].Modules[0].DashboardData.Temperature.ToString();
                //resgrid.InvalidateVisual();

                //text.Text = "Temperature: " + netAtmoResponse.Body.Devices[x-1].Modules[0].DashboardData.Temperature.ToString();               
                //var resgrid = (Grid)((TabItem)tabControl.Items[x]).FindResource("DeviceGrid");
                //var text = (TextBlock)resgrid.Children[1];
                //text.Text = "Temperature: " + netAtmoResponse.Body.Devices[x-1].Modules[0].DashboardData.Temperature.ToString();
            }
        }

        private void GenerateGridDefenitions(Grid grid, int columns, int rows)
        {
            for (int x = 0; x < rows; x++)
            {
                RowDefinition rowDefinition = new RowDefinition
                {
                    Height = GridLength.Auto
                };
                grid.RowDefinitions.Add(rowDefinition);
            }

            for (int y = 0; y < columns; y++)
            {
                ColumnDefinition columnDefinition = new ColumnDefinition
                {
                    Width = GridLength.Auto
                };
                grid.ColumnDefinitions.Add(columnDefinition);
            }


        }


        private void StackPanel_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Das Clickevent wurde im Stackpanel behandelt!");
            e.Handled = true;
        }

        private void Window_Click(object sender, RoutedEventArgs e)
        {
           // MessageBox.Show("Das Clickevent wurde im Window behandelt!");
        }

        private void Button_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            //MessageBox.Show("Behandelt beim Button!");
        }

        private void Window_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            //MessageBox.Show("Behandelt beim Window!");
            e.Handled = true;
        }

        private void BtnSpeichern_Click(object sender, RoutedEventArgs e)
        {
            settings = new NetmoSettings
            {
                ClientId = txtClientId.Text,
                ClientSecret = txtClientSecret.Text,
                Password = Encryption.EncryptString(txtPassword.Password.ToString(), ENCRYPTKEY),
                Username = txtName.Text,
                DeviceId = txtDeviceId.Text
            };
            SaveSettings(settings);
            MessageBox.Show("Die Einstellungen wurden gespeichert");
        }
    }

    public class NetmoSettings
    {

        public string ClientId = string.Empty;
        public string ClientSecret = string.Empty;
        public string Username = string.Empty;
        public string Password = string.Empty;
        public string DeviceId = string.Empty;
    }

}
