using System;
using System.Collections.Generic;

using CricketStructures.Match;
using CricketStructures.Player;
using CricketStructures.Season;

namespace CricketStructures.Statistics.Implementation.Player.Fielding
{
    public sealed class SeasonFielding : ISeasonAggregateStat<PlayerFieldingStatistics>
    {
        private readonly int fMinimum;
        public string Title => "Yearly Fielding";

        public PlayerName Name
        {
            get; private set;
        }

        public IReadOnlyList<string> Headers
        {
            get
            {
                if (Name != null)
                {
                    return new[] { "Year", "Catches", "RunOuts", "Keeper Catches", "Keeper Stumpings", "Total" };
                }
                else
                {
                    return new[] { "Name", "Year", "Catches", "RunOuts", "Keeper Catches", "Keeper Stumpings", "Total" };
                }
            }
        }

        public Func<PlayerFieldingStatistics, string[]> OutputValueSelector
        {
            get
            {
                if (Name != null)
                {
                    return value =>
                    new string[]
                    {
                            value.StartYear.Year.ToString(),
                            value.Catches.ToString(),
                            value.RunOuts.ToString(),
                            value.KeeperCatches.ToString(),
                            value.KeeperStumpings.ToString(),
                            value.TotalDismissals.ToString()
                    };
                }
                else
                {
                    return value =>
                    new string[]
                    {
                            value.Name.ToString(),
                            value.StartYear.Year.ToString(),
                            value.Catches.ToString(),
                            value.RunOuts.ToString(),
                            value.KeeperCatches.ToString(),
                            value.KeeperStumpings.ToString(),
                            value.TotalDismissals.ToString()
                    };
                }
            }
        }

        public Func<PlayerName, string, ICricketSeason, MatchType[], PlayerFieldingStatistics> StatGenerator => Create;

        PlayerFieldingStatistics Create(PlayerName name, string teamName, ICricketSeason season, MatchType[] matchTypes)
        {
            return CricketStatsFactory.Generate(CricketStatTypes.PlayerFieldingStats, teamName, season, matchTypes, name) as PlayerFieldingStatistics;
        }

        public Func<PlayerFieldingStatistics, bool> SelectorFunc => Selector;

        bool Selector(PlayerFieldingStatistics stat)
        {
            return stat.TotalDismissals > fMinimum;
        }

        public Comparison<PlayerFieldingStatistics> Comparison => (a, b) => a.StartYear.CompareTo(b.StartYear);
        public SeasonFielding(int minimum, PlayerName name)
        {
            fMinimum = minimum;
            Name = name;
        }

        public bool IncreaseStatScope()
        {
            return true;
        }
    }
}
