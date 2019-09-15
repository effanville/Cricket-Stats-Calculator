using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using Cricket;
using ReportingStructures;

namespace CricketStatsCalc
{

    public class MatchViewData
    {
        public MatchViewData(string input1, DateTime input2)
        {
            OppositionName = input1;

            MatchDate = input2;
        }

        public string OppositionName;

        public DateTime MatchDate;

    }

    class MatchDateCompare : IComparer<MatchViewData>
    {
        public int Compare(MatchViewData x, MatchViewData y)
        {
            return DateTime.Compare(x.MatchDate, y.MatchDate);
        }
    }


    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Globals.LoadDatabase();

            // Configure structure for holding error reports
            ErrorReports.Configure();
            InitializeComponent();
            foreach (Cricket_Player person in Globals.Ardeley)
            {
                Players.Items.Add(person.Name);
            }
            List<MatchViewData> outputs = new List<MatchViewData>();
            foreach (Cricket_Match opposition in Globals.GamesPlayed)
            {
                MatchViewData Temp = new MatchViewData(opposition.FOpposition, opposition.Date);
                outputs.Add(Temp);
            }

            MatchDateCompare MDC = new MatchDateCompare();
            outputs.Sort(MDC);

            foreach(MatchViewData match in outputs)
            {
                string Temp = match.OppositionName + " " + match.MatchDate.ToShortDateString();
                Matches.Items.Add(Temp);
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
            AddMatchForm AddMatchWindow = new AddMatchForm(-1);
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
            List<MatchViewData> outputs = new List<MatchViewData>();
            foreach (Cricket_Match opposition in Globals.GamesPlayed)
            {
                MatchViewData Temp = new MatchViewData(opposition.FOpposition, opposition.Date);
                outputs.Add(Temp);
            }

            MatchDateCompare MDC = new MatchDateCompare();
            outputs.Sort(MDC);

            foreach (MatchViewData match in outputs)
            {
                string Temp = match.OppositionName + " " + match.MatchDate.ToShortDateString();
                Matches.Items.Add(Temp);
            }
        }

        private void viewedit_match_Click(object sender, RoutedEventArgs e)
        {
            MatchSelectorForm EditMatchWindow = new MatchSelectorForm();
            EditMatchWindow.Show();
        }
    }
}
