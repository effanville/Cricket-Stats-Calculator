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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Cricket;

namespace CricketStatsCalc
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Globals.LoadDatabase();
            InitializeComponent();
            foreach (Cricket_Player person in Globals.Ardeley)
            {
                Players.Items.Add(person.Name);
            }
            foreach (Cricket_Match opposition in Globals.GamesPlayed)
            {
                Matches.Items.Add(opposition.FOpposition);
            }
        }

        void PlayerClick(object sender, RoutedEventArgs e)
        {
            Add_Pl_btn.Background = Brushes.Pink;
            AddPlayerForm AddPlWindow = new AddPlayerForm();
            AddPlWindow.Show();
        }

        void MatchClick(object sender, RoutedEventArgs e)
        {
            AddMatchForm AddMatchWindow = new AddMatchForm();
            Add_Match_btn.Background = Brushes.Pink;
            AddMatchWindow.Show();
        }

        void StatsClick(object sender, RoutedEventArgs e)
        {
            Stats_btn.Background = Brushes.Pink;
            foreach (Cricket_Player person in Globals.Ardeley)
            {
                person.set_statistics();
            }
            StatisticsWindow AddStatsWindow = new StatisticsWindow();
            AddStatsWindow.Show();
        }

        void SaveClick(object sender, RoutedEventArgs e)
        {
            Globals.SaveDatabase();
            Save_btn.Background = Brushes.Pink;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Players.Items.Clear();

            foreach (Cricket_Player person in Globals.Ardeley)
            {
                Players.Items.Add(person.Name);
            }
        }

        private void match_button_Click(object sender, RoutedEventArgs e)
        {

            Matches.Items.Clear();

            foreach(Cricket_Match opposition in Globals.GamesPlayed)
            {
                Matches.Items.Add(opposition.FOpposition);
            }
        }

        private void viewedit_match_Click(object sender, RoutedEventArgs e)
        {
            MatchSelectorForm EditMatchWindow = new MatchSelectorForm();
            EditMatchWindow.Show();
        }
    }
}
