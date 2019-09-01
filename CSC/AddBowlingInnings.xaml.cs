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

namespace CricketStatsCalc
{
    /// <summary>
    /// Interaction logic for AddBowlingInnings.xaml
    /// </summary>
    public partial class AddBowlingInnings : Window
    {
        Cricket_Match Latest = Globals.GamesPlayed.Last<Cricket_Match>();

        public AddBowlingInnings()
        {
            InitializeComponent();
            if (Latest.FPlayerNames[0] != null)
            {
                Player1.Text = Latest.FPlayerNames[0];
            }
            if (Latest.FPlayerNames[1] != null)
            {
                Player2.Text = Latest.FPlayerNames[1];
            }
            if (Latest.FPlayerNames[2] != null)
            {
                Player3.Text = Latest.FPlayerNames[2];
            }
            if (Latest.FPlayerNames[3] != null)
            {
                Player4.Text = Latest.FPlayerNames[3];
            }
            if (Latest.FPlayerNames[4] != null)
            {
                Player5.Text = Latest.FPlayerNames[4];
            }
            if (Latest.FPlayerNames[5] != null)
            {
                Player6.Text = Latest.FPlayerNames[5];
            }
            if (Latest.FPlayerNames[6] != null)
            {
                Player7.Text = Latest.FPlayerNames[6];
            }
            if (Latest.FPlayerNames[7] != null)
            {
                Player8.Text = Latest.FPlayerNames[7];
            }
            if (Latest.FPlayerNames[8] != null)
            {
                Player9.Text = Latest.FPlayerNames[8];
            }
            if (Latest.FPlayerNames[9] != null)
            {
                Player10.Text = Latest.FPlayerNames[9];
            }
            if (Latest.FPlayerNames[10] != null)
            {
                Player11.Text = Latest.FPlayerNames[10];
            }
        }

        void GoToFielidngClick(object sender, RoutedEventArgs e)
        {
            List<int> overs = Globals.DataCleanse(P1Overs.Text, P2Overs.Text, P3Overs.Text, P4Overs.Text, P5Overs.Text, P6Overs.Text, P7Overs.Text, P8Overs.Text, P9Overs.Text, P10Overs.Text, P11Overs.Text);

            List<int> Maidens = Globals.DataCleanse(P1Mdns.Text, P2Mdns.Text, P3Mdns.Text, P4Mdns.Text, P5Mdns.Text, P6Mdns.Text, P7Mdns.Text, P8Mdns.Text, P9Mdns.Text, P10Mdns.Text, P11Mdns.Text);

            List<int> Runs = Globals.DataCleanse(P1RunsConc.Text, P2RunsConc.Text, P3RunsConc.Text, P4RunsConc.Text, P5RunsConc.Text, P6RunsConc.Text, P7RunsConc.Text, P8RunsConc.Text, P9RunsConc.Text, P10RunsConc.Text, P11RunsConc.Text);

            List<int> Wickets = Globals.DataCleanse(P1Wkts.Text, P2Wkts.Text, P3Wkts.Text, P4Wkts.Text, P5Wkts.Text, P6Wkts.Text, P7Wkts.Text, P8Wkts.Text, P9Wkts.Text, P10Wkts.Text, P11Wkts.Text);
            
            Latest.FBowling.Add_Data(overs, Maidens, Runs, Wickets);
            Globals.GamesPlayed[Globals.GamesPlayed.Count() - 1] = Latest;

            // next step is to go to the form for inputting fielding data
            AddFieldingForm AddFieldingWindow = new AddFieldingForm();
            AddFieldingWindow.Show();
            Close();

        }
    }
}
