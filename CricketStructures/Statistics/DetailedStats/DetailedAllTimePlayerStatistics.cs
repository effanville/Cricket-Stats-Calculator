using System;
using System.IO;
using CricketStructures.Interfaces;
using StructureCommon.FileAccess;

namespace CricketStructures.Statistics.DetailedStats
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
                CalculateStats(team.TeamName, season);
            }

            CareerStats.CalculateStats(team);
        }

        public void CalculateStats(string teamName, ICricketSeason season)

        {
            BattingStats.CalculateStats(teamName, season);
            BowlingStats.CalculateStats(teamName, season);
            FieldingStats.CalculateStats(teamName, season);

            foreach (ICricketMatch match in season.Matches)
            {
                UpdateStats(match);
            }
        }

        public void UpdateStats(ICricketMatch match)
        {
            throw new NotImplementedException($"{nameof(DetailedAllTimePlayerStatistics)} stats routine has not been implemented for a match.");
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
