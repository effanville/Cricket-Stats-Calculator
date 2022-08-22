using System;
using System.Collections.Generic;
using System.Linq;

using Common.Structure.NamingStructures;

using CricketStructures.Match;
using CricketStructures.Player;
using CricketStructures.Statistics.Implementation.Collection;

namespace CricketStructures.Statistics.Implementation.Team
{
    internal sealed class TeamAgainstRecord : IMatchAggregateStat<Labell<string, TeamRecord>>
    {
        public string Title => "Record against each team";

        public PlayerName Name => null;

        public IReadOnlyList<string> Headers => new string[] { "Opposition", "Played", "Won", "Lost", "Win Ratio" };

        public Func<Labell<string, TeamRecord>, IReadOnlyList<string>> OutputValueSelector => record => new string[]
                {
                                record.Label.ToString(),
                                record.Instance.Played.ToString(),
                                record.Instance.Won.ToString(),
                                record.Instance.Lost.ToString(),
                                record.Instance.WinRatio.ToString()
                };

        public Action<string, ICricketMatch, List<Labell<string, TeamRecord>>> AddStatsAction => AddStats;
        void AddStats(string teamName, ICricketMatch match, List<Labell<string, TeamRecord>> stats)
        {
            var oppositionName = match.MatchData.OppositionName(teamName);
            var stat = stats.FirstOrDefault(stat => stat.Label.Equals(oppositionName));
            if (stat != null)
            {
                stat.Instance.UpdateStats(teamName, match);
            }
            else
            {
                var teamRecord = new TeamRecord();
                teamRecord.UpdateStats(teamName, match);
                stats.Add(new Labell<string, TeamRecord>(oppositionName, teamRecord));
            }
        }

        public Comparison<Labell<string, TeamRecord>> Comparison => (a, b) => a.CompareTo(b);

        public bool IncreaseStatScope()
        {
            throw new NotImplementedException();
        }
    }
}
