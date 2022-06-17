﻿using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.Structure.ReportWriting;

using CricketStructures.Match;
using CricketStructures.Match.Innings;
using CricketStructures.Player;
using CricketStructures.Season;
using CricketStructures.Statistics.Implementation.Player.Model;

namespace CricketStructures.Statistics.Implementation.Player.Batting
{
    internal sealed class HighScoreRecord : ICricketStat
    {
        private readonly PlayerName Name;
        public Dictionary<PlayerName, NamedRecord<int, int>> ScoresPast50
        {
            get;
            set;
        } = new Dictionary<PlayerName, NamedRecord<int, int>>();

        public HighScoreRecord()
        {
        }

        public HighScoreRecord(PlayerName name)
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
                match => UpdateStats(teamName, match));
        }

        public void ResetStats()
        {
            ScoresPast50.Clear();
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
                if (Name == null || battingEntry.Name.IsEqualTo(Name))
                {
                    if (battingEntry.RunsScored >= 100)
                    {
                        if (ScoresPast50.TryGetValue(battingEntry.Name, out var value))
                        {
                            value.UpdateValue(1, 0);
                        }
                        else
                        {
                            ScoresPast50[battingEntry.Name] = new NamedRecord<int, int>("ScoresPastFifty", battingEntry.Name, 1, 0, (a, b) => a + b, (c, d) => c + d);
                        }
                    }

                    if (battingEntry.RunsScored >= 50 && battingEntry.RunsScored < 100)
                    {
                        if (ScoresPast50.TryGetValue(battingEntry.Name, out var value))
                        {
                            value.UpdateValue(0, 1);
                        }
                        else
                        {
                            ScoresPast50[battingEntry.Name] = new NamedRecord<int, int>("ScoresPastFifty", battingEntry.Name, 0, 1, (a, b) => a + b, (c, d) => c + d);
                        }
                    }
                }
            }
        }

        public StringBuilder ExportStats(DocumentType exportType, DocumentElement headerElement)
        {
            var writer = new StringBuilder();
            if (ScoresPast50.Any())
            {
                var values = ScoresPast50.Values.ToList();
                values.Sort((a, b) => b.Value.CompareTo(a.Value));
                TextWriting.WriteTitle(writer, exportType, "Number Scores Past Fifty", headerElement);
                TableWriting.WriteTableFromEnumerable(writer, exportType, new string[] { "Name", "Centuries", "Fifties" }, values.Select(value => new string[] { value.Name.ToString(), value.Value.ToString(), value.SecondValue.ToString() }), headerFirstColumn: false);
            }

            return writer;
        }
    }
}