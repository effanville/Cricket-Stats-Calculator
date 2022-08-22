﻿using System.Collections.Generic;
using System.Linq;

using Common.Structure.ReportWriting;

using CricketStructures.Match;
using CricketStructures.Player;
using CricketStructures.Season;
using CricketStructures.Statistics.Implementation.Player.Bowling;

namespace CricketStructures.Statistics.Implementation.Team
{
    public sealed class ClubCareerBowlingRecords : ICricketStat
    {
        private bool _IsAllTime = false;
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
            _IsAllTime = true;
            foreach (var player in team.Players())
            {
                PlayerBowling.Add(player.Name, new PlayerBowlingRecord(player.Name, team, matchTypes));
            }
        }

        public void CalculateStats(string teamName, ICricketSeason season, MatchType[] matchTypes)
        {
            foreach (var player in season.Players(teamName, matchTypes))
            {
                PlayerBowling.Add(player, new PlayerBowlingRecord(player, teamName, season, matchTypes));
            }
        }

        public void UpdateStats(string teamName, ICricketMatch match)
        {
        }

        public void ResetStats()
        {
            PlayerBowling.Clear();
        }

        public void Finalise()
        {
        }

        public void ExportStats(ReportBuilder rb, DocumentElement headerElement)
        {
            if (PlayerBowling.Any())
            {
                var values = PlayerBowling.Values.ToList();

                if (_IsAllTime)
                {
                    values.Sort((a, b) => a.Name.CompareTo(b.Name));
                }
                else
                {
                    values.Sort((a, b) => b.TotalWickets.CompareTo(a.TotalWickets));
                }
                values.RemoveAll(bat => bat.TotalOvers.Equals(0));
                _ = rb.WriteTitle("Overall Bowling Performance", headerElement)
                    .WriteTableFromEnumerable(PlayerBowlingRecord.Headers(true, !_IsAllTime, _IsAllTime), values.Select(val => val.Values(true, !_IsAllTime, _IsAllTime)), headerFirstColumn: false);
            }
        }
    }
}
