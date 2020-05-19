using Cricket.Match;
using Cricket.Player;
using StructureCommon.Extensions;
using System;
using System.Security.RightsManagement;
using System.Windows.Controls;

namespace CricketStatisticsDatabase.CricketClasses.Statistics.PlayerStats
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

        public string ToCSVLine()
        {
            return Name.ToString() + "," + Opposition + "," + Date.ToUkDateString() + "," + HomeOrAway + "," + Runs + "," + TeamTotalScore.ToString();
        }
    }
}
