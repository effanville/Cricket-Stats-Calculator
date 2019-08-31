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
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class AddMatchForm : Window
    {
        public AddMatchForm()
        {
            InitializeComponent();
            ChoosePlayer1.ItemsSource = Globals.Ardeley;
            ChoosePlayer1.DisplayMemberPath = "Name";
            ChoosePlayer2.ItemsSource = Globals.Ardeley;
            ChoosePlayer2.DisplayMemberPath = "Name";
            ChoosePlayer3.ItemsSource = Globals.Ardeley;
            ChoosePlayer3.DisplayMemberPath = "Name";
            ChoosePlayer4.ItemsSource = Globals.Ardeley;
            ChoosePlayer4.DisplayMemberPath = "Name";
            ChoosePlayer5.ItemsSource = Globals.Ardeley;
            ChoosePlayer5.DisplayMemberPath = "Name";
            ChoosePlayer6.ItemsSource = Globals.Ardeley;
            ChoosePlayer6.DisplayMemberPath = "Name";
            ChoosePlayer7.ItemsSource = Globals.Ardeley;
            ChoosePlayer7.DisplayMemberPath = "Name";
            ChoosePlayer8.ItemsSource = Globals.Ardeley;
            ChoosePlayer8.DisplayMemberPath = "Name";
            ChoosePlayer9.ItemsSource = Globals.Ardeley;
            ChoosePlayer9.DisplayMemberPath = "Name";
            ChoosePlayer10.ItemsSource = Globals.Ardeley;
            ChoosePlayer10.DisplayMemberPath = "Name";
            ChoosePlayer11.ItemsSource = Globals.Ardeley;
            ChoosePlayer11.DisplayMemberPath = "Name";
            ResultBox.ItemsSource = Enum.GetValues(typeof(ResultType)).Cast<ResultType>();
            ResultBox.SelectedIndex = 3;
        }

        void GoToBattingClick(object sender, RoutedEventArgs e)
        {
            List<Cricket_Player> team = new List<Cricket_Player>();
            team.Add((Cricket_Player)ChoosePlayer1.SelectedItem);
            team.Add((Cricket_Player)ChoosePlayer2.SelectedItem);
            team.Add((Cricket_Player)ChoosePlayer3.SelectedItem);
            team.Add((Cricket_Player)ChoosePlayer4.SelectedItem);
            team.Add((Cricket_Player)ChoosePlayer5.SelectedItem);
            team.Add((Cricket_Player)ChoosePlayer6.SelectedItem);
            team.Add((Cricket_Player)ChoosePlayer7.SelectedItem);
            team.Add((Cricket_Player)ChoosePlayer8.SelectedItem);
            team.Add((Cricket_Player)ChoosePlayer9.SelectedItem);
            team.Add((Cricket_Player)ChoosePlayer10.SelectedItem);
            team.Add((Cricket_Player)ChoosePlayer11.SelectedItem);

            string OppoName = OppositionNameBox.Text;
            string date1 = DateBox.Text;
            string place = PlaceBox.Text;
            ResultType Result = (ResultType)ResultBox.SelectedValue;
           

            Cricket_Match newMatch = new Cricket_Match(OppoName, date1, place, Result, team);
            Globals.GamesPlayed.Add(newMatch);
            Go_To_Batting.Background = Brushes.Pink;
            AddBattingInnings AddBattingWindow = new AddBattingInnings();
            Close();
            AddBattingWindow.Show();
        }
    }
}
