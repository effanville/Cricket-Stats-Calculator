using System.Collections.Generic;
using System.Linq;

using Common.Structure.ReportWriting;

using CricketStructures.Match;
using CricketStructures.Player;
using CricketStructures.Season;
using CricketStructures.Statistics.Implementation.Player.Fielding;

namespace CricketStructures.Statistics.Implementation.Team
{
    public sealed class ClubCareerFieldingRecords : ICricketStat
    {
        bool _IsAllTime = false;
        public IDictionary<PlayerName, PlayerFieldingRecord> PlayerFielding
        {
            get;
            set;
        } = new Dictionary<PlayerName, PlayerFieldingRecord>();


        public ClubCareerFieldingRecords()
        {
        }

        public ClubCareerFieldingRecords(ICricketTeam team)
        {
            CalculateStats(team, MatchHelpers.AllMatchTypes);
        }

        public void CalculateStats(ICricketTeam team, MatchType[] matchTypes)
        {
            _IsAllTime = true;
            foreach (var player in team.Players())
            {
                PlayerFielding.Add(player.Name, new PlayerFieldingRecord(player.Name, team, matchTypes));
            }
        }

        public void CalculateStats(string teamName, ICricketSeason season, MatchType[] matchTypes)
        {
            foreach (var player in season.Players(teamName, matchTypes))
            {
                PlayerFielding.Add(player, new PlayerFieldingRecord(player, teamName, season, matchTypes));
            }
        }

        public void UpdateStats(string teamName, ICricketMatch match)
        {
        }

        public void ResetStats()
        {
            PlayerFielding.Clear();
        }

        public void Finalise()
        {
        }

        public void ExportStats(ReportBuilder rb, DocumentElement headerElement)
        {
            if (PlayerFielding.Any())
            {
                var values = PlayerFielding.Values.ToList();

                if (_IsAllTime)
                {
                    values.Sort((a, b) => a.Name.CompareTo(b.Name));
                }
                else
                {
                    values.Sort((a, b) => b.TotalDismissals.CompareTo(a.TotalDismissals));
                }

                _ = values.RemoveAll(field => field.TotalDismissals.Equals(0));
                _ = rb.WriteTitle("Overall Fielding Performance", headerElement)
                    .WriteTableFromEnumerable(PlayerFieldingRecord.Headers(true, !_IsAllTime, _IsAllTime), values.Select(val => val.Values(true, !_IsAllTime, _IsAllTime)), headerFirstColumn: false);
            }
        }
    }
}
