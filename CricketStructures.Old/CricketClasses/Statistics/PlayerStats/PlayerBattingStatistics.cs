using System;
using System.Linq;
using Cricket.Interfaces;
using Cricket.Match;
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

        public PlayerBattingStatistics(PlayerName name, MatchType[] matchTypes)
        {
            Name = name;
        }

        public PlayerBattingStatistics(PlayerName name, ICricketSeason season, MatchType[] matchTypes)
        {
            Name = name;
            SetSeasonStats(season, matchTypes);
        }

        public PlayerBattingStatistics(PlayerName name, ICricketTeam team, MatchType[] matchTypes)
        {
            Name = name;
            SetTeamStats(team, matchTypes);
        }

        public void SetSeasonStats(ICricketSeason season, MatchType[] matchTypes, bool reset = false)
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
                if (matchTypes.Contains(match.MatchData.Type))
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
            }

            if (TotalInnings != TotalNotOut)
            {
                Average = Math.Round(TotalRuns / (TotalInnings - (double)TotalNotOut), 2);
            }
        }

        public void SetTeamStats(ICricketTeam team, MatchType[] matchTypes)
        {
            TotalInnings = 0;
            TotalNotOut = 0;
            TotalRuns = 0;
            Best = new BestBatting();

            foreach (ICricketSeason season in team.Seasons)
            {
                SetSeasonStats(season, matchTypes, reset: false);
            }

            if (TotalInnings != TotalNotOut)
            {
                Average = Math.Round(TotalRuns / (TotalInnings - (double)TotalNotOut), 2);
            }
        }
    }
}
