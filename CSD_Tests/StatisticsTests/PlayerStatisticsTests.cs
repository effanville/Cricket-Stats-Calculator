﻿using System;
using System.Collections.Generic;

using CricketStructures.Match;
using CricketStructures.Match.Innings;
using CricketStructures.Player;
using CricketStructures.Statistics;
using CricketStructures.Statistics.Collection;
using CricketStructures.Statistics.Collection.Implementation;
using CricketStructures.Statistics.Implementation.Player.Batting;
using CricketStructures.Statistics.Implementation.Player.Bowling;
using CricketStructures.Statistics.Implementation.Player.Fielding;

using NUnit.Framework;

namespace CricketStructures.Tests.StatisticsTests
{
    [TestFixture]
    public sealed class PlayerStatisticsTests
    {
        private const string TeamName = "homeTeam";

        [TestCase(0, new double[] { 2, 1, 30, 30, 20 })]
        [TestCase(1, new double[] { 3, 1, 41, 20.5, 21 })]
        public void PlayerBattingStats(int valueIndex, double[] expected)
        {
            var values = GetValues(valueIndex);
            var player = new PlayerName("Root", "Joe");
            var batting = new List<(int, Wicket)>(values.Item1);
            var bowling = new List<(int, int, int, int)>(values.Item2);
            var fielding = new List<(int, int, int, int)>(values.Item3);
            var season = TestCaseInstances.CreateTestSeason(TeamName, player, batting, bowling, fielding);
            var stats = new PlayerBattingRecord(player);
            stats.CalculateStats(TeamName, season, MatchHelpers.AllMatchTypes);

            Assert.AreEqual(expected[0], stats.TotalInnings);
            Assert.AreEqual(expected[1], stats.TotalNotOut);
            Assert.AreEqual(expected[2], stats.TotalRuns);
            Assert.AreEqual(expected[3], stats.Average);
            Assert.AreEqual(expected[4], stats.Best.Runs);
        }

        [TestCase(0, new double[] { 40, 4, 20, 1, 10, 2, 40, 1 })]
        [TestCase(1, new double[] { 40, 4, 20, 1, 10, 2, 40, 1 })]
        public void PlayerBowlingStats(int valueIndex, double[] expected)
        {
            var values = GetValues(valueIndex);
            var player = new PlayerName("Root", "Joe");
            var batting = new List<(int, Wicket)>(values.Item1);
            var bowling = new List<(int, int, int, int)>(values.Item2);
            var fielding = new List<(int, int, int, int)>(values.Item3);
            var season = TestCaseInstances.CreateTestSeason(TeamName, player, batting, bowling, fielding);
            var stats = new PlayerBowlingRecord(player);
            stats.CalculateStats(TeamName, season, MatchHelpers.AllMatchTypes);

            Assert.AreEqual(expected[0], stats.Average);
            Assert.AreEqual(expected[1], stats.Economy);
            Assert.AreEqual(expected[2], stats.BestFigures.RunsConceded);
            Assert.AreEqual(expected[3], stats.BestFigures.Wickets);
            Assert.AreEqual((Over)expected[4], stats.TotalOvers);
            Assert.AreEqual(expected[5], stats.TotalMaidens);
            Assert.AreEqual(expected[6], stats.TotalRunsConceded);
            Assert.AreEqual(expected[7], stats.TotalWickets);
        }

        [TestCase(0, new int[] { 0, 0, 1, 1, 2, 0, 2 })]
        [TestCase(1, new int[] { 0, 0, 3, 1, 4, 0, 4 })]
        public void CalculatePlayerFieldingStats(int valueIndex, int[] expected)
        {
            var values = GetValues(valueIndex);
            var player = new PlayerName("Root", "Joe");
            var batting = new List<(int, Wicket)>(values.Item1);
            var bowling = new List<(int, int, int, int)>(values.Item2);
            var fielding = new List<(int, int, int, int)>(values.Item3);
            var season = TestCaseInstances.CreateTestSeason(TeamName, player, batting, bowling, fielding);
            var generalStats = CricketStatsFactory.Generate(CricketStatTypes.PlayerFieldingStats, TeamName, season, MatchHelpers.AllMatchTypes, player);
            var stats = generalStats as PlayerFieldingRecord;
            Assert.AreEqual(expected[0], stats.KeeperCatches, "Keeper Catches");
            Assert.AreEqual(expected[1], stats.KeeperStumpings, "stumpings");
            Assert.AreEqual(expected[2], stats.Catches, "Catches");
            Assert.AreEqual(expected[3], stats.RunOuts, "RunOuts");
            Assert.AreEqual(expected[4], stats.TotalDismissals);
            Assert.AreEqual(expected[5], stats.TotalKeeperDismissals);
            Assert.AreEqual(expected[6], stats.TotalNonKeeperDismissals);
        }

        [TestCase(0, new int[] { 2, 0 })]
        [TestCase(1, new int[] { 3, 0 })]
        public void PlayerSeasonStats(int valueIndex, int[] expected)
        {
            var values = GetValues(valueIndex);
            var player = new PlayerName("Root", "Joe");
            var batting = new List<(int, Wicket)>(values.Item1);
            var bowling = new List<(int, int, int, int)>(values.Item2);
            var fielding = new List<(int, int, int, int)>(values.Item3);
            var season = TestCaseInstances.CreateTestSeason(TeamName, player, batting, bowling, fielding);
            var stats = StatsCollectionBuilder.StandardStat(
                StatCollection.PlayerSeason,
                MatchHelpers.AllMatchTypes,
                teamName: TeamName,
                playerName: player,
                season: season) as PlayerBriefStatistics;
            Assert.AreEqual(expected[0], stats.Played.MatchesPlayed);
            Assert.AreEqual(expected[1], stats.Played.TotalMom);
        }

        private Tuple<(int, Wicket)[], (int, int, int, int)[], (int, int, int, int)[]> GetValues(int valueIndex)
        {
            (int, Wicket)[] batting;
            (int, int, int, int)[] bowling;
            (int, int, int, int)[] fielding;
            switch (valueIndex)
            {
                case 0:
                default:
                    batting = new (int, Wicket)[] { (10, Wicket.NotOut), (20, Wicket.Bowled) };
                    bowling = new (int, int, int, int)[] { (4, 2, 20, 1), (6, 0, 20, 0) };
                    fielding = new (int, int, int, int)[] { (1, 0, 0, 0), (0, 1, 0, 0) };
                    return new Tuple<(int, Wicket)[], (int, int, int, int)[], (int, int, int, int)[]>(batting, bowling, fielding);
                case 1:
                    batting = new (int, Wicket)[] { (0, Wicket.NotOut), (20, Wicket.Bowled), (21, Wicket.Stumped) };
                    bowling = new (int, int, int, int)[] { (4, 2, 20, 1), (6, 0, 20, 0), (0, 0, 0, 0) };
                    fielding = new (int, int, int, int)[] { (1, 0, 0, 0), (0, 1, 0, 0), (2, 0, 0, 0) };
                    return new Tuple<(int, Wicket)[], (int, int, int, int)[], (int, int, int, int)[]>(batting, bowling, fielding);
            }
        }
    }
}
