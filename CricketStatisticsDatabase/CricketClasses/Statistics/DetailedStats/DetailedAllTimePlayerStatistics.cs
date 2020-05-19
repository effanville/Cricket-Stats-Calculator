using Cricket.Interfaces;
using System.IO;

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
            foreach (var season in team.Seasons)
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

            foreach (var match in season.Matches)
            {
                UpdateStats(match);
            }
        }

        public void UpdateStats(ICricketMatch match)
        {
        }

        public void ExportStats(StreamWriter writer)
        {
            writer.WriteLine("");
            writer.WriteLine("Batting Performances");
            BattingStats.ExportStats(writer);

            writer.WriteLine("");
            writer.WriteLine("Bowling Performances");
            BowlingStats.ExportStats(writer);

            writer.WriteLine("");
            writer.WriteLine("Fielding Performances");
            FieldingStats.ExportStats(writer);

            writer.WriteLine("");
            writer.WriteLine("Career Performances");
            CareerStats.ExportStats(writer);
        }
    }
}
