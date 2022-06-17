using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.Structure.ReportWriting;

using CricketStructures.Match;
using CricketStructures.Match.Innings;
using CricketStructures.Player;
using CricketStructures.Season;
using CricketStructures.Statistics.Implementation.Player.Model;

namespace CricketStructures.Statistics.Implementation.Player.Bowling
{
    internal class Over5Wickets : ICricketStat
    {
        private readonly PlayerName Name;
        public List<BowlingPerformance> ManyWickets
        {
            get;
            set;
        } = new List<BowlingPerformance>();

        public Over5Wickets()
        {
        }

        public Over5Wickets(PlayerName name)
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
                postCycleAction: () => ManyWickets.Sort((a, b) => b.Wickets.CompareTo(a.Wickets)));
        }

        public void ResetStats()
        {
            ManyWickets.Clear();
        }

        public void UpdateStats(string teamName, ICricketMatch match)
        {
            CricketStatsHelpers.BowlingIterator(
                match,
                teamName,
                entry => AddManyWickets(entry));
            void AddManyWickets(BowlingEntry bowlingEntry)
            {
                if (Name == null || bowlingEntry.Name.Equals(Name))
                {
                    if (bowlingEntry.Wickets >= 5)
                    {
                        ManyWickets.Add(new BowlingPerformance(bowlingEntry, match.MatchData));
                    }
                }
            }
        }

        public StringBuilder ExportStats(DocumentType exportType, DocumentElement headerElement)
        {
            var stringBuilder = new StringBuilder();
            if (ManyWickets.Any())
            {
                TextWriting.WriteTitle(stringBuilder, exportType, "Five Wicket Hauls", headerElement);
                TableWriting.WriteTable(stringBuilder, exportType, ManyWickets, headerFirstColumn: false);
            }

            return stringBuilder;
        }
    }
}
