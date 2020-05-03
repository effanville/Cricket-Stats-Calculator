using Cricket.Player;
using Cricket.Interfaces;
using System.Collections.Generic;
using System;

namespace Cricket.Statistics
{
    public class PlayerBattingStatistics
    {
        public static string CsvHeader()
        {
            return nameof(Name) + "," + nameof(TotalInnings) + "," + nameof(TotalNotOut) + "," + nameof(TotalRuns) + "," + nameof(Average) + "," + "Bestbatting";
        }

        public override string ToString()
        {
            return Name.ToString() + "," + TotalInnings + "," + TotalNotOut + "," + TotalRuns + "," + Average + "," + Best.ToString();
        }

        public PlayerName Name
        {
            get;
            set;
        }

        private int fTotalInnings;
        public int TotalInnings
        {
            get
            {
                return fTotalInnings;
            }
            set
            {
                fTotalInnings = value;
            }
        }

        private int fTotalNotOut;
        public int TotalNotOut
        {
            get
            {
                return fTotalNotOut;
            }
            set
            {
                fTotalNotOut = value;
            }
        }

        private int fTotalRuns;
        public int TotalRuns
        {
            get
            {
                return fTotalRuns;
            }
            set
            {
                fTotalRuns = value;
            }
        }

        private double fAverage;
        public double Average
        {
            get
            {
                return fAverage;
            }
            set
            {
                fAverage = value;
            }
        }

        private List<int> fWicketLossNumbers = new List<int>(new int[Enum.GetValues(typeof(Match.Wicket)).Length]);
        public List<int> WicketLossNumbers
        {
            get { return fWicketLossNumbers; }
            set { fWicketLossNumbers = value; }
        }

        private BestBatting fBest;
        public BestBatting Best
        {
            get { return fBest; }
            set { fBest = value; }
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

        public void SetSeasonStats(ICricketSeason season)
        {
            TotalInnings = 0;
            TotalNotOut = 0;
            TotalRuns = 0;
            Best = new BestBatting();

            foreach (var match in season.Matches)
            {
                var batting = match.GetBatting(Name);
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
                        WicketLossNumbers[index] += 1;
                        TotalRuns += batting.RunsScored;

                        var possibleBest = new BestBatting()
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
                Average = (double)TotalRuns / ((double)TotalInnings - (double)TotalNotOut);
            }
        }
    }
}
