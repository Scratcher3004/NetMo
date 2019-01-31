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

namespace Netmo
{
    /// <summary>
    /// Interaktionslogik für Module.xaml
    /// </summary>
    public partial class NetmoModule : UserControl
    {
        Module module;
        double startHeight = 155;

        public NetmoModule()
        {
            InitializeComponent();
            startHeight = page.Height;
        }

        public void Init(Module module_)    // Initialize
        {
            module = module_;
            DisplayData();
        }

        public void Update(Module module_)  // Update by re-Initializing
        {
            Init(module_);
        }

        /// <summary>
        /// Displays the current Device-Data
        /// </summary>
        public void DisplayData()
        {
            page.Height = startHeight;
            ModuleDashboardData data = module.DashboardData;

            // Temperature Display
            temp.Text = "Temperature: " + data.Temperature;
            minTemp.Text = "Min Temperature: " + data.MinTemp;
            maxTemp.Text = "Max Temperature: " + data.MaxTemp;

            // Util Display
            humidity.Text = "Humidity: " + data.Humidity;

            if (data.Co2 != null) // Only display CO2 if the module has sent CO2-data
            {
                co2.Text = "CO2: " + data.Co2;
                co2.Visibility = Visibility.Visible;
                co2sign.Visibility = Visibility.Visible;
            }
            else // Else: Hide CO2
            {
                co2.Visibility = Visibility.Hidden;
                co2sign.Visibility = Visibility.Hidden;
                page.Height -= 30;
            }

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
        }
    }
}
