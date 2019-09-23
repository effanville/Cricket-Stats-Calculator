using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using Cricket;
using ReportingStructures;
using CricketDatabaseEditing;

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
            // Configure structure for holding error reports
            ErrorReports.Configure();
            
            MainWindowViewModel Data = new MainWindowViewModel(CricketDatabaseEditingFunctions.GetMatchesOppoDate(), CricketDatabaseEditingFunctions.GetPlayers);

            DataContext = Data;
            InitializeComponent();
        }

        void PlayerClick(object sender, RoutedEventArgs e)
        {
            AddPlayerForm AddPlWindow = new AddPlayerForm();
            AddPlWindow.Show();
        }

        void MatchClick(object sender, RoutedEventArgs e)
        {
            AddMatchForm AddMatchWindow = new AddMatchForm(-1);
            AddMatchWindow.Show();
        }

        private void viewedit_match_Click(object sender, RoutedEventArgs e)
        {
            MatchSelectorForm EditMatchWindow = new MatchSelectorForm();
            EditMatchWindow.Show();
        }

        void StatsClick(object sender, RoutedEventArgs e)
        {
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
        }
    }
}
