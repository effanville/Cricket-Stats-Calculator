using System;
using System.Collections.Generic;
using System.Linq;

using CricketStructures.Match;
using CricketStructures.Match.Innings;
using CricketStructures.Player;
using CricketStructures.Season;

namespace CricketStructures.Statistics
{
    internal static class CricketStatsHelpers
    {
        /// <summary>
        /// Cycles through a list of seasons and performs an action on each season.
        /// Also is able to perform an action prior and post this cycle.
        /// </summary>
        public static void SeasonIterator(
            IReadOnlyList<ICricketSeason> seasons,
            Action<ICricketSeason> seasonAction,
            Action preCycleAction = null,
            Action postCycleAction = null)
        {
            preCycleAction?.Invoke();
            foreach (ICricketSeason season in seasons)
            {
                seasonAction(season);
            }

            postCycleAction?.Invoke();
        }

        /// <summary>
        /// Cycles through the matches in a season and performs an action on each match.
        /// Also is able to perform an action prior and post this cycle.
        /// </summary>
        public static void MatchIterator(
            ICricketSeason season,
            MatchType[] matchTypes,
            Action<ICricketMatch> matchAction,
            Action preCycleAction = null,
            Action postCycleAction = null)
        {
            preCycleAction?.Invoke();
            foreach (ICricketMatch match in season.Matches)
            {
                if (matchTypes.Contains(match.MatchData.Type))
                {
                    matchAction(match);
                }
            }

            postCycleAction?.Invoke();
        }

        /// <summary>
        /// Cycles through the batting for the team in the match and performs an action on each entry.
        /// </summary>
        public static void BattingIterator(
            ICricketMatch match,
            string teamName,
            Action<BattingEntry> matchAction)
        {
            var innings = match.GetInnings(teamName, batting: true);
            var battingInnings = innings?.Batting;
            if (battingInnings == null)
            {
                return;
            }
            foreach (BattingEntry batting in battingInnings)
            {
                matchAction(batting);
            }
        }

        public static void BattingIterator(
            ICricketMatch match,
            string teamName,
            PlayerName playerName,
            Action<BattingEntry> matchAction)
        {
            var batting = match.GetBatting(teamName, playerName);
            if (batting == null)
            {
                return;
            }

            matchAction(batting);
        }

        public static void BowlingIterator(
            ICricketMatch match,
            string teamName,
            Action<BowlingEntry> matchAction)
        {
            var innings = match.GetInnings(teamName, batting: false);
            var bowlingInnings = innings?.Bowling;
            if (bowlingInnings == null)
            {
                return;
            }
            foreach (BowlingEntry bowling in bowlingInnings)
            {
                matchAction(bowling);
            }
        }

        public static void BowlingIterator(
            ICricketMatch match,
            string teamName,
            PlayerName playerName,
            Action<BowlingEntry> matchAction)
        {
            var batting = match.GetBowling(teamName, playerName);
            if (batting == null)
            {
                return;
            }

            matchAction(batting);
        }

        public static void FieldingIterator(
            ICricketMatch match,
            string teamName,
            Action<FieldingEntry> matchAction)
        {
            var innings = match.GetInnings(teamName, batting: false);
            List<FieldingEntry> allFielding = new List<FieldingEntry>();
            foreach (var player in innings.Players(teamName))
            {
                allFielding.Add(innings.GetFielding(teamName, player));
            }

            foreach (FieldingEntry fielding in allFielding)
            {
                matchAction(fielding);
            }
        }

        public static void FieldingIterator(
            ICricketMatch match,
            string teamName,
            PlayerName playerName,
            Action<FieldingEntry> matchAction)
        {
            var batting = match.GetFielding(teamName, playerName);
            if (batting == null)
            {
                return;
            }

            matchAction(batting);
        }
    }
}
