using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using climaster.ViewModels;

namespace climaster
{
    // MainWindow-rako elkarreragin-logika
    public partial class MainWindow : Window
    {
        private readonly WeatherViewModel _viewModel;
        private WeatherWidget? _widget;

        public MainWindow()
        {
            InitializeComponent();
            
            _viewModel = new WeatherViewModel();
            DataContext = _viewModel;

            Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // ViewModel-a hasieratu
            await _viewModel.InitializeAsync();

            // Widget-a sortu eta erakutsi
            ShowWidget();

            // Leiho nagusia ezkutatu (ez minimizatu)
            Hide();
        }

        private void ShowWidget()
        {
            if (_widget == null || !_widget.IsLoaded)
            {
                _widget = new WeatherWidget(_viewModel);
                _widget.Show();
            }
        }

        private async void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            await _viewModel.RefreshWeatherAsync();
        }

        private void HideButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        private void ShowConfig_Click(object sender, RoutedEventArgs e)
        {
            Show();
            WindowState = WindowState.Normal;
            Activate();
        }

        private void TrayIcon_TrayMouseDoubleClick(object sender, RoutedEventArgs e)
        {
            ShowConfig_Click(sender, e);
        }

        // Eguraldiaren informazio leihoa ireki
        private async void UseMyLocation_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                button.IsEnabled = false;
                button.Content = "🔄 Kokapena lortzen...";
            }

            try
            {
                var success = await _viewModel.UseMyLocationAsync();
                
                if (success)
                {
                    MessageBox.Show(
                        $"Kokapena detektatu da: {_viewModel.Settings.LocationName}\n" +
                        $"Lat: {_viewModel.Settings.Latitude:F4}\n" +
                        $"Lon: {_viewModel.Settings.Longitude:F4}",
                        "Kokapena Eguneratuta",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show(
                        "Ezin izan da zure kokapena lortu.\n\n" +
                        "Ziurtatu:\n" +
                        "1. Windows-en kokapen-zerbitzua gaituta dagoela\n" +
                        "2. Aplikazio honi baimenak emanda dituzula\n" +
                        "3. Internetera konektatuta zaudela\n\n" +
                        $"Errorea: {_viewModel.ErrorMessage}",
                        "Kokapen Errorea",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                }
            }
            finally
            {
                if (button != null)
                {
                    button.IsEnabled = true;
                    button.Content = "📍 Nire Oraingo Kokapena Erabili";
                }
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            // Erretiluko ikonoaren Dispose
            TrayIcon?.Dispose();
          
            
            // Widget-a itxi
            _widget?.Close();
            
            // Aplikazioa itxi
            Application.Current.Shutdown();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            // Itxi beharrean, ezkutatu
            e.Cancel = true;
            Hide();
        }
    }
}