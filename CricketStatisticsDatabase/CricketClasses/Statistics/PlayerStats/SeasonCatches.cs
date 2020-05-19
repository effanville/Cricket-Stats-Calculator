using Cricket.Player;
using System;

namespace Cricket.Statistics.PlayerStats
{
    public class SeasonCatches
    {
        public int SeasonDismissals
        {
            get;
            set;
        }

        public PlayerName Name
        {
            get;
            set;
        }

        public DateTime Year
        {
            get;
            set;
        }

        public string ToCSVLine()
        {
            return SeasonDismissals + "," + Name.ToString() + "," + Year.Year;
        }
    }
}
