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

        public void ExportStats(StreamWriter writer, ExportType exportType)
        {
            if (CenturyScores.Any())
            {
                FileWritingSupport.WriteTitle(writer, exportType, "Centuries", HtmlTag.h3);
                FileWritingSupport.WriteTable(writer, exportType, new Century().GetType().GetProperties().Select(type => type.Name), CenturyScores);
            }

            if (ScoresPast50.Any())
            {
                FileWritingSupport.WriteTitle(writer, exportType, "Number Scores Past Fifty", HtmlTag.h3);
                FileWritingSupport.WriteTable(writer, exportType, new HighScores().GetType().GetProperties().Select(type => type.Name), ScoresPast50);
            }

            if (CarryingBat.Any())
            {
                FileWritingSupport.WriteTitle(writer, exportType, "Carrying of Bat", HtmlTag.h3);
                FileWritingSupport.WriteTable(writer, exportType, new CarryingOfBat().GetType().GetProperties().Select(type => type.Name), CarryingBat);
            }

            if (SeasonRunsOver500.Any())
            {
                FileWritingSupport.WriteTitle(writer, exportType, "Over 500 runs in a season", HtmlTag.h3);
                FileWritingSupport.WriteTable(writer, exportType, new SeasonRuns().GetType().GetProperties().Select(type => type.Name), SeasonRunsOver500);
            }

            if (SeasonAverageOver30.Any())
            {
                FileWritingSupport.WriteTitle(writer, exportType, "Average over 30 in a season", HtmlTag.h3);
                FileWritingSupport.WriteTable(writer, exportType, new SeasonRuns().GetType().GetProperties().Select(type => type.Name), SeasonAverageOver30);
            }
        }
    }
}
