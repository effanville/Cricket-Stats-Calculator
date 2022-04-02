using Cricket.Player;

namespace Cricket.Statistics.PlayerStats
{
    public class SeasonWickets
    {
        public PlayerName Name
        {
            get;
            set;
        }

        public int Wickets
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

        public SeasonWickets()
        {
        }

        public SeasonWickets(PlayerName name, int wickets, int year, double average)
        {
            Name = name;
            Wickets = wickets;
            Year = year;
            Average = average;
        }
    }
}
