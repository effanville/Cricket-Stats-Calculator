using Cricket.Interfaces;
using Cricket.Match;
using CricketStatisticsDatabase.CricketClasses.Statistics.PlayerStats;
using ExportHelpers;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Cricket.Statistics.DetailedStats
{
    public class DetailedAllTimeBattingStatistics
    {
        public List<Century> CenturyScores
        {
            get;
            set;
        } = new List<Century>();

        public List<HighScores> ScoresPast50
        {
            get;
            set;
        } = new List<HighScores>();

        public List<CarryingOfBat> CarryingBat
        {
            get;
            set;
        } = new List<CarryingOfBat>();

        public List<SeasonRuns> SeasonRunsOver500
        {
            get;
            set;
        } = new List<SeasonRuns>();

        public List<SeasonRuns> SeasonAverageOver30
        {
            get;
            set;
        } = new List<SeasonRuns>();

        public void CalculateStats(ICricketTeam team)
        {
            foreach (var season in team.Seasons)
            {
                CalculateStats(season);
            }
        }

        public void CalculateStats(ICricketSeason season)
        {
            var seasonBriefStats = new TeamBriefStatistics(season);
            var manyRuns = seasonBriefStats.SeasonPlayerStats.Where(player => player.BattingStats.TotalRuns > 500);
            SeasonRunsOver500.AddRange(manyRuns.Select(element => new SeasonRuns() { Name = element.Name, Runs = element.BattingStats.TotalRuns, Year = element.SeasonYear.Year, Average = element.BattingStats.Average }));

            var goodAverage = seasonBriefStats.SeasonPlayerStats.Where(player => player.Played.TotalGamesPlayed > 5 && player.BattingStats.Average > 30);
            SeasonAverageOver30.AddRange(goodAverage.Select(element => new SeasonRuns() { Name = element.Name, Runs = element.BattingStats.TotalRuns, Year = element.SeasonYear.Year, Average = element.BattingStats.Average }));
            foreach (var match in season.Matches)
            {
                UpdateStats(match);
            }

            SeasonRunsOver500.Sort((a, b) => b.Runs.CompareTo(a.Runs));
            SeasonAverageOver30.Sort((a, b) => b.Average.CompareTo(a.Average));
            CenturyScores.Sort((a, b) => b.Runs.CompareTo(a.Runs));
            ScoresPast50.Sort((a, b) => b.Centuries.CompareTo(a.Centuries));
        }

        public void UpdateStats(ICricketMatch match)
        {
            foreach (var battingEntry in match.Batting.BattingInfo)
            {
                if (battingEntry.RunsScored >= 100)
                {
                    CenturyScores.Add(new Century(battingEntry, match.MatchData));
                    if (ScoresPast50.Any(entry => entry.Name.Equals(battingEntry.Name)))
                    {
                        var value = ScoresPast50.First(entry => entry.Name.Equals(battingEntry.Name));
                        value.Centuries++;
                    }
                    else
                    {
                        ScoresPast50.Add(new HighScores(battingEntry.Name, 1, 0));
                    }
                }

                if (battingEntry.RunsScored >= 50 && battingEntry.RunsScored < 100)
                {
                    if (ScoresPast50.Any(entry => entry.Name.Equals(battingEntry.Name)))
                    {
                        var value = ScoresPast50.First(entry => entry.Name.Equals(battingEntry.Name));
                        value.Fifties++;
                    }
                    else
                    {
                        ScoresPast50.Add(new HighScores(battingEntry.Name, 0, 1));
                    }
                }
            }

            if (match.BattingFirstOrSecond == TeamInnings.First || (match.BattingFirstOrSecond == TeamInnings.Second && match.Result != ResultType.Win))
            {
                var bat = match.Batting.BattingInfo[0];
                if (!bat.Out())
                {
                    CarryingBat.Add(new CarryingOfBat() { Name = bat.Name, Runs = bat.RunsScored, Date = match.MatchData.Date, Opposition = match.MatchData.Opposition, HomeOrAway = match.MatchData.HomeOrAway, TeamTotalScore = match.Batting.Score() });
                }

                bat = match.Batting.BattingInfo[1];
                if (!bat.Out())
                {
                    CarryingBat.Add(new CarryingOfBat() { Name = bat.Name, Runs = bat.RunsScored, Date = match.MatchData.Date, Opposition = match.MatchData.Opposition, HomeOrAway = match.MatchData.HomeOrAway, TeamTotalScore = match.Batting.Score() });
                }
            }
        }

        public void ExportStats(StreamWriter writer)
        {
            if (CenturyScores.Any())
            {
                writer.WriteLine("");
                writer.WriteLine("Centuries");
                writer.WriteLine(GenericHeaderWriter.TableHeader(new Century(), ","));
                foreach (var record in CenturyScores)
                {
                    writer.WriteLine(record.ToCSVLine());
                }
            }

            if (ScoresPast50.Any())
            {
                writer.WriteLine("");
                writer.WriteLine("Number Scores Past Fifty");
                writer.WriteLine(GenericHeaderWriter.TableHeader(new HighScores(), ","));
                foreach (var record in ScoresPast50)
                {
                    writer.WriteLine(record.ToCSVLine());
                }
            }

            if (CarryingBat.Any())
            {
                writer.WriteLine("");
                writer.WriteLine("Carrying of Bat");
                writer.WriteLine(GenericHeaderWriter.TableHeader(new CarryingOfBat(), ","));
                foreach (var record in CarryingBat)
                {
                    writer.WriteLine(record.ToCSVLine());
                }
            }

            if (SeasonRunsOver500.Any())
            {
                writer.WriteLine("");
                writer.WriteLine("Over 500 runs in a season");
                writer.WriteLine(GenericHeaderWriter.TableHeader(new SeasonRuns(), ","));
                foreach (var record in SeasonRunsOver500)
                {
                    writer.WriteLine(record.ToCSVLine());
                }
            }

            if (SeasonAverageOver30.Any())
            {
                writer.WriteLine("");
                writer.WriteLine("Average over 30 in a season");
                writer.WriteLine(GenericHeaderWriter.TableHeader(new SeasonRuns(), ","));
                foreach (var record in SeasonAverageOver30)
                {
                    writer.WriteLine(record.ToCSVLine());
                }
            }
        }
    }
}
