using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Climaster_feature.Models;
using Climaster_feature.ViewModels;

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

        private void ColorButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is string color && DataContext is MainViewModel vm)
            {
                // Set the base color (without alpha channel)
                vm.BaseColor = color;
            }
        }

        private void ElementBorder_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border && border.Tag is WidgetElement element && DataContext is MainViewModel vm)
            {
                vm.SelectElementCommand.Execute(element);
            }
        }
    }
}