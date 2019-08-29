using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Runtime;
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
    /// Interaction logic for Window3.xaml
    /// </summary>
    public partial class AddBattingInnings : Window
    {
        Cricket_Match Latest = Globals.GamesPlayed.Last<Cricket_Match>();

        public AddBattingInnings()
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

            Player1OutMethod.ItemsSource = Enum.GetValues(typeof(OutType)).Cast<OutType>();
            
            Player2OutMethod.ItemsSource = Enum.GetValues(typeof(OutType)).Cast<OutType>();
            Player3OutMethod.ItemsSource = Enum.GetValues(typeof(OutType)).Cast<OutType>();
            Player4OutMethod.ItemsSource = Enum.GetValues(typeof(OutType)).Cast<OutType>();
            Player5OutMethod.ItemsSource = Enum.GetValues(typeof(OutType)).Cast<OutType>();
            Player6OutMethod.ItemsSource = Enum.GetValues(typeof(OutType)).Cast<OutType>();
            Player7OutMethod.ItemsSource = Enum.GetValues(typeof(OutType)).Cast<OutType>();
            Player8OutMethod.ItemsSource = Enum.GetValues(typeof(OutType)).Cast<OutType>();
            Player9OutMethod.ItemsSource = Enum.GetValues(typeof(OutType)).Cast<OutType>();
            Player10OutMethod.ItemsSource = Enum.GetValues(typeof(OutType)).Cast<OutType>();
            Player11OutMethod.ItemsSource = Enum.GetValues(typeof(OutType)).Cast<OutType>();

            Player1OutMethod.SelectedIndex = 7;
            Player2OutMethod.SelectedIndex = 7;
            Player3OutMethod.SelectedIndex = 7;
            Player4OutMethod.SelectedIndex = 7;
            Player5OutMethod.SelectedIndex = 7;
            Player6OutMethod.SelectedIndex = 7;
            Player7OutMethod.SelectedIndex = 7;
            Player8OutMethod.SelectedIndex = 7;
            Player9OutMethod.SelectedIndex = 7;
            Player10OutMethod.SelectedIndex = 7;
            Player11OutMethod.SelectedIndex = 7;
        }

        void GoToBowling_Click(object sender, RoutedEventArgs e)
        {
            // vector of un modified input data from user
            List<string> runsdata = new List<string>();
            runsdata.Add(Player1_Runs.Text);
            runsdata.Add(Player2_Runs.Text);
            runsdata.Add(Player3_Runs.Text);
            runsdata.Add(Player4_Runs.Text);
            runsdata.Add(Player5_Runs.Text);
            runsdata.Add(Player6_Runs.Text);
            runsdata.Add(Player7_Runs.Text);
            runsdata.Add(Player8_Runs.Text);
            runsdata.Add(Player9_Runs.Text);
            runsdata.Add(Player10_Runs.Text);
            runsdata.Add(Player11_Runs.Text);

            // vector of runs data to be added to batting innings
            List<int> runs = new List<int>(new int[11]);


            int result = 0;
            for (int i = 0; i < runsdata.Count; i++)
            {
                result = 0;
                // if user entered a value, input that, otherwise, return 0 runs scored.
                runs[i] = int.TryParse(runsdata[i], out result) ? result : 0;
            }          

            List<OutType> HowOut = new List<OutType>();
            HowOut.Add((OutType)Player1OutMethod.SelectedValue);
            HowOut.Add((OutType)Player2OutMethod.SelectedValue);
            HowOut.Add((OutType)Player3OutMethod.SelectedValue);
            HowOut.Add((OutType)Player4OutMethod.SelectedValue);
            HowOut.Add((OutType)Player5OutMethod.SelectedValue);
            HowOut.Add((OutType)Player6OutMethod.SelectedValue);
            HowOut.Add((OutType)Player7OutMethod.SelectedValue);
            HowOut.Add((OutType)Player8OutMethod.SelectedValue);
            HowOut.Add((OutType)Player9OutMethod.SelectedValue);
            HowOut.Add((OutType)Player10OutMethod.SelectedValue);
            HowOut.Add((OutType)Player11OutMethod.SelectedValue);

            int noExtras = 0;
            Int32.TryParse(Extras.Text, out noExtras);


            Latest.FBatting.Set_Data(runs, HowOut, noExtras);
            Globals.GamesPlayed[Globals.GamesPlayed.Count()-1] = Latest;

            AddBowlingInnings AddBowlingWindow = new AddBowlingInnings();
            AddBowlingWindow.Show();
            Close();
        }
    }
}
