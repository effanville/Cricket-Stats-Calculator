using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.Structure.ReportWriting;

using CricketStructures.Match;
using CricketStructures.Match.Innings;
using CricketStructures.Player;
using CricketStructures.Season;

namespace CricketStructures.Statistics.Implementation.Player.Fielding
{
    internal class SingleInningsDismissals : ICricketStat
    {
        private readonly PlayerName Name;
        public List<InningsDismissals> DismissalsInOneInnings
        {
            get;
            set;
        } = new List<InningsDismissals>();

        public SingleInningsDismissals()
        {
        }

        public SingleInningsDismissals(PlayerName name)
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
                postCycleAction: () => DismissalsInOneInnings.Sort((a, b) => b.Dismissals.CompareTo(a.Dismissals)));
        }

        public void ResetStats()
        {
            DismissalsInOneInnings.Clear();
        }

        public void UpdateStats(string teamName, ICricketMatch match)
        {
            foreach (FieldingEntry field in match.GetAllFielding(teamName))
            {
                if (Name == null || field.Name.Equals(Name))
                {
                    if (field.TotalDismissals() > 4)
                    {
                        DismissalsInOneInnings.Add(new InningsDismissals(teamName, field, match.MatchData));
                    }
                }
            }
        }

        public StringBuilder ExportStats(DocumentType exportType, DocumentElement headerElement)
        {
            var writer = new StringBuilder();
            if (DismissalsInOneInnings.Any())
            {
                TextWriting.WriteTitle(writer, exportType, "Most Dismissals in on Innings", headerElement);
                TableWriting.WriteTable(writer, exportType, DismissalsInOneInnings, headerFirstColumn: false);
            }

            return writer;
        }
    }
}
