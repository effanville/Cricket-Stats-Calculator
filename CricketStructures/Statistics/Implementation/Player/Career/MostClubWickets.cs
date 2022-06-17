﻿using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.Structure.ReportWriting;

using CricketStructures.Match;
using CricketStructures.Match.Innings;
using CricketStructures.Season;
using CricketStructures.Statistics.Implementation.Player.Model;

namespace CricketStructures.Statistics.Implementation.Player.Career
{
    internal sealed class MostClubWickets : ICricketStat
    {
        public List<NameDurationRecord<int>> ClubWickets
        {
            get;
            set;
        } = new List<NameDurationRecord<int>>();

        public void CalculateStats(ICricketTeam team, MatchType[] matchTypes)
        {
            CricketStatsHelpers.SeasonIterator(
                team.Seasons,
                season => CalculateStats(team.TeamName, season, matchTypes),
                postCycleAction: () => ClubWickets.Sort((a, b) => b.Value.CompareTo(a.Value)));
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
            CricketStatsHelpers.BowlingIterator(match, teamName, entry => CalculateWickets(entry));
            void CalculateWickets(BowlingEntry bowling)
            {
                var playerWickets = ClubWickets.FirstOrDefault(run => run.Name.Equals(bowling.Name));
                if (playerWickets != null)
                {
                    playerWickets.UpdateValue(match.MatchData.Date, bowling.Wickets);
                }
                else
                {
                    ClubWickets.Add(new NameDurationRecord<int>("MostWickets", bowling.Name, match.MatchData.Date, bowling.Wickets, (value, otherValue) => value + otherValue));
                }
            }
        }

        public void ResetStats()
        {
            ClubWickets.Clear();
        }

        public StringBuilder ExportStats(DocumentType exportType, DocumentElement headerElement)
        {
            var writer = new StringBuilder();
            TextWriting.WriteTitle(writer, exportType, "Wickets Taken", headerElement);
            var export = ClubWickets.Take(5);
            TableWriting.WriteTableFromEnumerable(writer, exportType, new string[] { "Name", "StartYear", "End Year", "Wickets" }, export.Select(value => new string[] { value.Name.ToString(), value.Start.ToShortDateString(), value.End.ToShortDateString(), value.Value.ToString() }), headerFirstColumn: false);
            return writer;
        }


    }
}