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
    internal sealed class TeamRunsRecord : ISeasonAggregateStat<DatedRecord<ClubRunsRecord>>
    {
        public string Title => "Yearly Runs Scored";

        public PlayerName Name => new PlayerName("empty", "empty");

        public IReadOnlyList<string> Headers => new string[] { "Year", "Number Runs", "Runs Per Game", "Runs Per Wicket" };

        public Func<DatedRecord<ClubRunsRecord>, IReadOnlyList<string>> OutputValueSelector => record =>
        new string[] 
        { 
            record.Date.Year.ToString(),
            record.Value.NumberRuns.ToString(),
            record.Value.RunsPerGame.TruncateToString(),
            record.Value.RunsPerWicket.TruncateToString()
        };

        public Func<PlayerName, string, ICricketSeason, MatchType[], DatedRecord<ClubRunsRecord>> StatGenerator => Create;

        DatedRecord<ClubRunsRecord> Create(PlayerName name, string teamName, ICricketSeason season, MatchType[] matchTypes)
        {
            var newEntry = new ClubRunsRecord();
            newEntry.CalculateStats(teamName, season, matchTypes);
            return new DatedRecord<ClubRunsRecord>($"{season.Year.Year}", season.Year, newEntry, null);
        }

        public Func<DatedRecord<ClubRunsRecord>, bool> SelectorFunc => record => true;

        public Comparison<DatedRecord<ClubRunsRecord>> Comparison => DatedRecordComparisons.InverseDateCompare<ClubRunsRecord>();

        public bool IncreaseStatScope()
        {
            return true;
        }
    }
}