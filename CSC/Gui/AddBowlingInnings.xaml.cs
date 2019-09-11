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
        public int GameIndex = -1;
        Cricket_Match Latest;

        public AddBowlingInnings(int index)
        {
            GameIndex = index;

            InitializeComponent();

            if (GameIndex == -1)
            {
                Latest = Globals.GamesPlayed.Last<Cricket_Match>();
            }
            if (GameIndex > -1)
            {
                Latest = Globals.GamesPlayed[GameIndex];

                P1Overs.Text = Globals.GamesPlayed[GameIndex].FBowling.FOvers_Bowled[0].ToString();
                P2Overs.Text = Globals.GamesPlayed[GameIndex].FBowling.FOvers_Bowled[1].ToString();
                P3Overs.Text = Globals.GamesPlayed[GameIndex].FBowling.FOvers_Bowled[2].ToString();
                P4Overs.Text = Globals.GamesPlayed[GameIndex].FBowling.FOvers_Bowled[3].ToString();
                P5Overs.Text = Globals.GamesPlayed[GameIndex].FBowling.FOvers_Bowled[4].ToString();
                P6Overs.Text = Globals.GamesPlayed[GameIndex].FBowling.FOvers_Bowled[5].ToString();
                P7Overs.Text = Globals.GamesPlayed[GameIndex].FBowling.FOvers_Bowled[6].ToString();
                P8Overs.Text = Globals.GamesPlayed[GameIndex].FBowling.FOvers_Bowled[7].ToString();
                P9Overs.Text = Globals.GamesPlayed[GameIndex].FBowling.FOvers_Bowled[8].ToString();
                P10Overs.Text = Globals.GamesPlayed[GameIndex].FBowling.FOvers_Bowled[9].ToString();
                P11Overs.Text = Globals.GamesPlayed[GameIndex].FBowling.FOvers_Bowled[10].ToString();

                P1Mdns.Text = Globals.GamesPlayed[GameIndex].FBowling.FMaidens[0].ToString();
                P2Mdns.Text = Globals.GamesPlayed[GameIndex].FBowling.FMaidens[1].ToString();
                P3Mdns.Text = Globals.GamesPlayed[GameIndex].FBowling.FMaidens[2].ToString();
                P4Mdns.Text = Globals.GamesPlayed[GameIndex].FBowling.FMaidens[3].ToString();
                P5Mdns.Text = Globals.GamesPlayed[GameIndex].FBowling.FMaidens[4].ToString();
                P6Mdns.Text = Globals.GamesPlayed[GameIndex].FBowling.FMaidens[5].ToString();
                P7Mdns.Text = Globals.GamesPlayed[GameIndex].FBowling.FMaidens[6].ToString();
                P8Mdns.Text = Globals.GamesPlayed[GameIndex].FBowling.FMaidens[7].ToString();
                P9Mdns.Text = Globals.GamesPlayed[GameIndex].FBowling.FMaidens[8].ToString();
                P10Mdns.Text = Globals.GamesPlayed[GameIndex].FBowling.FMaidens[9].ToString();
                P11Mdns.Text = Globals.GamesPlayed[GameIndex].FBowling.FMaidens[10].ToString();

                P1RunsConc.Text = Globals.GamesPlayed[GameIndex].FBowling.FRuncs_Conceded[0].ToString();
                P2RunsConc.Text = Globals.GamesPlayed[GameIndex].FBowling.FRuncs_Conceded[1].ToString();
                P3RunsConc.Text = Globals.GamesPlayed[GameIndex].FBowling.FRuncs_Conceded[2].ToString();
                P4RunsConc.Text = Globals.GamesPlayed[GameIndex].FBowling.FRuncs_Conceded[3].ToString();
                P5RunsConc.Text = Globals.GamesPlayed[GameIndex].FBowling.FRuncs_Conceded[4].ToString();
                P6RunsConc.Text = Globals.GamesPlayed[GameIndex].FBowling.FRuncs_Conceded[5].ToString();
                P7RunsConc.Text = Globals.GamesPlayed[GameIndex].FBowling.FRuncs_Conceded[6].ToString();
                P8RunsConc.Text = Globals.GamesPlayed[GameIndex].FBowling.FRuncs_Conceded[7].ToString();
                P9RunsConc.Text = Globals.GamesPlayed[GameIndex].FBowling.FRuncs_Conceded[8].ToString();
                P10RunsConc.Text = Globals.GamesPlayed[GameIndex].FBowling.FRuncs_Conceded[9].ToString();
                P11RunsConc.Text = Globals.GamesPlayed[GameIndex].FBowling.FRuncs_Conceded[10].ToString();

                P1Wkts.Text = Globals.GamesPlayed[GameIndex].FBowling.FWickets[0].ToString();
                P2Wkts.Text = Globals.GamesPlayed[GameIndex].FBowling.FWickets[1].ToString();
                P3Wkts.Text = Globals.GamesPlayed[GameIndex].FBowling.FWickets[2].ToString();
                P4Wkts.Text = Globals.GamesPlayed[GameIndex].FBowling.FWickets[3].ToString();
                P5Wkts.Text = Globals.GamesPlayed[GameIndex].FBowling.FWickets[4].ToString();
                P6Wkts.Text = Globals.GamesPlayed[GameIndex].FBowling.FWickets[5].ToString();
                P7Wkts.Text = Globals.GamesPlayed[GameIndex].FBowling.FWickets[6].ToString();
                P8Wkts.Text = Globals.GamesPlayed[GameIndex].FBowling.FWickets[7].ToString();
                P9Wkts.Text = Globals.GamesPlayed[GameIndex].FBowling.FWickets[8].ToString();
                P10Wkts.Text = Globals.GamesPlayed[GameIndex].FBowling.FWickets[9].ToString();
                P11Wkts.Text = Globals.GamesPlayed[GameIndex].FBowling.FWickets[10].ToString();
            }

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
            if (GameIndex < 0)
            {
                Globals.GamesPlayed[Globals.GamesPlayed.Count() - 1] = Latest;

                // next step is to go to the form for inputting fielding data
                AddFieldingForm AddFieldingWindow = new AddFieldingForm(-1);
                AddFieldingWindow.Show();
            }
            if (GameIndex > -1)
            {
                Globals.GamesPlayed[GameIndex] = Latest;
                AddFieldingForm AddFieldingWindow = new AddFieldingForm(GameIndex);
                AddFieldingWindow.Show();
            }
            Close();

        }
    }
}
