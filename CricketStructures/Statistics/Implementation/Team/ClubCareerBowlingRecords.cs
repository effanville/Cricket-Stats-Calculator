using System.Collections.Generic;
using System.Linq;

using Common.Structure.ReportWriting;

using CricketStructures.Match;
using CricketStructures.Player;
using CricketStructures.Season;
using CricketStructures.Statistics.Implementation.Player;

namespace CricketStructures.Statistics.Implementation.Team
{
    public sealed class ClubCareerBowlingRecords : ICricketStat
    {
        public IDictionary<PlayerName, PlayerBowlingRecord> PlayerBowling
        {
            get;
            set;
        } = new Dictionary<PlayerName, PlayerBowlingRecord>();


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
                PlayerBowling.Add(player.Name, new PlayerBowlingRecord(player.Name, team));
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
            if (PlayerBowling.Any())
            {
                var values = PlayerBowling.Values.ToList();
                values.Sort((a, b) => a.Name.CompareTo(b.Name));
                _ = rb.WriteTitle("Overall Bowling Performance", headerElement)
                    .WriteTable(values, headerFirstColumn: false);
            }
        }
    }
}
