using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using Cricket;
using ReportingStructures;

namespace CricketStatsCalc
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class AddMatchForm : Window
    {
        public int GameIndex = -1;

        BindingList<string> MoMList = new BindingList<string>();
        
        public AddMatchForm(int indexinput)
        {
            

        GameIndex = indexinput;

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

            MatchTypeBox.ItemsSource = Enum.GetValues(typeof(MatchType)).Cast<MatchType>();
            MatchTypeBox.SelectedIndex = 0;
            if (indexinput > -1)
            {
                int value = Globals.IndexFromPlayerName(Globals.GamesPlayed[indexinput].FPlayerNames[0]);
                if (value != -1)
                {
                    ChoosePlayer1.SelectedIndex = value;
                    
                }

                value = Globals.IndexFromPlayerName(Globals.GamesPlayed[indexinput].FPlayerNames[1]);
                if (value != -1)
                {
                    ChoosePlayer2.SelectedIndex = value;
                   
                }
                    value = Globals.IndexFromPlayerName(Globals.GamesPlayed[indexinput].FPlayerNames[2]);
                if (value != -1)
                {
                    ChoosePlayer3.SelectedIndex = value;
                    
                }
                    value = Globals.IndexFromPlayerName(Globals.GamesPlayed[indexinput].FPlayerNames[3]);
                if (value != -1)
                {
                    ChoosePlayer4.SelectedIndex = value;
                    
                }
               
                value = Globals.IndexFromPlayerName(Globals.GamesPlayed[indexinput].FPlayerNames[4]);
                if (value != -1)
                {
                    ChoosePlayer5.SelectedIndex = value;
                   
                }
                    value = Globals.IndexFromPlayerName(Globals.GamesPlayed[indexinput].FPlayerNames[5]);
                if (value != -1)
                {
                    ChoosePlayer6.SelectedIndex = value;
                    
                }
                    value = Globals.IndexFromPlayerName(Globals.GamesPlayed[indexinput].FPlayerNames[6]);
                if (value != -1)
                {
                    ChoosePlayer7.SelectedIndex = value;
                   
                }
                    value = Globals.IndexFromPlayerName(Globals.GamesPlayed[indexinput].FPlayerNames[7]);
                if (value != -1)
                {
                    ChoosePlayer8.SelectedIndex = value;
                   
                }
                    value = Globals.IndexFromPlayerName(Globals.GamesPlayed[indexinput].FPlayerNames[8]);
                if (value != -1)
                {
                    ChoosePlayer9.SelectedIndex = value;
                    
                }

                    value = Globals.IndexFromPlayerName(Globals.GamesPlayed[indexinput].FPlayerNames[9]);
                if (value != -1)
                {
                    ChoosePlayer10.SelectedIndex = value;
                    
                }
                    value = Globals.IndexFromPlayerName(Globals.GamesPlayed[indexinput].FPlayerNames[10]);
                if (value != -1)
                {
                    ChoosePlayer11.SelectedIndex = value;
                    
                }

                    ResultBox.SelectedIndex = (int)Globals.GamesPlayed[indexinput].FResult;

                Binding Asource= new Binding();
                Asource.Source = MoMList;
                
                ManOfMatchBox.ItemsSource = MoMList;

                if (Globals.GamesPlayed[indexinput].FMoM != null)
                {
                    ManOfMatchBox.SelectedIndex = indexinput;
                }

                OppositionNameBox.Text = Globals.GamesPlayed[indexinput].FOpposition;
                DateBox.Text = Globals.GamesPlayed[indexinput].Date.ToShortDateString();
                PlaceBox.Text = Globals.GamesPlayed[indexinput].FPlace;
            }
        }

        public void ChoosePlayer1_SelectionChanged(object sender, RoutedEventArgs e)
        {
            MoMList.Remove(ChoosePlayer1.Text);
            MoMList.Add(((Cricket_Player)ChoosePlayer1.SelectedItem).Name);
            ManOfMatchBox.ItemsSource = null;
            ManOfMatchBox.ItemsSource = MoMList;
        }

        public void ChoosePlayer2_SelectionChanged(object sender, RoutedEventArgs e)
        {
            MoMList.Remove(ChoosePlayer2.Text);
            MoMList.Add(((Cricket_Player)ChoosePlayer2.SelectedItem).Name);
            ManOfMatchBox.ItemsSource = null;
            ManOfMatchBox.ItemsSource = MoMList;
        }
        public void ChoosePlayer3_SelectionChanged(object sender, RoutedEventArgs e)
        {
            MoMList.Remove(ChoosePlayer3.Text);
            MoMList.Add(((Cricket_Player)ChoosePlayer3.SelectedItem).Name);
            ManOfMatchBox.ItemsSource = null;
            ManOfMatchBox.ItemsSource = MoMList;
        }
        public void ChoosePlayer4_SelectionChanged(object sender, RoutedEventArgs e)
        {
            MoMList.Remove(ChoosePlayer4.Text);
            MoMList.Add(((Cricket_Player)ChoosePlayer4.SelectedItem).Name);
            ManOfMatchBox.ItemsSource = null;
            ManOfMatchBox.ItemsSource = MoMList;
        }
        public void ChoosePlayer5_SelectionChanged(object sender, RoutedEventArgs e)
        {
            MoMList.Remove(ChoosePlayer5.Text);
            MoMList.Add(((Cricket_Player)ChoosePlayer5.SelectedItem).Name);
            ManOfMatchBox.ItemsSource = null;
            ManOfMatchBox.ItemsSource = MoMList;
        }
        public void ChoosePlayer6_SelectionChanged(object sender, RoutedEventArgs e)
        {
            MoMList.Remove(ChoosePlayer6.Text);
            MoMList.Add(((Cricket_Player)ChoosePlayer6.SelectedItem).Name);
            ManOfMatchBox.ItemsSource = null;
            ManOfMatchBox.ItemsSource = MoMList;
        }
        public void ChoosePlayer7_SelectionChanged(object sender, RoutedEventArgs e)
        {
            MoMList.Remove(ChoosePlayer7.Text);
            MoMList.Add(((Cricket_Player)ChoosePlayer7.SelectedItem).Name);
            ManOfMatchBox.ItemsSource = null;
            ManOfMatchBox.ItemsSource = MoMList;
        }
        public void ChoosePlayer8_SelectionChanged(object sender, RoutedEventArgs e)
        {
            MoMList.Remove(ChoosePlayer8.Text);
            MoMList.Add(((Cricket_Player)ChoosePlayer8.SelectedItem).Name);
            ManOfMatchBox.ItemsSource = null;
            ManOfMatchBox.ItemsSource = MoMList;
        }
        public void ChoosePlayer9_SelectionChanged(object sender, RoutedEventArgs e)
        {
            MoMList.Remove(ChoosePlayer9.Text);
            MoMList.Add(((Cricket_Player)ChoosePlayer9.SelectedItem).Name);
            ManOfMatchBox.ItemsSource = null;
            ManOfMatchBox.ItemsSource = MoMList;
        }
        public void ChoosePlayer10_SelectionChanged(object sender, RoutedEventArgs e)
        {
                MoMList.Remove(ChoosePlayer10.Text);
                MoMList.Add(((Cricket_Player)ChoosePlayer10.SelectedItem).Name);
        }
        public void ChoosePlayer11_SelectionChanged(object sender, RoutedEventArgs e)
        {
            
                MoMList.Remove(ChoosePlayer11.Text);
                MoMList.Add(((Cricket_Player)ChoosePlayer11.SelectedItem).Name);
            ManOfMatchBox.ItemsSource = null;
            ManOfMatchBox.ItemsSource = MoMList;
        }

        void GoToBattingClick(object sender, RoutedEventArgs e)
        {

                List<string> team = new List<string>(new string[11]);
                if (ChoosePlayer1.SelectedItem is Cricket_Player)
                { team[0] = ((Cricket_Player)ChoosePlayer1.SelectedItem).Name; }

                if (ChoosePlayer2.SelectedItem is Cricket_Player)
                { team[1] = ((Cricket_Player)ChoosePlayer2.SelectedItem).Name; }

                if (ChoosePlayer3.SelectedItem is Cricket_Player)
                { team[2] = ((Cricket_Player)ChoosePlayer3.SelectedItem).Name; }

                if (ChoosePlayer4.SelectedItem is Cricket_Player)
                { team[3] = ((Cricket_Player)ChoosePlayer4.SelectedItem).Name; }

                if (ChoosePlayer5.SelectedItem is Cricket_Player)
                { team[4] = ((Cricket_Player)ChoosePlayer5.SelectedItem).Name; }

                if (ChoosePlayer6.SelectedItem is Cricket_Player)
                { team[5] = ((Cricket_Player)ChoosePlayer6.SelectedItem).Name; }
                if (ChoosePlayer7.SelectedItem is Cricket_Player)
                { team[6] = ((Cricket_Player)ChoosePlayer7.SelectedItem).Name; }
                if (ChoosePlayer8.SelectedItem is Cricket_Player)
                { team[7] = ((Cricket_Player)ChoosePlayer8.SelectedItem).Name; }
                if (ChoosePlayer9.SelectedItem is Cricket_Player)
                { team[8] = ((Cricket_Player)ChoosePlayer9.SelectedItem).Name; }
                if (ChoosePlayer10.SelectedItem is Cricket_Player)
                { team[9] = ((Cricket_Player)ChoosePlayer10.SelectedItem).Name; }
                if (ChoosePlayer11.SelectedItem is Cricket_Player)
                { team[10] = ((Cricket_Player)ChoosePlayer11.SelectedItem).Name; }

                string OppoName = OppositionNameBox.Text;
            if (OppoName == "Opposition Name")
            {
                ErrorReports.AddError("Opposition Name is not set");
            }

                DateTime date1;

            if (!DateTime.TryParse(DateBox.Text, out date1))
            {
                ErrorReports.AddWarning("No Date Specified");
            }

            string MoM = "";
            if (ManOfMatchBox.SelectedValue != null)
            {
                MoM = ManOfMatchBox.SelectedValue.ToString();
            }
            else
            {
                ErrorReports.AddWarning("No Man of Match Selected");
            }

                
                string place = PlaceBox.Text;
            if (place == "Place")
            {
                ErrorReports.AddWarning("Place of game not set.");
                place = null;
            }
                ResultType Result = (ResultType)ResultBox.SelectedValue;
            MatchType TypeofMatch = (MatchType)MatchTypeBox.SelectedValue;

            //now perform consistency checks on inputted data
            //check for players added once
            bool pls = false;
            for(int i = 0; i < team.Count; i++)
            {
                if (team[i] != null)
                {
                    pls = true;
                }
                for (int j = i + 1; j < team.Count; j++)
                {
                    if (team[i] !=null && team[j] !=null)
                    {
                        if (team[i] == team[j])
                        {
                            string issue = "Same player has been added twice to match: " + team[i];
                            ErrorReports.AddError(issue);
                        }
                    }
                }
            }
            if (!pls)
            {
                ErrorReports.AddError("No players included in this match");
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
                if (ErrorReports.GetErrors().Count != 0 || ErrorReports.GetWarnings().Count!=0|| ErrorReports.GetReport().Count!=0)
                {
                    
                    ErrorsWindow.ShowDialog();
                }

                if (ErrorsWindow.BackForward)
                {
                    if (GameIndex < 0)
                    {
                        Cricket_Match newMatch = new Cricket_Match(OppoName, date1, place, Result, TypeofMatch, MoM, team);
                        Globals.GamesPlayed.Add(newMatch);
                    }

                    if (GameIndex > -1)
                    {
                        Globals.GamesPlayed[GameIndex].EditMatchdata(OppoName, date1, place, Result, MoM, team);
                    }

                    AddBattingInnings AddBattingWindow = new AddBattingInnings(GameIndex);
                    AddBattingWindow.Show();

                    Close();
                }
                else
                {
                }
            }
        }
    }
}
