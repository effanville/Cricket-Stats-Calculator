using CricketStructures.Player;

namespace CricketStructures.Statistics.PlayerStats
{
    public class HighWickets
    {
        public PlayerName Name
        {
            get;
            set;
        }

        public int NumberFiveFor
        {
            get;
            set;
        }

        public HighWickets(PlayerName name, int number)
        {
            Name = name;
            NumberFiveFor = number;
        }
    }
}
