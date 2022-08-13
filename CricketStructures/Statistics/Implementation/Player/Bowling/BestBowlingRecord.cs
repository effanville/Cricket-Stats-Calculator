using System;
using System.Collections.Generic;
using System.Linq;

using Common.Structure.ReportWriting;

using CricketStructures.Match;
using CricketStructures.Match.Innings;
using CricketStructures.Player;
using CricketStructures.Season;

namespace CricketStructures.Statistics.Implementation.Player.Bowling
{
    internal sealed class BestBowlingRecord : ICricketStat
    {
        private int MinimumNumberWickets;
        private readonly PlayerName Name;
        public List<BowlingPerformance> ManyWickets
        {
            get;
            set;
        } = new List<BowlingPerformance>();

        public BestBowlingRecord()
        {
        }

        public BestBowlingRecord(int minimumNumberWickets)
            : this()
        {
            MinimumNumberWickets = minimumNumberWickets;
        }
        public BestBowlingRecord(int minimumNumberWickets, PlayerName name)
            : this(minimumNumberWickets)
        {
            Name = name;
        }

        public void CalculateStats(ICricketTeam team, MatchType[] matchTypes)
        {
            // try to display many.
            int minimumNumbertoDisplay = 10;
            int numberEntries = 0;
            int previousMinNumber = MinimumNumberWickets;
            do
            {
                CricketStatsHelpers.SeasonIterator(
                team.Seasons,
                season => CalculateStats(team.TeamName, season, matchTypes),
                preCycleAction: ResetStats);
                numberEntries = ManyWickets.Count;
                if (numberEntries < minimumNumbertoDisplay)
                {
                    previousMinNumber = MinimumNumberWickets;
                    MinimumNumberWickets = Math.Max(0, MinimumNumberWickets - 5);
                }
            }
            while (numberEntries < minimumNumbertoDisplay && previousMinNumber != MinimumNumberWickets);
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
                AddManyWickets);
            void AddManyWickets(BowlingEntry bowlingEntry)
            {
                if (Name == null || bowlingEntry.Name.Equals(Name))
                {
                    if (bowlingEntry.Wickets >= MinimumNumberWickets)
                    {
                        ManyWickets.Add(new BowlingPerformance(teamName, bowlingEntry, match.MatchData));
                    }
                }
            }
        }

        public void ExportStats(ReportBuilder rb, DocumentElement headerElement)
        {
            if (ManyWickets.Any())
            {
                string title = MinimumNumberWickets == 0 ? "All Bowling Performances" : $"Bowling performances over {MinimumNumberWickets} wickets";
                _ = rb.WriteTitle(title, headerElement)
                    .WriteTable(ManyWickets, headerFirstColumn: false);
            }
        }
    }
}
