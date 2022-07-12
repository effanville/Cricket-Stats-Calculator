using System.Collections.Generic;
using System.Linq;

using Common.Structure.ReportWriting;

using CricketStructures.Match;
using CricketStructures.Match.Innings;
using CricketStructures.Season;
using CricketStructures.Statistics.Implementation.Player.Model;

namespace CricketStructures.Statistics.Implementation.Player.Career
{
    internal sealed class HighestClubBattingAverage : ICricketStat
    {
        public List<BattingAverageList> ClubBattingAverage
        {
            get;
            set;
        } = new List<BattingAverageList>();

        public void CalculateStats(ICricketTeam team, MatchType[] matchTypes)
        {
            CricketStatsHelpers.SeasonIterator(
                team.Seasons,
                season => CalculateStats(team.TeamName, season, matchTypes),
                postCycleAction: () => ClubBattingAverage.Sort((a, b) => b.Average.CompareTo(a.Average)));
        }

        public void CalculateStats(string teamName, ICricketSeason season, MatchType[] matchTypes)
        {
            CricketStatsHelpers.MatchIterator(
                season,
                matchTypes,
                match => UpdateStats(teamName, match));
        }

        public void UpdateStats(string teamName, ICricketMatch match)
        {
            CricketStatsHelpers.BattingIterator(
                match,
                teamName,
                entry => CalculateAverage(entry));
            void CalculateAverage(BattingEntry batting)
            {
                var playerRuns = ClubBattingAverage.FirstOrDefault(run => run.Name.Equals(batting.Name));
                if (playerRuns != null)
                {
                    if (batting.MethodOut != Wicket.DidNotBat)
                    {
                        playerRuns.UpdateValues(match.MatchData.Date, batting.RunsScored, batting.MethodOut == Wicket.NotOut);
                    }
                }
                else
                {
                    var newEntry = new BattingAverageList(
                        batting.Name,
                        match.MatchData.Date,
                        batting.RunsScored,
                        batting.MethodOut == Wicket.NotOut);
                    ClubBattingAverage.Add(newEntry);
                }
            }
        }

        public void ResetStats()
        {
            ClubBattingAverage.Clear();
        }

        public void ExportStats(ReportBuilder rb, DocumentElement headerElement)
        {
            _ = rb.WriteTitle("Batting Average", headerElement)
                .WriteTable(ClubBattingAverage, headerFirstColumn: false);
        }
    }
}
