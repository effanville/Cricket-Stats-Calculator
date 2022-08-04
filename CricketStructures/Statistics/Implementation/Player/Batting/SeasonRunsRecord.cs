using System;
using System.Collections.Generic;
using System.Linq;

using Common.Structure.ReportWriting;

using CricketStructures.Match;
using CricketStructures.Player;
using CricketStructures.Season;
using CricketStructures.Statistics.Implementation.Collection;

namespace CricketStructures.Statistics.Implementation.Player.Batting
{
    internal sealed class SeasonRunsRecord : ICricketStat
    {
        private readonly int MinimumNumber;
        private readonly PlayerName Name;
        public List<SeasonRuns> SeasonRuns
        {
            get;
            set;
        } = new List<SeasonRuns>();

        private SeasonRunsRecord()
        {
        }
        public SeasonRunsRecord(int minimumNumber)
            : this()
        {
            MinimumNumber = minimumNumber;
        }
        public SeasonRunsRecord(int minimumNumber, PlayerName name)
            : this(minimumNumber)
        {
            Name = name;
        }

        public void CalculateStats(ICricketTeam team, MatchType[] matchTypes)
        {
            CricketStatsHelpers.SeasonIterator(
                team.Seasons,
                season => CalculateStats(team.TeamName, season, matchTypes),
                preCycleAction: ResetStats);
        }

        public void CalculateStats(string teamName, ICricketSeason season, MatchType[] matchTypes)
        {
            var playerNames = Name == null ? season.Players(teamName, matchTypes) : new List<PlayerName>() { Name };
            List<PlayerBriefStatistics> playerStats = playerNames.Select(name => new PlayerBriefStatistics(teamName, name, season, matchTypes)).ToList();

            IEnumerable<PlayerBriefStatistics> manyRuns = playerStats.Where(player => player.BattingStats.TotalRuns > MinimumNumber);
            SeasonRuns.AddRange(manyRuns.Select(element => new SeasonRuns(element.SeasonYear, element.Name, element.BattingStats.TotalInnings, element.BattingStats.TotalNotOut, element.BattingStats.TotalRuns, element.BattingStats.Average)));

            if (MinimumNumber == 0)
            {
                SeasonRuns.Sort((a, b) => a.Year.CompareTo(b.Year));
            }
            else
            {
                SeasonRuns.Sort((a, b) => b.Runs.CompareTo(a.Runs));
            }
        }

        public void ResetStats()
        {
            SeasonRuns.Clear();
        }

        public void UpdateStats(string teamName, ICricketMatch match)
        {
            throw new NotImplementedException();
        }

        public void ExportStats(ReportBuilder rb, DocumentElement headerElement)
        {
            if (SeasonRuns.Any())
            {
                string title = MinimumNumber == 0 ? "Yearly Batting Record" : $"Over {MinimumNumber} runs in a season";
                string[] headers = new string[] { "Year", "Innings", "Not Out", "Runs", "Average" };
                var values = SeasonRuns
                    .Select(value =>
                        new string[]
                        {
                            value.Year.Year.ToString(),
                            value.Innings.ToString(),
                            value.NotOut.ToString(),
                            value.Runs.ToString(),
                            value.Average.ToString()
                        });
                _ = rb.WriteTitle(title, headerElement)
                    .WriteTableFromEnumerable(headers, values, headerFirstColumn: false);
            }
        }
    }
}
