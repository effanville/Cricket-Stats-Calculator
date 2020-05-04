using Cricket.Player;
using Cricket.Interfaces;

namespace Cricket.Statistics
{
    public class PlayerBowlingStatistics
    {
        public static string CsvHeader()
        {
            return nameof(Name) + "," + nameof(TotalOvers) + "," + nameof(TotalMaidens) + "," + nameof(TotalRunsConceded) + "," + nameof(TotalWickets) + "," + nameof(Average) + "," + nameof(Economy) + "," + "Best figures";
        }

        public override string ToString()
        {
            return Name.ToString() + "," + TotalOvers + "," + TotalMaidens + "," + TotalRunsConceded + "," + TotalWickets + "," + Average + "," + Economy + "," + BestFigures.ToString();
        }

        public PlayerName Name
        {
            get;
            set;
        }

        private double fTotalOvers;
        public double TotalOvers
        {
            get { return fTotalOvers; }
            set { fTotalOvers = value; }
        }

        private int fTotalMaidens;
        public int TotalMaidens
        {
            get { return fTotalMaidens; }
            set { fTotalMaidens = value; }
        }

        private int fTotalRunsConceded;
        public int TotalRunsConceded
        {
            get { return fTotalRunsConceded; }
            set { fTotalRunsConceded = value; }
        }


        private int fTotalWickets;
        public int TotalWickets
        {
            get { return fTotalWickets; }
            set { fTotalWickets = value; }
        }

        private double fAverage;
        public double Average
        {
            get { return fAverage; }
            set
            {
                fAverage = value;
            }
        }

        private double fEconomy;
        public double Economy
        {
            get { return fEconomy; }
            set { fEconomy = value; }
        }

        private BestBowling fBestFigures;
        public BestBowling BestFigures
        {
            get { return fBestFigures; }
            set { fBestFigures = value; }
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

        public void SetSeasonStats(ICricketSeason season)
        {
            TotalOvers = 0;
            TotalMaidens = 0;
            TotalRunsConceded = 0;
            TotalWickets = 0;
            BestFigures = new BestBowling();

            foreach (var match in season.Matches)
            {
                var bowling = match.GetBowling(Name);
                if (bowling != null)
                {
                    TotalOvers += bowling.OversBowled;
                    TotalMaidens += bowling.Maidens;
                    TotalRunsConceded += bowling.RunsConceded;
                    TotalWickets += bowling.Wickets;

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
            }

            if (TotalWickets != 0)
            {
                Average = (double)TotalRunsConceded / (double)TotalWickets;
            }
            else
            {
                Average = double.NaN;
            }

            if (TotalOvers != 0)
            {
                Economy = (double)TotalRunsConceded / (double)TotalOvers;
            }
            else
            {
                Economy = double.NaN;
            }
        }
    }
}
