using System.Windows;

namespace Climaster_feature
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            // Cleanup when closing
            Closing += MainWindow_Closing;
        }

        private void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            // Stop server if running
            if (DataContext is ViewModels.MainViewModel viewModel)
            {
                if (viewModel.IsServerRunning)
                {
                    viewModel.StopServerCommand.Execute(null);
                }
            }
        }
    }
}