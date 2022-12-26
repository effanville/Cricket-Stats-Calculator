using System;
using System.Collections.Generic;

using Common.Structure.Extensions;
using Common.Structure.NamingStructures;

using CricketStructures.Match;
using CricketStructures.Player;
using CricketStructures.Season;
using CricketStructures.Statistics.Implementation.Collection;

namespace CricketStructures.Statistics.Implementation.Team
{
    internal sealed class TeamWicketsRecord : ISeasonAggregateStat<DatedRecord<ClubWicketsRecord>>
    {
        public string Title => "Yearly Wickets Taken";

        public PlayerName Name => new PlayerName("empty", "empty");

        public IReadOnlyList<string> Headers => new string[] { "Year", "Number Wickets", "Wickets Per Game", "Runs Per Wicket" };

        public Func<DatedRecord<ClubWicketsRecord>, IReadOnlyList<string>> OutputValueSelector => record =>
        new string[] 
        { 
            record.Date.Year.ToString(),
            record.Value.NumberWickets.ToString(),
            record.Value.WicketsPerGame.TruncateToString(),
            record.Value.RunsPerWicket.TruncateToString()
        };

        public Func<PlayerName, string, ICricketSeason, MatchType[], DatedRecord<ClubWicketsRecord>> StatGenerator => Create;

        DatedRecord<ClubWicketsRecord> Create(PlayerName name, string teamName, ICricketSeason season, MatchType[] matchTypes)
        {
            var newEntry = new ClubWicketsRecord();
            newEntry.CalculateStats(teamName, season, matchTypes);
            return new DatedRecord<ClubWicketsRecord>($"{season.Year.Year}", season.Year, newEntry, null);
        }

        public Func<DatedRecord<ClubWicketsRecord>, bool> SelectorFunc => record => true;

        public Comparison<DatedRecord<ClubWicketsRecord>> Comparison => DatedRecordComparisons.InverseDateCompare<ClubWicketsRecord>();

        public bool IncreaseStatScope()
        {
            return true;
        }
    }
}