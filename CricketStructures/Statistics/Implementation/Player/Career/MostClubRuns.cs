using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.Structure.ReportWriting;

using CricketStructures.Match;
using CricketStructures.Match.Innings;
using CricketStructures.Season;
using CricketStructures.Statistics.Implementation.Player.Model;

namespace CricketStructures.Statistics.Implementation.Player.Career
{
    internal class MostClubRuns : ICricketStat
    {
        public List<NameDurationRecord<int>> ClubRuns
        {
            get;
            set;
        } = new List<NameDurationRecord<int>>();

        public MostClubRuns()
        {
        }

        public void CalculateStats(ICricketTeam team, MatchType[] matchTypes)
        {
            CricketStatsHelpers.SeasonIterator(
                team.Seasons,
                season => CalculateStats(team.TeamName, season, matchTypes));
            ClubRuns.Sort((a, b) => b.Value.CompareTo(a.Value));
        }

        public void CalculateStats(string teamName, ICricketSeason season, MatchType[] matchTypes)
        {
            CricketStatsHelpers.MatchIterator(
                season,
                matchTypes,
                match => UpdateStats(teamName, match));
        }

        public void UpdateStats(string teamName, ICricketMatch match)
        {
            CricketStatsHelpers.BattingIterator(
                match,
                teamName,
                entry => UpdateRuns(entry));
            void UpdateRuns(BattingEntry batting)
            {
                var playerRuns = ClubRuns.FirstOrDefault(run => run.Name.Equals(batting.Name));
                if (playerRuns != null)
                {
                    playerRuns.UpdateValue(match.MatchData.Date, batting.RunsScored);
                }
                else
                {
                    ClubRuns.Add(new NameDurationRecord<int>("MostClubRuns", batting.Name, match.MatchData.Date, batting.RunsScored, (value, otherValue) => value + otherValue));
                }
            }
        }

        public StringBuilder ExportStats(DocumentType exportType, DocumentElement headerElement)
        {
            var writer = new StringBuilder();
            TextWriting.WriteTitle(writer, exportType, "Most Club Runs", headerElement);
            var export = ClubRuns.Take(5);
            TableWriting.WriteTableFromEnumerable(writer, exportType, new string[] { "Name", "StartDate", "EndDate", "Runs Scored" }, export.Select(value => new string[] { value.Name.ToString(), value.Start.ToShortDateString(), value.End.ToShortDateString(), value.Value.ToString() }), headerFirstColumn: false);
            return writer;
        }

        public void ResetStats()
        {
            ClubRuns.Clear();
        }
    }
}
