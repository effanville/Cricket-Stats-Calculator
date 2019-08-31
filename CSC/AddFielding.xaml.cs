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
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class AddFieldingForm : Window
    {
        Cricket_Match Latest = Globals.GamesPlayed.Last<Cricket_Match>();

        public AddFieldingForm()
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

        private void Submit_Match_Click(object sender, RoutedEventArgs e)
        {
            List<int> catches = Globals.DataCleanse(P1cat.Text, P2cat.Text, P3cat.Text, P4cat.Text, P5cat.Text, P6cat.Text, P7cat.Text, P8cat.Text, P9cat.Text, P10cat.Text, P11cat.Text);

            List<int> ro = Globals.DataCleanse(P1RO.Text, P2RO.Text, P3RO.Text, P4RO.Text, P5RO.Text, P6RO.Text, P7RO.Text, P8RO.Text, P9RO.Text, P10RO.Text, P11RO.Text);

            List<int> st = Globals.DataCleanse(P1WS.Text, P2WS.Text, P3WS.Text, P4WS.Text, P5WS.Text, P6WS.Text, P7WS.Text, P8WS.Text, P9WS.Text, P10WS.Text, P11WS.Text);

            List<int> keepcat = Globals.DataCleanse(P1WCat.Text, P2WCat.Text, P3WCat.Text, P4WCat.Text, P5WCat.Text, P6WCat.Text, P7WCat.Text, P8WCat.Text, P9WCat.Text, P10WCat.Text, P11WCat.Text);

            Latest.FFieldingStats.Add_Data(catches, ro,  st,  keepcat);

            // ensure all players just added have statistics recalculated
            foreach (Cricket_Player person in Latest.FPlayers)
            {
                if(person!=null)
                {
                    person.Calculated = false;
                }              
            }

            Globals.GamesPlayed[Globals.GamesPlayed.Count() - 1] = Latest;
            Close();
        }
    }
}
