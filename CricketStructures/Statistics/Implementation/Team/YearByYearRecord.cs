using System;
using System.Collections.Generic;

using Common.Structure.NamingStructures;

using CricketStructures.Match;
using CricketStructures.Player;
using CricketStructures.Season;
using CricketStructures.Statistics.Implementation.Collection;

namespace CricketStructures.Statistics.Implementation.Team
{
    internal class YearByYearRecord : ISeasonAggregateStat<DatedRecord<TeamRecord>>
    {
        public string Title => "Yearly Records";

        public PlayerName Name => new PlayerName("empty", "empty");

        public IReadOnlyList<string> Headers => new string[] { "Year", "Played", "Won", "Lost", "Win Ratio" };

        public Func<DatedRecord<TeamRecord>, IReadOnlyList<string>> OutputValueSelector => record => new List<string>()
                {
                    record.Date.Year.ToString(),
                    record.Value.Played.ToString(),
                    record.Value.Won.ToString(),
                    record.Value.Lost.ToString(),
                    record.Value.WinRatio.ToString()
                };

        public Func<PlayerName, string, ICricketSeason, MatchType[], DatedRecord<TeamRecord>> StatGenerator => Create;
        DatedRecord<TeamRecord> Create(PlayerName name, string teamName, ICricketSeason season, MatchType[] matchTypes)
        {
            var newEntry = new TeamRecord();
            newEntry.CalculateStats(teamName, season, matchTypes);
            return new DatedRecord<TeamRecord>($"{season.Year.Year}", season.Year, newEntry, null);
        }

        public Func<DatedRecord<TeamRecord>, bool> SelectorFunc => a => true;

        public Comparison<DatedRecord<TeamRecord>> Comparison => DatedRecordComparisons.DateCompare<TeamRecord>();
        public bool IncreaseStatScope()
        {
            return true;
        }
    }
}
