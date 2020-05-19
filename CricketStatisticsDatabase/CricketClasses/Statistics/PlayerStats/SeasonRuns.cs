using Cricket.Player;

namespace Cricket.Statistics
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

        public string ToCSVLine()
        {
            return Name.ToString() + "," + Runs + "," + Year + "," + Average;
        }

    }
}
