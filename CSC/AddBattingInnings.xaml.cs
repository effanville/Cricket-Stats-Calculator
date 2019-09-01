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

namespace CricketStatsCalc
{
    /// <summary>
    /// Interaction logic for Window3.xaml
    /// </summary>
    public partial class AddBattingInnings : Window
    {
        public int GameIndex = -1;
        public Cricket_Match Latest;

        public AddBattingInnings(int index)
        {
            GameIndex = index;
            InitializeComponent();

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

            if (index == -1)
            {
                Latest = Globals.GamesPlayed.Last<Cricket_Match>();

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
            else
            {
                Latest = Globals.GamesPlayed[index];

                Player1OutMethod.SelectedIndex = (int)Globals.GamesPlayed[index].FBatting.FMethod_Out[0];
                Player2OutMethod.SelectedIndex = (int)Globals.GamesPlayed[index].FBatting.FMethod_Out[1];
                Player3OutMethod.SelectedIndex = (int)Globals.GamesPlayed[index].FBatting.FMethod_Out[2];
                Player4OutMethod.SelectedIndex = (int)Globals.GamesPlayed[index].FBatting.FMethod_Out[3];
                Player5OutMethod.SelectedIndex = (int)Globals.GamesPlayed[index].FBatting.FMethod_Out[4];
                Player6OutMethod.SelectedIndex = (int)Globals.GamesPlayed[index].FBatting.FMethod_Out[5];
                Player7OutMethod.SelectedIndex = (int)Globals.GamesPlayed[index].FBatting.FMethod_Out[6];
                Player8OutMethod.SelectedIndex = (int)Globals.GamesPlayed[index].FBatting.FMethod_Out[7];
                Player9OutMethod.SelectedIndex = (int)Globals.GamesPlayed[index].FBatting.FMethod_Out[8];
                Player10OutMethod.SelectedIndex = (int)Globals.GamesPlayed[index].FBatting.FMethod_Out[9];
                Player11OutMethod.SelectedIndex = (int)Globals.GamesPlayed[index].FBatting.FMethod_Out[10];

                Player1_Runs.Text = Globals.GamesPlayed[index].FBatting.FRuns_Scored[0].ToString();
                Player2_Runs.Text = Globals.GamesPlayed[index].FBatting.FRuns_Scored[1].ToString();
                Player3_Runs.Text = Globals.GamesPlayed[index].FBatting.FRuns_Scored[2].ToString();
                Player4_Runs.Text = Globals.GamesPlayed[index].FBatting.FRuns_Scored[3].ToString();
                Player5_Runs.Text = Globals.GamesPlayed[index].FBatting.FRuns_Scored[4].ToString();
                Player6_Runs.Text = Globals.GamesPlayed[index].FBatting.FRuns_Scored[5].ToString();
                Player7_Runs.Text = Globals.GamesPlayed[index].FBatting.FRuns_Scored[6].ToString();
                Player8_Runs.Text = Globals.GamesPlayed[index].FBatting.FRuns_Scored[7].ToString();
                Player9_Runs.Text = Globals.GamesPlayed[index].FBatting.FRuns_Scored[8].ToString();
                Player10_Runs.Text = Globals.GamesPlayed[index].FBatting.FRuns_Scored[9].ToString();
                Player11_Runs.Text = Globals.GamesPlayed[index].FBatting.FRuns_Scored[10].ToString();

                Extras.Text = Globals.GamesPlayed[index].FBatting.fExtras.ToString();
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

        void GoToBowling_Click(object sender, RoutedEventArgs e)
        {

                List<int> runs = Globals.DataCleanse(Player1_Runs.Text, Player2_Runs.Text, Player3_Runs.Text, Player4_Runs.Text, Player5_Runs.Text, Player6_Runs.Text, Player7_Runs.Text, Player8_Runs.Text, Player9_Runs.Text, Player10_Runs.Text, Player11_Runs.Text);


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

            if (GameIndex < 0)
            {
                Globals.GamesPlayed[Globals.GamesPlayed.Count() - 1] = Latest;

                AddBowlingInnings AddBowlingWindow = new AddBowlingInnings(-1);
                AddBowlingWindow.Show();
            }
            if (GameIndex > -1)
            {
                Globals.GamesPlayed[GameIndex] = Latest;
                AddBowlingInnings AddBowlingWindow = new AddBowlingInnings(GameIndex);
                AddBowlingWindow.Show();
            }
            Close();
        }
    }
}
