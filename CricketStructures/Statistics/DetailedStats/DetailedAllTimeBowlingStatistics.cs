using System.Collections.Generic;
using System.Linq;
using CricketStructures.Season;
using CricketStructures.Match;
using CricketStructures.Match.Innings;
using CricketStructures.Statistics.PlayerStats;
using Common.Structure.ReportWriting;
using System.Text;

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

        public void ExportStats(StringBuilder stringBuilder, DocumentType exportType)
        {
            if (Over5Wickets.Any())
            {
                TextWriting.WriteTitle(stringBuilder, exportType, "Five Wicket Hauls", DocumentElement.h3);
                TableWriting.WriteTable(stringBuilder, exportType, Over5Wickets, headerFirstColumn: false);
            }

            if (SeasonWicketsOver30.Any())
            {
                TextWriting.WriteTitle(stringBuilder, exportType, "Over 30 Wickets in Season", DocumentElement.h3);
                TableWriting.WriteTable(stringBuilder, exportType, SeasonWicketsOver30, headerFirstColumn: false);
            }

            if (SeasonAverageUnder15.Any())
            {
                TextWriting.WriteTitle(stringBuilder, exportType, "Season Average under 15", DocumentElement.h3);
                TableWriting.WriteTable(stringBuilder, exportType, SeasonAverageUnder15, headerFirstColumn: false);
            }

            if (LowEconomy.Any())
            {
                TextWriting.WriteTitle(stringBuilder, exportType, "Season Average under 15", DocumentElement.h3);
                TableWriting.WriteTable(stringBuilder, exportType, LowEconomy, headerFirstColumn: false);
            }

            if (LowStrikeRate.Any())
            {
                TextWriting.WriteTitle(stringBuilder, exportType, "Season Average under 15", DocumentElement.h3);
                TableWriting.WriteTable(stringBuilder, exportType, LowStrikeRate, headerFirstColumn: false);
            }
        }
    }
}
