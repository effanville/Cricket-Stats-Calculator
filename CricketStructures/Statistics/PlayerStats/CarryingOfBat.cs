using System;
using CricketStructures.Match.Innings;
using CricketStructures.Player;

namespace CricketStructures.Statistics.PlayerStats
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

        public string Location
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
