using System;
using System.Collections.Generic;
using Cricket.Interfaces;
using Cricket.Player;

namespace Cricket.Statistics
{
    public class PlayerBattingStatistics
    {
        public PlayerName Name
        {
            get;
            set;
        }

        public int TotalInnings
        {
            get;
            set;
        }

        public int TotalNotOut
        {
            get;
            set;
        }

        public int TotalRuns
        {
            get;
            set;
        }

        public double Average
        {
            get;
            set;
        }

        //public List<int> WicketLossNumbers
        //{
        //    get;
        //    set;
        //} = new List<int>(new int[Enum.GetValues(typeof(Match.Wicket)).Length]);

        public BestBatting Best
        {
            get;
            set;
        }

        public PlayerBattingStatistics()
        {
        }

        public PlayerBattingStatistics(PlayerName name)
        {
            Name = name;
        }

        public PlayerBattingStatistics(PlayerName name, ICricketSeason season)
        {
            Name = name;
            SetSeasonStats(season);
        }

        public PlayerBattingStatistics(PlayerName name, ICricketTeam team)
        {
            Name = name;
            SetTeamStats(team);
        }

        public void SetSeasonStats(ICricketSeason season, bool reset = false)
        {
            if (reset)
            {
                TotalInnings = 0;
                TotalNotOut = 0;
                TotalRuns = 0;
                Best = new BestBatting();
            }

            foreach (ICricketMatch match in season.Matches)
            {
                Match.BattingEntry batting = match.GetBatting(Name);
                if (batting != null)
                {
                    if (batting.MethodOut != Match.Wicket.DidNotBat)
                    {
                        TotalInnings++;
                        if (!batting.Out())
                        {
                            TotalNotOut++;
                        }
                        int index = (int)batting.MethodOut;
                        //WicketLossNumbers[index] += 1;
                        TotalRuns += batting.RunsScored;

                        BestBatting possibleBest = new BestBatting()
                        {
                            Runs = batting.RunsScored,
                            HowOut = batting.MethodOut,
                            Opposition = match.MatchData.Opposition,
                            Date = match.MatchData.Date
                        };

                        if (possibleBest.CompareTo(Best) > 0)
                        {
                            Best = possibleBest;
                        }
                    }
                }
            }

            if (TotalInnings != TotalNotOut)
            {
                Average = TotalRuns / (TotalInnings - (double)TotalNotOut);
            }
        }

        public void SetTeamStats(ICricketTeam team)
        {
            TotalInnings = 0;
            TotalNotOut = 0;
            TotalRuns = 0;
            Best = new BestBatting();

            foreach (ICricketSeason season in team.Seasons)
            {
                SetSeasonStats(season, reset: false);
            }

            if (TotalInnings != TotalNotOut)
            {
                Average = TotalRuns / (TotalInnings - (double)TotalNotOut);
            }
        }
    }
}
