using System.Collections.Generic;
using System.Linq;
using CricketStructures.Match;
using CricketStructures.Season;
using Common.Structure.ReportWriting;
using CricketStructures.Match.Result;

namespace CricketStructures.Statistics.Implementation.Team
{
    public sealed class LargestVictories : ICricketStat
    {
        public List<BowlingWinningMargin> WinBy100Runs
        {
            get;
            set;
        } = new List<BowlingWinningMargin>();

        public List<BattingWinningMargin> WinBy10Wickets
        {
            get;
            set;
        } = new List<BattingWinningMargin>();

        public LargestVictories()
        {
        }

        /// <inheritdoc/>
        public void CalculateStats(ICricketTeam team, MatchType[] matchTypes)
        {
            CricketStatsHelpers.SeasonIterator(
                team.Seasons,
                season => CalculateStats(team.TeamName, season, matchTypes));
        }

        /// <inheritdoc/>
        public void CalculateStats(string teamName, ICricketSeason season, MatchType[] matchTypes)
        {
            CricketStatsHelpers.MatchIterator(
                season,
                matchTypes,
                match => UpdateStats(teamName, match));
        }

        /// <inheritdoc/>
        public void UpdateStats(string teamName, ICricketMatch match)
        {
            if (match.BattedFirst(teamName))
            {
                if (!match.MatchResult().IsNoResult && match.FirstInnings.Score().Runs > match.SecondInnings.Score().Runs + 100)
                {
                    BowlingWinningMargin margin = new BowlingWinningMargin(teamName, match);
                    WinBy100Runs.Add(margin);
                    WinBy100Runs.Sort((a, b) => b.WinningRuns.CompareTo(a.WinningRuns));
                }
            }
            else
            {
                if (!match.MatchResult().IsNoResult && match.SecondInnings.Score().Wickets.Equals(0))
                {
                    BattingWinningMargin margin = new BattingWinningMargin(teamName, match);
                    WinBy10Wickets.Add(margin);
                    WinBy10Wickets.Sort((a, b) => b.Score.CompareTo(a.Score));
                }
            }
        }

        /// <inheritdoc/>
        public void ExportStats(ReportBuilder rb, DocumentElement headerElement)
        {
            if (WinBy100Runs.Any())
            {
                _ = rb.WriteTitle("Wins by 100 Runs", headerElement)
                    .WriteTable(WinBy100Runs, headerFirstColumn: false);
            }

            if (WinBy10Wickets.Any())
            {
                _ = rb.WriteTitle("Wins by 10 wickets", headerElement)
                    .WriteTable(WinBy10Wickets, headerFirstColumn: false);
            }
        }

        /// <inheritdoc/>
        public void ResetStats()
        {
            WinBy100Runs.Clear();
            WinBy10Wickets.Clear();
        }
    }
}
