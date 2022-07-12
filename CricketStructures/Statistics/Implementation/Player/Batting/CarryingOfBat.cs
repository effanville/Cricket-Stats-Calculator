using System.Collections.Generic;
using System.Linq;

using Common.Structure.ReportWriting;

using CricketStructures.Match;
using CricketStructures.Match.Innings;
using CricketStructures.Player;
using CricketStructures.Season;
using CricketStructures.Statistics.Implementation.Player.Model;

namespace CricketStructures.Statistics.Implementation.Player.Batting
{
    internal sealed class CarryingOfBat : ICricketStat
    {
        private readonly PlayerName Name;
        public List<PlayerScore> CarryingBat
        {
            get;
            set;
        } = new List<PlayerScore>();

        public CarryingOfBat()
        {
        }

        public CarryingOfBat(PlayerName name)
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
                match => UpdateStats(teamName, match));
        }

        public void ResetStats()
        {
            CarryingBat.Clear();
        }

        public void UpdateStats(string teamName, ICricketMatch match)
        {
            var innings = match.GetInnings(teamName, batting: true);
            var battingInnings = innings?.Batting;
            if (battingInnings == null)
            {
                return;
            }

            if (Name != null && !(battingInnings[0].Name.Equals(Name) || battingInnings[1].Name.Equals(Name)))
            {
                return;
            }

            bool battedFirst = match.BattedFirst(teamName);
            if (battedFirst || !battedFirst && match.Result != ResultType.Win)
            {
                if (battingInnings.Any())
                {
                    BattingEntry bat = battingInnings[0];
                    if (!bat.Out())
                    {
                        CarryingBat.Add(new PlayerScore() { Name = bat.Name, Runs = bat.RunsScored, Date = match.MatchData.Date, Opposition = match.MatchData.OppositionName(teamName), Location = match.MatchData.Location, TeamTotalScore = innings.BattingScore() });
                    }

                    bat = battingInnings[1];
                    if (!bat.Out())
                    {
                        CarryingBat.Add(new PlayerScore() { Name = bat.Name, Runs = bat.RunsScored, Date = match.MatchData.Date, Opposition = match.MatchData.OppositionName(teamName), Location = match.MatchData.Location, TeamTotalScore = innings.BattingScore() });
                    }
                }
            }
        }

        public void ExportStats(ReportBuilder rb, DocumentElement headerElement)
        {
            if (CarryingBat.Any())
            {
                _ = rb.WriteTitle("Carrying of Bat", headerElement)
                    .WriteTable(CarryingBat, headerFirstColumn: false);
            }
        }
    }
}
