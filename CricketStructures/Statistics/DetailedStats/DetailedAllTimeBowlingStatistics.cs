using System.Collections.Generic;
using System.IO;
using System.Linq;
using CricketStructures.Interfaces;
using CricketStructures.Match.Innings;
using CricketStructures.Statistics.PlayerStats;
using StructureCommon.FileAccess;

namespace CricketStructures.Statistics.DetailedStats
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    /// Could include hat tricks and most expensive over
    /// </remarks>
    public class DetailedAllTimeBowlingStatistics
    {
        public List<BowlingPerformance> Over5Wickets
        {
            get;
            set;
        } = new List<BowlingPerformance>();

        public List<SeasonWickets> SeasonWicketsOver30
        {
            get;
            set;
        } = new List<SeasonWickets>();

        public List<SeasonWickets> SeasonAverageUnder15
        {
            get;
            set;
        } = new List<SeasonWickets>();

        public DetailedAllTimeBowlingStatistics()
        {
        }

        public List<HighWickets> NumberFiveFors
        {
            get;
            set;
        } = new List<HighWickets>();

        public List<AllTimeEconomy> LowEconomy
        {
            get;
            set;
        } = new List<AllTimeEconomy>();

        public List<AllTimeEconomy> LowStrikeRate
        {
            get;
            set;
        } = new List<AllTimeEconomy>();

        public void CalculateStats(ICricketTeam team)
        {
            TeamBriefStatistics teamStats = new TeamBriefStatistics(team);
            IEnumerable<double> economy = teamStats.SeasonPlayerStats.Select(player => player.BowlingStats.Economy);

            foreach (ICricketSeason season in team.Seasons)
            {
                CalculateStats(team.TeamName, season);
            }
        }

        public void CalculateStats(string teamName, ICricketSeason season)
        {
            TeamBriefStatistics seasonStats = new TeamBriefStatistics(teamName, season);

            IEnumerable<PlayerBriefStatistics> manyWickets = seasonStats.SeasonPlayerStats.Where(player => player.BowlingStats.TotalWickets >= 30);
            SeasonWicketsOver30.AddRange(manyWickets.Select(lots => new SeasonWickets(lots.Name, lots.BowlingStats.TotalWickets, seasonStats.SeasonYear.Year, lots.BowlingStats.Average)));

            IEnumerable<PlayerBriefStatistics> lowAverage = seasonStats.SeasonPlayerStats.Where(player => player.BowlingStats.TotalWickets > 15 && player.BowlingStats.Average < 15);
            SeasonAverageUnder15.AddRange(lowAverage.Select(lots => new SeasonWickets(lots.Name, lots.BowlingStats.TotalWickets, seasonStats.SeasonYear.Year, lots.BowlingStats.Average)));

            foreach (ICricketMatch match in season.Matches)
            {
                UpdateStats(teamName, match);
            }

            Over5Wickets.Sort((a, b) => b.Wickets.CompareTo(a.Wickets));
            SeasonWicketsOver30.Sort((a, b) => b.Wickets.CompareTo(a.Wickets));
            SeasonAverageUnder15.Sort((a, b) => a.Average.CompareTo(b.Average));
            NumberFiveFors.Sort((a, b) => b.NumberFiveFor.CompareTo(a.NumberFiveFor));
            LowEconomy.Sort((a, b) => a.Economy.CompareTo(b.Economy));
            LowStrikeRate.Sort((a, b) => a.Economy.CompareTo(b.Economy));
        }

        public void UpdateStats(string teamName, ICricketMatch match)
        {
            foreach (BowlingEntry bowlingEntry in match.GetInnings(teamName, batting: false).Bowling)
            {
                if (bowlingEntry.Wickets >= 5)
                {
                    Over5Wickets.Add(new BowlingPerformance(bowlingEntry, match.MatchData));

                    if (NumberFiveFors.Any(entry => entry.Name.Equals(bowlingEntry.Name)))
                    {
                        HighWickets value = NumberFiveFors.First(entry => entry.Name.Equals(bowlingEntry.Name));
                        value.NumberFiveFor++;
                    }
                    else
                    {
                        NumberFiveFors.Add(new HighWickets(bowlingEntry.Name, 1));
                    }
                }
            }
        }

        public void ExportStats(StreamWriter writer, ExportType exportType)
        {
            if (Over5Wickets.Any())
            {
                writer.WriteTitle(exportType, "Five Wicket Hauls", HtmlTag.h3);
                writer.WriteTable(exportType, Over5Wickets, headerFirstColumn: false);
            }

            if (SeasonWicketsOver30.Any())
            {
                writer.WriteTitle(exportType, "Over 30 Wickets in Season", HtmlTag.h3);
                writer.WriteTable(exportType, SeasonWicketsOver30, headerFirstColumn: false);
            }

            if (SeasonAverageUnder15.Any())
            {
                writer.WriteTitle(exportType, "Season Average under 15", HtmlTag.h3);
                writer.WriteTable(exportType, SeasonAverageUnder15, headerFirstColumn: false);
            }

            if (LowEconomy.Any())
            {
                writer.WriteTitle(exportType, "Season Average under 15", HtmlTag.h3);
                writer.WriteTable(exportType, LowEconomy, headerFirstColumn: false);
            }

            if (LowStrikeRate.Any())
            {
                writer.WriteTitle(exportType, "Season Average under 15", HtmlTag.h3);
                writer.WriteTable(exportType, LowStrikeRate, headerFirstColumn: false);
            }
        }
    }
}
