using System;
using System.Windows;
using ReportingStructures;

namespace CricketStatsCalc
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class AddPlayerForm : Window
    {
        public AddPlayerForm()
        {
            Action closethis = new Action(this.Close);
            Action ErrorReporting = new Action(function);
            AddPlayerFormViewModel Data = new AddPlayerFormViewModel(closethis, ErrorReporting);
            InitializeComponent();

            DataContext = Data;
        }

        public void function()
        {
            ErrorReportsWindow ErrorsWindow = new ErrorReportsWindow();
            ErrorsWindow.ShowDialog();
        }
    }
}
