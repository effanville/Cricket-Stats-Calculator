﻿using System;
using System.Collections.Generic;
using System.Linq;

using Common.Structure.ReportWriting;

using CricketStructures.Match;
using CricketStructures.Player;
using CricketStructures.Season;
using CricketStructures.Statistics.Implementation.Collection;

namespace CricketStructures.Statistics.Implementation.Player.Batting
{
    internal sealed class SeasonAverageRecord : ICricketStat
    {
        private int MinimumAverage;
        private readonly PlayerName Name;
        public List<SeasonRuns> SeasonAverage
        {
            get;
            set;
        } = new List<SeasonRuns>();


        public SeasonAverageRecord()
        {
        }

        public SeasonAverageRecord(int minimumAverage)
            : this()
        {
            MinimumAverage = minimumAverage;
        }
        public SeasonAverageRecord(int minimumAverage, PlayerName name)
            : this(minimumAverage)
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
            var playerNames = Name == null ? season.Players(teamName, matchTypes) : new List<PlayerName>() { Name };
            List<PlayerBriefStatistics> playerStats = playerNames.Select(name => new PlayerBriefStatistics(teamName, name, season, matchTypes)).ToList();
            IEnumerable<PlayerBriefStatistics> goodAverage = playerStats.Where(player => player.Played.TotalGamesPlayed > 5 && player.BattingStats.Average > MinimumAverage);
            SeasonAverage.AddRange(goodAverage.Select(element => new SeasonRuns(element.SeasonYear, element.Name, element.BattingStats.TotalInnings, element.BattingStats.TotalNotOut, element.BattingStats.TotalRuns, element.BattingStats.Average)));

            SeasonAverage.Sort((a, b) => b.Average.CompareTo(a.Average));
        }

        public void ResetStats()
        {
            SeasonAverage.Clear();
        }

        public void UpdateStats(string teamName, ICricketMatch match)
        {
            throw new NotImplementedException();
        }

        public void ExportStats(ReportBuilder rb, DocumentElement headerElement)
        {
            if (SeasonAverage.Any())
            {
                _ = rb.WriteTitle($"Average over {MinimumAverage} in a season", headerElement)
                    .WriteTable(SeasonAverage, headerFirstColumn: false);
            }
        }
    }
}