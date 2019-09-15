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
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class AddFieldingForm : Window
    {
        public int GameIndex;

        Cricket_Match Latest;

        public AddFieldingForm(int index)
        {
            InitializeComponent();
            GameIndex = index;
            if (GameIndex == -1)
            {
                Latest = Globals.GamesPlayed.Last<Cricket_Match>();
            }
            if (GameIndex > -1)
            {
                Latest = Globals.GamesPlayed[GameIndex];

                P1cat.Text = Globals.GamesPlayed[GameIndex].FFieldingStats.FCatches[0].ToString();
                P2cat.Text = Globals.GamesPlayed[GameIndex].FFieldingStats.FCatches[1].ToString();
                P3cat.Text = Globals.GamesPlayed[GameIndex].FFieldingStats.FCatches[2].ToString();
                P4cat.Text = Globals.GamesPlayed[GameIndex].FFieldingStats.FCatches[3].ToString();
                P5cat.Text = Globals.GamesPlayed[GameIndex].FFieldingStats.FCatches[4].ToString();
                P6cat.Text = Globals.GamesPlayed[GameIndex].FFieldingStats.FCatches[5].ToString();
                P7cat.Text = Globals.GamesPlayed[GameIndex].FFieldingStats.FCatches[6].ToString();
                P8cat.Text = Globals.GamesPlayed[GameIndex].FFieldingStats.FCatches[7].ToString();
                P9cat.Text = Globals.GamesPlayed[GameIndex].FFieldingStats.FCatches[8].ToString();
                P10cat.Text = Globals.GamesPlayed[GameIndex].FFieldingStats.FCatches[9].ToString();
                P11cat.Text = Globals.GamesPlayed[GameIndex].FFieldingStats.FCatches[10].ToString();

                P1RO.Text = Globals.GamesPlayed[GameIndex].FFieldingStats.FRunOuts[0].ToString();
                P2RO.Text = Globals.GamesPlayed[GameIndex].FFieldingStats.FRunOuts[1].ToString();
                P3RO.Text = Globals.GamesPlayed[GameIndex].FFieldingStats.FRunOuts[2].ToString();
                P4RO.Text = Globals.GamesPlayed[GameIndex].FFieldingStats.FRunOuts[3].ToString();
                P5RO.Text = Globals.GamesPlayed[GameIndex].FFieldingStats.FRunOuts[4].ToString();
                P6RO.Text = Globals.GamesPlayed[GameIndex].FFieldingStats.FRunOuts[5].ToString();
                P7RO.Text = Globals.GamesPlayed[GameIndex].FFieldingStats.FRunOuts[6].ToString();
                P8RO.Text = Globals.GamesPlayed[GameIndex].FFieldingStats.FRunOuts[7].ToString();
                P9RO.Text = Globals.GamesPlayed[GameIndex].FFieldingStats.FRunOuts[8].ToString();
                P10RO.Text = Globals.GamesPlayed[GameIndex].FFieldingStats.FRunOuts[9].ToString();
                P11RO.Text = Globals.GamesPlayed[GameIndex].FFieldingStats.FRunOuts[10].ToString();

                P1WCat.Text = Globals.GamesPlayed[GameIndex].FFieldingStats.FCatchesKeeper[0].ToString();
                P2WCat.Text = Globals.GamesPlayed[GameIndex].FFieldingStats.FCatchesKeeper[1].ToString();
                P3WCat.Text = Globals.GamesPlayed[GameIndex].FFieldingStats.FCatchesKeeper[2].ToString();
                P4WCat.Text = Globals.GamesPlayed[GameIndex].FFieldingStats.FCatchesKeeper[3].ToString();
                P5WCat.Text = Globals.GamesPlayed[GameIndex].FFieldingStats.FCatchesKeeper[4].ToString();
                P6WCat.Text = Globals.GamesPlayed[GameIndex].FFieldingStats.FCatchesKeeper[5].ToString();
                P7WCat.Text = Globals.GamesPlayed[GameIndex].FFieldingStats.FCatchesKeeper[6].ToString();
                P8WCat.Text = Globals.GamesPlayed[GameIndex].FFieldingStats.FCatchesKeeper[7].ToString();
                P9WCat.Text = Globals.GamesPlayed[GameIndex].FFieldingStats.FCatchesKeeper[8].ToString();
                P10WCat.Text = Globals.GamesPlayed[GameIndex].FFieldingStats.FCatchesKeeper[9].ToString();
                P11WCat.Text = Globals.GamesPlayed[GameIndex].FFieldingStats.FCatchesKeeper[10].ToString();

                P1WS.Text = Globals.GamesPlayed[GameIndex].FFieldingStats.FStumpings[0].ToString();
                P2WS.Text = Globals.GamesPlayed[GameIndex].FFieldingStats.FStumpings[1].ToString();
                P3WS.Text = Globals.GamesPlayed[GameIndex].FFieldingStats.FStumpings[2].ToString();
                P4WS.Text = Globals.GamesPlayed[GameIndex].FFieldingStats.FStumpings[3].ToString();
                P5WS.Text = Globals.GamesPlayed[GameIndex].FFieldingStats.FStumpings[4].ToString();
                P6WS.Text = Globals.GamesPlayed[GameIndex].FFieldingStats.FStumpings[5].ToString();
                P7WS.Text = Globals.GamesPlayed[GameIndex].FFieldingStats.FStumpings[6].ToString();
                P8WS.Text = Globals.GamesPlayed[GameIndex].FFieldingStats.FStumpings[7].ToString();
                P9WS.Text = Globals.GamesPlayed[GameIndex].FFieldingStats.FStumpings[8].ToString();
                P10WS.Text = Globals.GamesPlayed[GameIndex].FFieldingStats.FStumpings[9].ToString();
                P11WS.Text = Globals.GamesPlayed[GameIndex].FFieldingStats.FStumpings[10].ToString();
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

        private void Submit_Match_Click(object sender, RoutedEventArgs e)
        {

            List<int> catches = Globals.DataCleanse(P1cat.Text, P2cat.Text, P3cat.Text, P4cat.Text, P5cat.Text, P6cat.Text, P7cat.Text, P8cat.Text, P9cat.Text, P10cat.Text, P11cat.Text);

            List<int> ro = Globals.DataCleanse(P1RO.Text, P2RO.Text, P3RO.Text, P4RO.Text, P5RO.Text, P6RO.Text, P7RO.Text, P8RO.Text, P9RO.Text, P10RO.Text, P11RO.Text);

            List<int> st = Globals.DataCleanse(P1WS.Text, P2WS.Text, P3WS.Text, P4WS.Text, P5WS.Text, P6WS.Text, P7WS.Text, P8WS.Text, P9WS.Text, P10WS.Text, P11WS.Text);

            List<int> keepcat = Globals.DataCleanse(P1WCat.Text, P2WCat.Text, P3WCat.Text, P4WCat.Text, P5WCat.Text, P6WCat.Text, P7WCat.Text, P8WCat.Text, P9WCat.Text, P10WCat.Text, P11WCat.Text);

            if (catches.Sum() + ro.Sum() + st.Sum() + keepcat.Sum() > 10)
            {
                ErrorReports.AddError("Total number of fielding dismissals exceeds 10.");
            }
            
            if (!ErrorReports.OkNotOk())
            {
                ErrorReportsWindow ErrorsWindow = new ErrorReportsWindow();
                ErrorsWindow.ShowDialog();
            }
            else
            {
                
                ErrorReportsWindow ErrorsWindow = new ErrorReportsWindow();
                // Only show window if have things to show.
                if (ErrorReports.GetErrors().Count != 0 || ErrorReports.GetWarnings().Count != 0 || ErrorReports.GetReport().Count != 0)
                {

                    ErrorsWindow.ShowDialog();
                }

                if (ErrorsWindow.BackForward)
                {
                    Latest.FFieldingStats.Add_Data(catches, ro, st, keepcat);
                    if (GameIndex == -1)
                    {
                        Globals.GamesPlayed[Globals.GamesPlayed.Count() - 1] = Latest;
                    }
                    if (GameIndex > -1)
                    {
                        Globals.GamesPlayed[GameIndex] = Latest;
                    }
                    foreach (string person in Latest.FPlayerNames)
                    {
                        Cricket_Player A = Globals.GetPlayerFromName(person);
                        if (A != null)
                        {
                            A.Calculated = false;
                        }
                    }

                    Close();
                }
                else
                {
                }
            }
        }
    }
}
