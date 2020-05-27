using System;
using Cricket.Match;
using Cricket.Player;

namespace Cricket.Statistics.PlayerStats
{
    public class CarryingOfBat
    {
        public PlayerName Name
        {
            get;
            set;
        }

        public string Opposition
        {
            get;
            set;
        }

        public DateTime Date
        {
            get;
            set;
        }

        public Location HomeOrAway
        {
            get;
            set;
        }

        public int Runs
        {
            get;
            set;
        }

        public InningsScore TeamTotalScore
        {
            get;
            set;
        }

        public CarryingOfBat()
        {
        }
    }
}
