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
    public partial class MatchSelectorForm : Window
    {
        public MatchSelectorForm()
        {
            InitializeComponent();
            ChooseMatch.ItemsSource = Globals.GamesPlayed;
            ChooseMatch.DisplayMemberPath = "FOpposition";
        }

        private void ChooseMatchClick(object sender, RoutedEventArgs e)
        {
            AddMatchForm AddMatchWindow = new AddMatchForm(ChooseMatch.SelectedIndex);
            AddMatchWindow.Show();
            Close();
        }
    }
}
