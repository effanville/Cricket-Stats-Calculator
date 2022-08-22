using System.Collections.Generic;
using System.Linq;

using Common.Structure.ReportWriting;

using CricketStructures.Match;
using CricketStructures.Player;
using CricketStructures.Season;
using CricketStructures.Statistics.Implementation.Player.Batting;

namespace CricketStructures.Statistics.Implementation.Team
{
    public sealed class ClubCareerBattingRecords : ICricketStat
    {
        bool _IsAllTime = false;
        public IDictionary<PlayerName, PlayerBattingRecord> PlayerBatting
        {
            get;
            set;
        } = new Dictionary<PlayerName, PlayerBattingRecord>();


        public ClubCareerBattingRecords()
        {
        }
        public ClubCareerBattingRecords(ICricketTeam team)
        {
            CalculateStats(team, MatchHelpers.AllMatchTypes);
        }

        /// <inheritdoc/>
        public void CalculateStats(ICricketTeam team, MatchType[] matchTypes)
        {
            _IsAllTime = true;
            foreach (var player in team.Players())
            {
                PlayerBatting.Add(player.Name, new PlayerBattingRecord(player.Name, team, matchTypes));
            }
        }

        /// <inheritdoc/>
        public void CalculateStats(string teamName, ICricketSeason season, MatchType[] matchTypes)
        {
            foreach (var player in season.Players(teamName, matchTypes))
            {
                PlayerBatting.Add(player, new PlayerBattingRecord(player, teamName, season, matchTypes));
            }
        }

        /// <inheritdoc/>
        public void UpdateStats(string teamName, ICricketMatch match)
        {
        }

        /// <inheritdoc/>
        public void ResetStats()
        {
        }

        /// <inheritdoc/>
        public void ExportStats(ReportBuilder rb, DocumentElement headerElement)
        {
            if (PlayerBatting.Any())
            {
                var values = PlayerBatting.Values.ToList();
                if (_IsAllTime)
                {
                    values.Sort((a, b) => a.Name.CompareTo(b.Name));
                }
                else
                {
                    values.Sort((a, b) => b.TotalRuns.CompareTo(a.TotalRuns));
                }

                _ = values.RemoveAll(bat => bat.TotalInnings.Equals(0));
                _ = rb.WriteTitle("Overall Batting Performance", headerElement)
                    .WriteTableFromEnumerable(PlayerBattingRecord.Headers(true, !_IsAllTime, _IsAllTime), values.Select(value => value.Values(true, !_IsAllTime, _IsAllTime)), headerFirstColumn: false);
            }
        }

        public void Finalise()
        {
        }
    }
}
