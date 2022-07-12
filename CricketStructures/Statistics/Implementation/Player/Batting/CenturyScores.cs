using System.Collections.Generic;
using System.Linq;

using Common.Structure.ReportWriting;

using CricketStructures.Match;
using CricketStructures.Match.Innings;
using CricketStructures.Player;
using CricketStructures.Season;
using CricketStructures.Statistics.Implementation.Player.Model;

namespace CricketStructures.Statistics.Implementation.Player.Batting
{
    internal sealed class CenturyScores : ICricketStat
    {
        private readonly PlayerName Name;
        public List<PlayerScore> Centuries
        {
            get;
            set;
        } = new List<PlayerScore>();

        public CenturyScores()
        {
        }

        public CenturyScores(PlayerName name)
        {
            Name = name;
        }

        public void CalculateStats(ICricketTeam team, MatchType[] matchTypes)
        {
            CricketStatsHelpers.SeasonIterator(
                team.Seasons,
                season => CalculateStats(team.TeamName, season, matchTypes));
        }

        public void CalculateStats(string teamName, ICricketSeason season, MatchType[] matchTypes)
        {
            CricketStatsHelpers.MatchIterator(
                season,
                matchTypes,
                match => UpdateStats(teamName, match),
                postCycleAction: () => Centuries.Sort((a, b) => b.Runs.CompareTo(a.Runs)));
        }

        public void ResetStats()
        {
            Centuries.Clear();
        }

        public void UpdateStats(string teamName, ICricketMatch match)
        {
            var innings = match.GetInnings(teamName, batting: true);
            var battingInnings = innings?.Batting;
            if (battingInnings == null)
            {
                return;
            }
            foreach (BattingEntry battingEntry in battingInnings)
            {
                if (battingEntry.RunsScored >= 100)
                {
                    if (Name == null || battingEntry.Name.Equals(Name))
                    {
                        Centuries.Add(new PlayerScore(teamName, battingEntry, match.MatchData));
                    }
                }
            }
        }

        public void ExportStats(ReportBuilder rb, DocumentElement headerElement)
        {
            if (Centuries.Any())
            {
                _ = rb.WriteTitle("Centuries", headerElement)
                    .WriteTable(Centuries, headerFirstColumn: false);
            }
        }
    }
}
