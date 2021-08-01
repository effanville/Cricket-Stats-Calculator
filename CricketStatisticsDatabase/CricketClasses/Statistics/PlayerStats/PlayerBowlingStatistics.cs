using System;
using System.Linq;
using Cricket.Interfaces;
using Cricket.Match;
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

        public PlayerBowlingStatistics(PlayerName name, MatchType[] matchTypes)
        {
            Name = name;
        }

        public PlayerBowlingStatistics(PlayerName name, ICricketSeason season, MatchType[] matchTypes)
        {
            Name = name;
            SetSeasonStats(season, matchTypes);
        }

        public PlayerBowlingStatistics(PlayerName name, ICricketTeam team, MatchType[] matchTypes)
        {
            Name = name;
            SetTeamStats(team, matchTypes);
        }

        public void SetSeasonStats(ICricketSeason season, MatchType[] matchTypes, bool reset = false)
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
                if (matchTypes.Contains(match.MatchData.Type))
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
            }

            if (TotalWickets != 0)
            {
                Average = Math.Round(TotalRunsConceded / (double)TotalWickets, 2);
                StrikeRate = Math.Round(6 * (double)TotalOvers / TotalWickets, 2);
            }
            else
            {
                Average = double.NaN;
                StrikeRate = double.NaN;
            }

            if (TotalOvers != 0)
            {
                Economy = Math.Round(TotalRunsConceded / (double)TotalOvers, 2);
            }
            else
            {
                Economy = double.NaN;
            }
        }

        public void SetTeamStats(ICricketTeam team, MatchType[] matchTypes)
        {
            TotalOvers = 0;
            TotalMaidens = 0;
            TotalRunsConceded = 0;
            TotalWickets = 0;
            BestFigures = new BestBowling();
            foreach (ICricketSeason season in team.Seasons)
            {
                SetSeasonStats(season, matchTypes);
            }

            if (TotalWickets != 0)
            {
                Average = Math.Round(TotalRunsConceded / (double)TotalWickets, 2);
                StrikeRate = Math.Round(6 * (double)TotalOvers / TotalWickets, 2);
            }
            else
            {
                Average = double.NaN;
                StrikeRate = double.NaN;
            }

            if (TotalOvers != 0)
            {
                Economy = Math.Round(TotalRunsConceded / (double)TotalOvers, 2);
            }
            else
            {
                Economy = double.NaN;
            }
        }
    }
}
