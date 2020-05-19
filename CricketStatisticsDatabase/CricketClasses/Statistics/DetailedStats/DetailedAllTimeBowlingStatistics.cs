using Cricket.Interfaces;
using Cricket.Statistics.PlayerStats;
using ExportHelpers;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Cricket.Statistics.DetailedStats
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
            var teamStats = new TeamBriefStatistics(team);
            var economy = teamStats.SeasonPlayerStats.Select(player => player.BowlingStats.Economy);

            foreach (var season in team.Seasons)
            {
                CalculateStats(season);
            }
        }

        public void CalculateStats(ICricketSeason season)
        {
            var seasonStats = new TeamBriefStatistics(season);

            var manyWickets = seasonStats.SeasonPlayerStats.Where(player => player.BowlingStats.TotalWickets >= 30);
            SeasonWicketsOver30.AddRange(manyWickets.Select(lots => new SeasonWickets(lots.Name, lots.BowlingStats.TotalWickets, seasonStats.SeasonYear.Year, lots.BowlingStats.Average)));

            var lowAverage = seasonStats.SeasonPlayerStats.Where(player => player.BowlingStats.TotalWickets > 15 && player.BowlingStats.Average < 15);
            SeasonAverageUnder15.AddRange(lowAverage.Select(lots => new SeasonWickets(lots.Name, lots.BowlingStats.TotalWickets, seasonStats.SeasonYear.Year, lots.BowlingStats.Average)));

            foreach (var match in season.Matches)
            {
                UpdateStats(match);
            }

            Over5Wickets.Sort((a, b) => b.Wickets.CompareTo(a.Wickets));
            SeasonWicketsOver30.Sort((a, b) => b.Wickets.CompareTo(a.Wickets));
            SeasonAverageUnder15.Sort((a, b) => a.Average.CompareTo(b.Average));
            NumberFiveFors.Sort((a, b) => b.NumberFiveFor.CompareTo(a.NumberFiveFor));
            LowEconomy.Sort((a, b) => a.Economy.CompareTo(b.Economy));
            LowStrikeRate.Sort((a, b) => a.Economy.CompareTo(b.Economy));
        }

        public void UpdateStats(ICricketMatch match)
        {
            foreach (var bowlingEntry in match.Bowling.BowlingInfo)
            {
                if (bowlingEntry.Wickets >= 5)
                {
                    Over5Wickets.Add(new BowlingPerformance(bowlingEntry, match.MatchData));

                    if (NumberFiveFors.Any(entry => entry.Name.Equals(bowlingEntry.Name)))
                    {
                        var value = NumberFiveFors.First(entry => entry.Name.Equals(bowlingEntry.Name));
                        value.NumberFiveFor++;
                    }
                    else
                    {
                        NumberFiveFors.Add(new HighWickets(bowlingEntry.Name, 1));
                    }
                }
            }
        }

        public void ExportStats(StreamWriter writer)
        {
            if (Over5Wickets.Any())
            {
                writer.WriteLine("");
                writer.WriteLine("Five Wicket Hauls");
                writer.WriteLine(GenericHeaderWriter.TableHeader(new BowlingPerformance(), ","));
                foreach (var record in Over5Wickets)
                {
                    writer.WriteLine(record.ToCSVLine());
                }
            }

            if (SeasonWicketsOver30.Any())
            {
                writer.WriteLine("");
                writer.WriteLine("Over 30 Wickets in Season");
                writer.WriteLine(GenericHeaderWriter.TableHeader(new SeasonWickets(), ","));
                foreach (var record in SeasonWicketsOver30)
                {
                    writer.WriteLine(record.ToCSVLine());
                }
            }

            if (SeasonAverageUnder15.Any())
            {
                writer.WriteLine("");
                writer.WriteLine("Season Average under 15");
                writer.WriteLine(GenericHeaderWriter.TableHeader(new SeasonWickets(), ","));
                foreach (var record in SeasonAverageUnder15)
                {
                    writer.WriteLine(record.ToCSVLine());
                }
            }

            if (LowEconomy.Any())
            {
                writer.WriteLine("");
                writer.WriteLine("Season Average under 15");
                writer.WriteLine(GenericHeaderWriter.TableHeader(new AllTimeEconomy(), ","));
                foreach (var record in LowEconomy)
                {
                    writer.WriteLine(record.ToCSVLine());
                }
            }

            if (LowStrikeRate.Any())
            {
                writer.WriteLine("");
                writer.WriteLine("Season Average under 15");
                writer.WriteLine(GenericHeaderWriter.TableHeader(new AllTimeEconomy(), ","));
                foreach (var record in LowStrikeRate)
                {
                    writer.WriteLine(record.ToCSVLine());
                }
            }
        }
    }
}
