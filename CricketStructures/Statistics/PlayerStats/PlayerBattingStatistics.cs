using System.Linq;
using CricketStructures.Interfaces;
using CricketStructures.Match;
using CricketStructures.Match.Innings;
using CricketStructures.Player;

namespace CricketStructures.Statistics
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

        public PlayerBattingStatistics(string teamName, PlayerName name, ICricketSeason season, MatchType[] matchTypes)
        {
            Name = name;
            SetSeasonStats(teamName, season, matchTypes);
        }

        public PlayerBattingStatistics(PlayerName name, ICricketTeam team, MatchType[] matchTypes)
        {
            Name = name;
            SetTeamStats(team, matchTypes);
        }

        public void SetSeasonStats(string teamName, ICricketSeason season, MatchType[] matchTypes, bool reset = false)
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
                    BattingEntry batting = match.GetBatting(teamName, Name);
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
                                Opposition = match.MatchData.OppositionName(),
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
                Average = TotalRuns / (TotalInnings - (double)TotalNotOut);
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
                SetSeasonStats(team.TeamName, season, matchTypes, reset: false);
            }

            if (TotalInnings != TotalNotOut)
            {
                Average = TotalRuns / (TotalInnings - (double)TotalNotOut);
            }
        }
    }
}
