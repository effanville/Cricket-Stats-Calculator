using Cricket.Interfaces;
using Cricket.Player;
using System;

namespace Cricket.Statistics.PlayerStats
{
    public class CareerBowlingRecord
    {
        public PlayerName Name
        {
            get;
            set;
        }

        public DateTime StartYear
        {
            get;
            set;
        }

        public DateTime EndYear
        {
            get;
            set;
        }

        public double Overs
        {
            get;
            set;
        }

        public int Maidens
        {
            get;
            set;
        }

        public int RunsConceded
        {
            get;
            set;
        }

        public int Wickets
        {
            get;
            set;
        }

        public double Average
        {
            get;
            set;
        }

        public BestBowling BestFigures
        {
            get;
            set;
        }

        public int Catches
        {
            get;
            set;
        }

        public int KeeperDismissals
        {
            get;
            set;
        }

        public CareerBowlingRecord()
        {
        }

        public CareerBowlingRecord(PlayerName name, ICricketTeam team)
        {
            Name = name;
            Overs = 0;
            Maidens = 0;
            RunsConceded = 0;
            Wickets = 0;
            BestFigures = new BestBowling();
            StartYear = DateTime.Today;
            EndYear = new DateTime();

            Catches = 0;
            KeeperDismissals = 0;
            foreach (var season in team.Seasons)
            {
                foreach (var match in season.Matches)
                {
                    if (match.PlayNotPlay(Name))
                    {
                        if (match.MatchData.Date.Year < StartYear.Year)
                        {
                            StartYear = match.MatchData.Date;
                        }
                        if (match.MatchData.Date.Year > EndYear.Year)
                        {
                            EndYear = match.MatchData.Date;
                        }

                        var bowling = match.GetBowling(Name);
                        if (bowling != null)
                        {
                            Overs += bowling.OversBowled;
                            Maidens += bowling.Maidens;
                            RunsConceded += bowling.RunsConceded;
                            Wickets += bowling.Wickets;

                            var possibleBest = new BestBowling()
                            {
                                Wickets = bowling.Wickets,
                                Runs = bowling.RunsConceded,
                                Opposition = match.MatchData.Opposition,
                                Date = match.MatchData.Date
                            };

                            if (possibleBest.CompareTo(BestFigures) > 0)
                            {
                                BestFigures = possibleBest;
                            }
                        }

                        var fielding = match.GetFielding(Name);
                        if (fielding != null)
                        {
                            Catches += fielding.Catches;
                            KeeperDismissals += fielding.KeeperStumpings + fielding.KeeperCatches;
                        }
                    }
                }
            }

            if (Wickets != 0)
            {
                Average = (double)RunsConceded / (double)Wickets;
            }
            else
            {
                Average = double.NaN;
            }
        }


        public string ToCSVLine()
        {
            return Name.ToString() + "," + Overs + "," + Maidens + "," + RunsConceded + "," + Wickets + "," + Average + "," + BestFigures.ToString() + "," + Catches + "," + KeeperDismissals;
        }
    }
}
