using Cricket.Interfaces;
using Cricket.Match;
using System;

namespace Cricket.Statistics.DetailedStats
{
    public class TeamYearRecord : TeamRecord
    {
        public DateTime Year
        {
            get;
            set;
        }

        public TeamYearRecord(ICricketSeason season)
            : base(season)
        {
            Year = season.Year;
        }

        public new string ToCSVLine()
        {
            return Year.Year.ToString() + "," + base.ToCSVLine();
        }
    }
}
