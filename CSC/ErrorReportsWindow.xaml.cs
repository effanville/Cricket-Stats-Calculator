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
using System.Windows.Shapes;
using Cricket;
using ReportingStructures;

namespace CricketStatsCalc
{

    public class TypeandReport
    {
        public TypeandReport(ReportType A, string B)
        {
            TypeofReport = A;
            report = B;
        }

        public ReportType TypeofReport { get; set; }

        public string report { get; set; }
    }

    /// <summary>
    /// Interaction logic for ErrorReportsWindow.xaml
    /// </summary>
    public partial class ErrorReportsWindow : Window
    {
        public bool BackForward = true;

        public ErrorReportsWindow()
        {
            InitializeComponent();
            if (ErrorReports.GetErrors().Count != 0)
            {
                GoBackButton.Visibility = Visibility.Hidden;
                GoBackButton.Visibility = Visibility.Collapsed;
            }

            List<TypeandReport> ReportsErrors = new List<TypeandReport>();

            List<string> errors = ErrorReports.GetErrors();
            if (errors.Count > 0)
            {
                foreach (string Error in ErrorReports.GetErrors())
                {
                    TypeandReport Dummy = new TypeandReport(ReportType.Error, Error);
                    ReportsErrors.Add(Dummy);
                }
            }

            List<string> warnings = ErrorReports.GetWarnings();
            if (warnings.Count > 0)
            {
                foreach (string warning in ErrorReports.GetWarnings())
                {
                    TypeandReport Dummy = new TypeandReport(ReportType.Warning, warning);
                    ReportsErrors.Add(Dummy);
                }
            }

            List<string> reports = ErrorReports.GetReport();
            if (reports.Count > 0)
            {
                foreach (string report in ErrorReports.GetReport())
                {
                    TypeandReport Dummy = new TypeandReport(ReportType.Report, report);
                    ReportsErrors.Add(Dummy);
                }
            }


            KnownReports.ItemsSource = ReportsErrors;
        }

        void ContinuePress(object sender, RoutedEventArgs e)
        {
            ErrorReports.Clear();
            BackForward = true;
            Close();
        }

        void GoBackPress(object sender, RoutedEventArgs e)
        {
            BackForward = false;
            ErrorReports.Clear();
            Close();
        }
        
    }
}
