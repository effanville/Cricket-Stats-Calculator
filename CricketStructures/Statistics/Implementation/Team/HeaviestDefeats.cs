﻿using System.Collections.Generic;
using System.Linq;
using CricketStructures.Match;
using CricketStructures.Season;
using Common.Structure.ReportWriting;
using System.Text;
using CricketStructures.Match.Result;

namespace CricketStructures.Statistics.Implementation.Team
{
    public class HeaviestDefeats : ICricketStat
    {
        public List<BowlingWinningMargin> HeaviestLossByRuns
        {
            get;
            set;
        } = new List<BowlingWinningMargin>();

        public List<BattingWinningMargin> HeaviestLossByWickets
        {
            get;
            set;
        } = new List<BattingWinningMargin>();

        public HeaviestDefeats()
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
                if (!match.MatchResult().IsNoResult && match.SecondInnings.Score().Wickets.Equals(0))
                {
                    BattingWinningMargin margin = new BattingWinningMargin(teamName, match);
                    HeaviestLossByWickets.Add(margin);
                    HeaviestLossByWickets.Sort((a, b) => b.Score.CompareTo(a.Score));
                }
            }
            if (!match.BattedFirst(teamName))
            {
                if (!match.MatchResult().IsNoResult && match.FirstInnings.Score().Runs > match.SecondInnings.Score().Runs + 100)
                {
                    BowlingWinningMargin margin = new BowlingWinningMargin(teamName, match);
                    HeaviestLossByRuns.Add(margin);
                    HeaviestLossByRuns.Sort((a, b) => b.WinningRuns.CompareTo(a.WinningRuns));
                }
            }
        }

        /// <inheritdoc/>
        public StringBuilder ExportStats(DocumentType exportType, DocumentElement headerElement)
        {
            StringBuilder writer = new StringBuilder();
            if (HeaviestLossByRuns.Any())
            {
                TextWriting.WriteTitle(writer, exportType, "Loss by 100 runs", headerElement);
                TableWriting.WriteTable(writer, exportType, HeaviestLossByRuns, headerFirstColumn: false);
            }

            if (HeaviestLossByWickets.Any())
            {
                TextWriting.WriteTitle(writer, exportType, "Loss by 10 wickets", headerElement);
                TableWriting.WriteTable(writer, exportType, HeaviestLossByWickets, headerFirstColumn: false);
            }

            return writer;
        }

        /// <inheritdoc/>
        public void ResetStats()
        {
            HeaviestLossByRuns.Clear();
            HeaviestLossByWickets.Clear();
        }
    }
}