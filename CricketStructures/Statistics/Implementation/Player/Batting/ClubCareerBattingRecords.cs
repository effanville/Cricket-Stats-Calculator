using System.Collections.Generic;
using System.Linq;

using Common.Structure.ReportWriting;

using CricketStructures.Match;
using CricketStructures.Player;
using CricketStructures.Season;

namespace CricketStructures.Statistics.Implementation.Player.Batting
{
    public sealed class ClubCareerBattingRecords : ICricketStat
    {
        public IDictionary<PlayerName, CareerBattingRecord> PlayerBatting
        {
            get;
            set;
        } = new Dictionary<PlayerName, CareerBattingRecord>();


        public ClubCareerBattingRecords()
        {
        }
        public ClubCareerBattingRecords(ICricketTeam team)
        {
            CalculateStats(team, MatchHelpers.AllMatchTypes);
        }

        public void CalculateStats(ICricketTeam team, MatchType[] matchTypes)
        {
            foreach (var player in team.Players())
            {
                PlayerBatting.Add(player.Name, new CareerBattingRecord(player.Name, team));
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

        public void ExportStats(ReportBuilder rb, DocumentElement headerElement)
        {
            if (PlayerBatting.Any())
            {
                var values = PlayerBatting.Values.ToList();
                values.Sort((a, b) => a.Name.CompareTo(b.Name));
                _ = rb.WriteTitle("Overall Batting Performance", headerElement)
                    .WriteTable(PlayerBatting.Values, headerFirstColumn: false);
            }
        }
    }
}
