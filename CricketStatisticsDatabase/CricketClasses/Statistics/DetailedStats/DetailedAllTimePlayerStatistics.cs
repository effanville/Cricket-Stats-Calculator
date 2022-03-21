using System.IO;
using Cricket.Interfaces;
using Common.Structure.FileAccess;
using Common.Structure.ReportWriting;

namespace Cricket.Statistics.DetailedStats
{
    public class DetailedAllTimePlayerStatistics
    {
        public DetailedAllTimeBattingStatistics BattingStats
        {
            get;
            set;
        } = new DetailedAllTimeBattingStatistics();

        public DetailedAllTimeBowlingStatistics BowlingStats
        {
            get;
            set;
        } = new DetailedAllTimeBowlingStatistics();

        public DetailedAllTimeFieldingStatistics FieldingStats
        {
            get;
            set;
        } = new DetailedAllTimeFieldingStatistics();

        public DetailedAllTimeCareerStatistics CareerStats
        {
            get;
            set;
        } = new DetailedAllTimeCareerStatistics();

        public void CalculateStats(ICricketTeam team)
        {
            foreach (ICricketSeason season in team.Seasons)
            {
                CalculateStats(season);
            }

            CareerStats.CalculateStats(team);
        }

        public void CalculateStats(ICricketSeason season)

        {
            BattingStats.CalculateStats(season);
            BowlingStats.CalculateStats(season);
            FieldingStats.CalculateStats(season);

            foreach (ICricketMatch match in season.Matches)
            {
                UpdateStats(match);
            }
        }

        public void UpdateStats(ICricketMatch match)
        {
        }

        public void ExportStats(StreamWriter writer, ExportType exportType)
        {
            FileWritingSupport.WriteTitle(writer, exportType, "Batting Performances", HtmlTag.h2);
            BattingStats.ExportStats(writer, exportType);

            FileWritingSupport.WriteTitle(writer, exportType, "Bowling Performances", HtmlTag.h2);
            BowlingStats.ExportStats(writer, exportType);

            FileWritingSupport.WriteTitle(writer, exportType, "Fielding Performances", HtmlTag.h2);
            FieldingStats.ExportStats(writer, exportType);

            FileWritingSupport.WriteTitle(writer, exportType, "Career Performances", HtmlTag.h2);
            CareerStats.ExportStats(writer, exportType);
        }
    }
}
