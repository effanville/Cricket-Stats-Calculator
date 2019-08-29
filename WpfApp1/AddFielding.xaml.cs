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
            List<int> catches = new List<int>();
            int result = 0;
            catches.Add(Int32.TryParse(P1cat.Text, out result) ? result : 0);
            catches.Add(Int32.TryParse(P2cat.Text, out result) ? result : 0);
            catches.Add(Int32.TryParse(P3cat.Text, out result) ? result : 0);
            catches.Add(Int32.TryParse(P4cat.Text, out result) ? result : 0);
            catches.Add(Int32.TryParse(P5cat.Text, out result) ? result : 0);
            catches.Add(Int32.TryParse(P6cat.Text, out result) ? result : 0);
            catches.Add(Int32.TryParse(P7cat.Text, out result) ? result : 0);
            catches.Add(Int32.TryParse(P8cat.Text, out result) ? result : 0);
            catches.Add(Int32.TryParse(P9cat.Text, out result) ? result : 0);
            catches.Add(Int32.TryParse(P10cat.Text, out result) ? result : 0);
            catches.Add(Int32.TryParse(P11cat.Text, out result) ? result : 0);

            List<int> ro = new List<int>();
            ro.Add(Int32.TryParse(P1RO.Text, out result) ? result : 0);
            ro.Add(Int32.TryParse(P2RO.Text, out result) ? result : 0);
            ro.Add(Int32.TryParse(P3RO.Text, out result) ? result : 0);
            ro.Add(Int32.TryParse(P4RO.Text, out result) ? result : 0);
            ro.Add(Int32.TryParse(P5RO.Text, out result) ? result : 0);
            ro.Add(Int32.TryParse(P6RO.Text, out result) ? result : 0);
            ro.Add(Int32.TryParse(P7RO.Text, out result) ? result : 0);
            ro.Add(Int32.TryParse(P8RO.Text, out result) ? result : 0);
            ro.Add(Int32.TryParse(P9RO.Text, out result) ? result : 0);
            ro.Add(Int32.TryParse(P10RO.Text, out result) ? result : 0);
            ro.Add(Int32.TryParse(P11RO.Text, out result) ? result : 0);

            List<int> st = new List<int>();
            st.Add(Int32.TryParse(P1WS.Text, out result) ? result : 0);
            st.Add(Int32.TryParse(P2WS.Text, out result) ? result : 0);
            st.Add(Int32.TryParse(P3WS.Text, out result) ? result : 0);
            st.Add(Int32.TryParse(P4WS.Text, out result) ? result : 0);
            st.Add(Int32.TryParse(P5WS.Text, out result) ? result : 0);
            st.Add(Int32.TryParse(P6WS.Text, out result) ? result : 0);
            st.Add(Int32.TryParse(P7WS.Text, out result) ? result : 0);
            st.Add(Int32.TryParse(P8WS.Text, out result) ? result : 0);
            st.Add(Int32.TryParse(P9WS.Text, out result) ? result : 0);
            st.Add(Int32.TryParse(P10WS.Text, out result) ? result : 0);
            st.Add(Int32.TryParse(P11WS.Text, out result) ? result : 0);

            List<int> keepcat = new List<int>();
            keepcat.Add(Int32.TryParse(P1WCat.Text, out result) ? result : 0);
            keepcat.Add(Int32.TryParse(P2WCat.Text, out result) ? result : 0);
            keepcat.Add(Int32.TryParse(P3WCat.Text, out result) ? result : 0);
            keepcat.Add(Int32.TryParse(P4WCat.Text, out result) ? result : 0);
            keepcat.Add(Int32.TryParse(P5WCat.Text, out result) ? result : 0);
            keepcat.Add(Int32.TryParse(P6WCat.Text, out result) ? result : 0);
            keepcat.Add(Int32.TryParse(P7WCat.Text, out result) ? result : 0);
            keepcat.Add(Int32.TryParse(P8WCat.Text, out result) ? result : 0);
            keepcat.Add(Int32.TryParse(P9WCat.Text, out result) ? result : 0);
            keepcat.Add(Int32.TryParse(P10WCat.Text, out result) ? result : 0);
            keepcat.Add(Int32.TryParse(P11WCat.Text, out result) ? result : 0);

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
