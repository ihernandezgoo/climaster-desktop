using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Threading;

namespace Climaster_feature
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            // Handle unhandled exceptions
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(
                $"❌ Error no controlado:\n\n{e.Exception.Message}\n\nDetalles:\n{e.Exception.StackTrace}", 
                "Error de Aplicación", 
                MessageBoxButton.OK, 
                MessageBoxImage.Error);

            // Mark as handled to prevent app crash
            e.Handled = true;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
            {
                MessageBox.Show(
                    $"❌ Error crítico:\n\n{ex.Message}\n\nLa aplicación se cerrará.", 
                    "Error Crítico", 
                    MessageBoxButton.OK, 
                    MessageBoxImage.Error);
            }
        }
    }
}
