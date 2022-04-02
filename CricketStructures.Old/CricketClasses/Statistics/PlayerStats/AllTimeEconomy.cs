using Cricket.Player;

namespace Cricket.Statistics.PlayerStats
{
    public class AllTimeEconomy
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

        public double Economy
        {
            get;
            set;
        }

        public AllTimeEconomy()
        {
        }

        public AllTimeEconomy(PlayerName name, int wickets, double average)
        {
            Name = name;
            Wickets = wickets;
            Economy = average;
        }
    }
}
