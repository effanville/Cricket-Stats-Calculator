using System.Windows;
using Cricket;
using ReportingStructures;
using CricketDatabaseEditing;

namespace CricketStatsCalc
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class AddPlayerForm : Window
    {
        public AddPlayerForm()
        {
            InitializeComponent();

        }

        void AddNameClick(object sender, RoutedEventArgs e)
        {
            if (PlayerAddBox.Text != "Input Player Name Here")
            {
                if (CricketDatabaseEditingFunctions.AddPlayer(PlayerAddBox.Text))
                {
                    PlayerAddBox.Clear();
                    Close();
                }
            }
            else
            {
                ErrorReports.AddError("User has not specified a name for new player.");
            }

            if (ErrorReports.GetErrors().Count != 0)
            {
                ErrorReportsWindow errorReportsWindow = new ErrorReportsWindow();
                errorReportsWindow.Show();
            }

        }


    }
}
