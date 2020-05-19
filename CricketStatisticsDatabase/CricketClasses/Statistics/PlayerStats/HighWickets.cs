using Cricket.Player;
using System.Runtime.InteropServices;

namespace Cricket.Statistics.PlayerStats
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

        public string ToCSVLine()
        {
            return Name.ToString() + "," + NumberFiveFor;
        }
    }
}
