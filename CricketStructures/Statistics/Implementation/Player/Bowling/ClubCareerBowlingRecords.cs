using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.Structure.ReportWriting;

using CricketStructures.Match;
using CricketStructures.Player;
using CricketStructures.Season;

namespace CricketStructures.Statistics.Implementation.Player.Bowling
{
    public sealed class ClubCareerBowlingRecords : ICricketStat
    {
        public IDictionary<PlayerName, CareerBowlingRecord> PlayerBowling
        {
            get;
            set;
        } = new Dictionary<PlayerName, CareerBowlingRecord>();


        public ClubCareerBowlingRecords()
        {
        }
        public ClubCareerBowlingRecords(ICricketTeam team)
        {
            CalculateStats(team, MatchHelpers.AllMatchTypes);
        }

        public void CalculateStats(ICricketTeam team, MatchType[] matchTypes)
        {
            foreach (var player in team.Players())
            {
                PlayerBowling.Add(player.Name, new CareerBowlingRecord(player.Name, team));
            }
        }

        public void CalculateStats(string teamName, ICricketSeason season, MatchType[] matchTypes)
        {
        }

        public void UpdateStats(string teamName, ICricketMatch match)
        {
        }

        public void ResetStats()
        {
        }

        public StringBuilder ExportStats(DocumentType exportType, DocumentElement headerElement)
        {
            var writer = new StringBuilder();
            if (PlayerBowling.Any())
            {
                var values = PlayerBowling.Values.ToList();
                values.Sort((a, b) => a.Name.CompareTo(b.Name));
                TextWriting.WriteTitle(writer, exportType, "Overall Bowling Performance", headerElement);
                TableWriting.WriteTable(writer, exportType, values, headerFirstColumn: false);
            }

            return writer;
        }
    }
}
