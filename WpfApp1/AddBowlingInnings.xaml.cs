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

namespace WpfApp1
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
            if (Latest.FPlayers[0] != null)
            {
                Player1.Text = Latest.FPlayers[0].Name;
            }
            if (Latest.FPlayers[1] != null)
            {
                Player2.Text = Latest.FPlayers[1].Name;
            }
            if (Latest.FPlayers[2] != null)
            {
                Player3.Text = Latest.FPlayers[2].Name;
            }
            if (Latest.FPlayers[3] != null)
            {
                Player4.Text = Latest.FPlayers[3].Name;
            }
            if (Latest.FPlayers[4] != null)
            {
                Player5.Text = Latest.FPlayers[4].Name;
            }
            if (Latest.FPlayers[5] != null)
            {
                Player6.Text = Latest.FPlayers[5].Name;
            }
            if (Latest.FPlayers[6] != null)
            {
                Player7.Text = Latest.FPlayers[6].Name;
            }
            if (Latest.FPlayers[7] != null)
            {
                Player8.Text = Latest.FPlayers[7].Name;
            }
            if (Latest.FPlayers[8] != null)
            {
                Player9.Text = Latest.FPlayers[8].Name;
            }
            if (Latest.FPlayers[9] != null)
            {
                Player10.Text = Latest.FPlayers[9].Name;
            }
            if (Latest.FPlayers[10] != null)
            {
                Player11.Text = Latest.FPlayers[10].Name;
            }
        }

        void GoToFielidngClick(object sender, RoutedEventArgs e)
        {
            // vector for un-edited user inputted data
            List<string> oversdata = new List<string>();
            oversdata.Add(P1Overs.Text);
            oversdata.Add(P2Overs.Text);
            oversdata.Add(P3Overs.Text);
            oversdata.Add(P4Overs.Text);
            oversdata.Add(P5Overs.Text);
            oversdata.Add(P6Overs.Text);
            oversdata.Add(P7Overs.Text);
            oversdata.Add(P8Overs.Text);
            oversdata.Add(P9Overs.Text);
            oversdata.Add(P10Overs.Text);
            oversdata.Add(P11Overs.Text);

            // vector for overs data for adding to bowling innings
            List<int> overs = new List<int>(new int[11]);

            int result = 0;
            for (int i = 0; i < oversdata.Count; i++)
            {
                result = 0;
                // if user entered a value, input that, otherwise, return 0 runs scored.
                overs[i] = int.TryParse(oversdata[i], out result) ? result : 0;
            }


            // vector for un-edited user inputted data
            List<string> maidensdata = new List<string>();
            maidensdata.Add(P1Overs.Text);
            maidensdata.Add(P2Overs.Text);
            maidensdata.Add(P3Overs.Text);
            maidensdata.Add(P4Overs.Text);
            maidensdata.Add(P5Overs.Text);
            maidensdata.Add(P6Overs.Text);
            maidensdata.Add(P7Overs.Text);
            maidensdata.Add(P8Overs.Text);
            maidensdata.Add(P9Overs.Text);
            maidensdata.Add(P10Overs.Text);
            maidensdata.Add(P11Overs.Text);

            // vector for overs data for adding to bowling innings
            List<int> Maidens = new List<int>(new int[11]);

            //now populate the data to be added
            for (int i = 0; i < maidensdata.Count; i++)
            {
                result = 0;
                // if user entered a value, input that, otherwise, return 0 runs scored.
                Maidens[i] = int.TryParse(maidensdata[i], out result) ? result : 0;
            }

            List<string> runsconcdata = new List<string>();
            runsconcdata.Add(P1RunsConc.Text);
            runsconcdata.Add(P2RunsConc.Text);
            runsconcdata.Add(P3RunsConc.Text);
            runsconcdata.Add(P4RunsConc.Text);
            runsconcdata.Add(P5RunsConc.Text);
            runsconcdata.Add(P6RunsConc.Text);
            runsconcdata.Add(P7RunsConc.Text);
            runsconcdata.Add(P8RunsConc.Text);
            runsconcdata.Add(P9RunsConc.Text);
            runsconcdata.Add(P10RunsConc.Text);
            runsconcdata.Add(P11RunsConc.Text);

            // vector for adding runs conceded to bowling stats
            List<int> Runs = new List<int>(new int[11]);

            for (int i = 0; i < runsconcdata.Count; i++)
            {
                result = 0;
                // if user entered a value, input that, otherwise, return 0 runs scored.
                Runs[i] = int.TryParse(runsconcdata[i], out result) ? result : 0;
            }

            List<string> wicketsdata = new List<string>();
            wicketsdata.Add(P1Wkts.Text);
            wicketsdata.Add(P2Wkts.Text);
            wicketsdata.Add(P3Wkts.Text);
            wicketsdata.Add(P4Wkts.Text);
            wicketsdata.Add(P5Wkts.Text);
            wicketsdata.Add(P6Wkts.Text);
            wicketsdata.Add(P7Wkts.Text);
            wicketsdata.Add(P8Wkts.Text);
            wicketsdata.Add(P9Wkts.Text);
            wicketsdata.Add(P10Wkts.Text);
            wicketsdata.Add(P11Wkts.Text);

            List<int> Wickets = new List<int>(new int[11]);

            for (int i = 0; i < wicketsdata.Count; i++)
            {
                result = 0;
                // if user entered a value, input that, otherwise, return 0 runs scored.
                Wickets[i] = int.TryParse(wicketsdata[i], out result) ? result : 0;
            }
            
            Latest.FBowling.Add_Data(overs, Maidens, Runs, Wickets);
            Globals.GamesPlayed[Globals.GamesPlayed.Count() - 1] = Latest;

            // next step is to go to the form for inputting fielding data
            AddFieldingForm AddFieldingWindow = new AddFieldingForm();
            AddFieldingWindow.Show();
            Close();

        }
    }
}
