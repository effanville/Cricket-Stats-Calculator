﻿using Cricket.Interfaces;
using Cricket.Player;

namespace Cricket.Statistics
{
    public class PlayerBowlingStatistics
    {
        public PlayerName Name
        {
            get;
            set;
        }

        public double TotalOvers
        {
            get;
            set;
        }

        public int TotalMaidens
        {
            get;
            set;
        }

        public int TotalRunsConceded
        {
            get;
            set;
        }

        public int TotalWickets
        {
            get;
            set;
        }

        public double Average
        {
            get;
            set;
        }

        public double Economy
        {
            get;
            set;
        }

        public double StrikeRate
        {
            get;
            set;
        }

        public BestBowling BestFigures
        {
            get;
            set;
        }

        public PlayerBowlingStatistics()
        {
        }

        public PlayerBowlingStatistics(PlayerName name)
        {
            Name = name;
        }

        public PlayerBowlingStatistics(PlayerName name, ICricketSeason season)
        {
            Name = name;
            SetSeasonStats(season);
        }

        public PlayerBowlingStatistics(PlayerName name, ICricketTeam team)
        {
            Name = name;
            SetTeamStats(team);
        }

        public void SetSeasonStats(ICricketSeason season, bool reset = false)
        {
            if (reset)
            {
                TotalOvers = 0;
                TotalMaidens = 0;
                TotalRunsConceded = 0;
                TotalWickets = 0;
                BestFigures = new BestBowling();
            }

            foreach (ICricketMatch match in season.Matches)
            {
                Match.BowlingEntry bowling = match.GetBowling(Name);
                if (bowling != null)
                {
                    TotalOvers += bowling.OversBowled;
                    TotalMaidens += bowling.Maidens;
                    TotalRunsConceded += bowling.RunsConceded;
                    TotalWickets += bowling.Wickets;

                    BestBowling possibleBest = new BestBowling()
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
            }

            if (TotalWickets != 0)
            {
                Average = TotalRunsConceded / (double)TotalWickets;
                StrikeRate = 6 * (double)TotalOvers / TotalWickets;
            }
            else
            {
                Average = double.NaN;
                StrikeRate = double.NaN;
            }

            if (TotalOvers != 0)
            {
                Economy = TotalRunsConceded / (double)TotalOvers;
            }
            else
            {
                Economy = double.NaN;
            }
        }

        public void SetTeamStats(ICricketTeam team)
        {
            TotalOvers = 0;
            TotalMaidens = 0;
            TotalRunsConceded = 0;
            TotalWickets = 0;
            BestFigures = new BestBowling();
            foreach (ICricketSeason season in team.Seasons)
            {
                SetSeasonStats(season);
            }

            if (TotalWickets != 0)
            {
                Average = TotalRunsConceded / (double)TotalWickets;
                StrikeRate = 6 * (double)TotalOvers / TotalWickets;
            }
            else
            {
                Average = double.NaN;
                StrikeRate = double.NaN;
            }

            if (TotalOvers != 0)
            {
                Economy = TotalRunsConceded / (double)TotalOvers;
            }
            else
            {
                Economy = double.NaN;
            }
        }
    }
}
