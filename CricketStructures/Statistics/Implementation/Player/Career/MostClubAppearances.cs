using System.Collections.Generic;
using System.Linq;

using Common.Structure.ReportWriting;

using CricketStructures.Match;
using CricketStructures.Season;
using CricketStructures.Statistics.Implementation.Player.Model;

namespace CricketStructures.Statistics.Implementation.Player.Career
{
    internal sealed class MostClubAppearances : ICricketStat
    {
        public List<NameDurationRecord<int>> ClubAppearances
        {
            get;
            set;
        } = new List<NameDurationRecord<int>>();

        public void CalculateStats(ICricketTeam team, MatchType[] matchTypes)
        {
            CricketStatsHelpers.SeasonIterator(
                team.Seasons,
                season => CalculateStats(team.TeamName, season, matchTypes),
                null,
                () => ClubAppearances.Sort((a, b) => b.Value.CompareTo(a.Value)));
        }

        /// <inheritdoc/>
        public void CalculateStats(string teamName, ICricketSeason season, MatchType[] matchTypes)
        {
            CricketStatsHelpers.MatchIterator(
                season,
                matchTypes,
                match => UpdateStats(teamName, match));
        }

        public void UpdateStats(string teamName, ICricketMatch match)
        {
            foreach (var playerName in match.Players(teamName))
            {
                var playerApps = ClubAppearances.FirstOrDefault(run => run.Name.Equals(playerName));
                if (playerApps != null)
                {
                    playerApps.UpdateValue(match.MatchData.Date, 1);
                }
                else
                {
                    ClubAppearances.Add(new NameDurationRecord<int>("MostAppearances", playerName, match.MatchData.Date, 1, (value, otherValue) => value + otherValue));
                }
            }
        }

        public void ResetStats()
        {
            ClubAppearances.Clear();
        }

        public void ExportStats(ReportBuilder rb, DocumentElement headerElement)
        {
            var export = ClubAppearances.Take(5);
            _ = rb.WriteTitle("Appearances", headerElement)
                .WriteTableFromEnumerable(new string[] { "Name", "StartDate", "EndDate", "Appearances" }, export.Select(value => new string[] { value.Name.ToString(), value.Start.ToShortDateString(), value.End.ToShortDateString(), value.Value.ToString() }), headerFirstColumn: false);
        }
    }
}
