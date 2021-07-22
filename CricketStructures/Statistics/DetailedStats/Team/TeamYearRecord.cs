using CricketStructures.Season;

namespace CricketStructures.Statistics.DetailedStats
{
    public class TeamYearRecord : TeamRecord
    {
        public int Year
        {
            get;
            set;
        }

        public TeamYearRecord(ICricketSeason season)
            : base(season)
        {
            Year = season.Year.Year;
        }

        public new string ToCSVLine()
        {
            return Year.ToString() + "," + base.ToCSVLine();
        }
    }
}
