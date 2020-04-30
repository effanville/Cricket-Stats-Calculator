using System;
using System.Windows;
using System.Windows.Threading;

namespace CricketStatisticsDatabase
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            //Handling the exception within the UnhandledExcpeiton handler.
            MessageBox.Show(e.Exception.Message + Environment.NewLine + e.Exception.StackTrace, "Exception Caught", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
        }
    }
}
