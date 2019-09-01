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
            List<string> team = new List<string>(new string[11]);
            if (ChoosePlayer1.SelectedItem is Cricket_Player)
            { team[0]= ((Cricket_Player)ChoosePlayer1.SelectedItem).Name; }

            if (ChoosePlayer2.SelectedItem is Cricket_Player )
            { team[1]=((Cricket_Player)ChoosePlayer2.SelectedItem).Name; }

            if (ChoosePlayer3.SelectedItem is Cricket_Player)
            { team[2]=((Cricket_Player)ChoosePlayer3.SelectedItem).Name; }

            if (ChoosePlayer4.SelectedItem is Cricket_Player)
            { team[3]=((Cricket_Player)ChoosePlayer4.SelectedItem).Name; }

            if (ChoosePlayer5.SelectedItem is Cricket_Player)
            { team[4]= ((Cricket_Player)ChoosePlayer5.SelectedItem).Name; }

            if (ChoosePlayer6.SelectedItem is Cricket_Player)
            {   team[5] = ((Cricket_Player)ChoosePlayer6.SelectedItem).Name; }
            if (ChoosePlayer7.SelectedItem is Cricket_Player )
            {   team[6] = ((Cricket_Player)ChoosePlayer7.SelectedItem).Name; }
            if (ChoosePlayer8.SelectedItem is Cricket_Player)
            {   team[7] = ((Cricket_Player)ChoosePlayer8.SelectedItem).Name; }
            if (ChoosePlayer9.SelectedItem is Cricket_Player )
            { team[8] = ((Cricket_Player)ChoosePlayer9.SelectedItem).Name; }
            if (ChoosePlayer10.SelectedItem is Cricket_Player )
            {   team[9] = ((Cricket_Player)ChoosePlayer10.SelectedItem).Name; }
            if (ChoosePlayer11.SelectedItem is Cricket_Player)
            { team[10] = ((Cricket_Player)ChoosePlayer11.SelectedItem).Name; }

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
