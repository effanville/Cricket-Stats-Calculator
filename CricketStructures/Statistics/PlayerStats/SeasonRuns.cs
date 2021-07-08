using CricketStructures.Player;

namespace CricketStructures.Statistics.PlayerStats
{
    public class SeasonRuns
    {
        public PlayerName Name
        {
            get;
            set;
        }

        public int Runs
        {
            get;
            set;
        }

        public int Year
        {
            get;
            set;
        }

        public double Average
        {
            get;
            set;
        }

        public SeasonRuns()
        {
        }
    }
}
