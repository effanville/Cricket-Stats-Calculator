using System.Collections.Generic;
using System.IO;
using System.Linq;
using CricketStructures.Interfaces;
using CricketStructures.Match;
using CricketStructures.Match.Innings;
using CricketStructures.Statistics.PlayerStats;
using StructureCommon.FileAccess;

namespace CricketStructures.Statistics.DetailedStats
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
            foreach (ICricketSeason season in team.Seasons)
            {
                CalculateStats(team.TeamName, season);
            }
        }

        public void CalculateStats(string teamName, ICricketSeason season)
        {
            TeamBriefStatistics seasonBriefStats = new TeamBriefStatistics(teamName, season);
            IEnumerable<PlayerBriefStatistics> manyRuns = seasonBriefStats.SeasonPlayerStats.Where(player => player.BattingStats.TotalRuns > 500);
            SeasonRunsOver500.AddRange(manyRuns.Select(element => new SeasonRuns() { Name = element.Name, Runs = element.BattingStats.TotalRuns, Year = element.SeasonYear.Year, Average = element.BattingStats.Average }));

            IEnumerable<PlayerBriefStatistics> goodAverage = seasonBriefStats.SeasonPlayerStats.Where(player => player.Played.TotalGamesPlayed > 5 && player.BattingStats.Average > 30);
            SeasonAverageOver30.AddRange(goodAverage.Select(element => new SeasonRuns() { Name = element.Name, Runs = element.BattingStats.TotalRuns, Year = element.SeasonYear.Year, Average = element.BattingStats.Average }));
            foreach (ICricketMatch match in season.Matches)
            {
                UpdateStats(teamName, match);
            }

            SeasonRunsOver500.Sort((a, b) => b.Runs.CompareTo(a.Runs));
            SeasonAverageOver30.Sort((a, b) => b.Average.CompareTo(a.Average));
            CenturyScores.Sort((a, b) => b.Runs.CompareTo(a.Runs));
            ScoresPast50.Sort((a, b) => b.Centuries.CompareTo(a.Centuries));
        }

        public void UpdateStats(string teamName, ICricketMatch match)
        {
            foreach (BattingEntry battingEntry in match.GetInnings(teamName, batting: true).Batting)
            {
                if (battingEntry.RunsScored >= 100)
                {
                    CenturyScores.Add(new Century(teamName, battingEntry, match.MatchData));
                    if (ScoresPast50.Any(entry => entry.Name.Equals(battingEntry.Name)))
                    {
                        HighScores value = ScoresPast50.First(entry => entry.Name.Equals(battingEntry.Name));
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
                        HighScores value = ScoresPast50.First(entry => entry.Name.Equals(battingEntry.Name));
                        value.Fifties++;
                    }
                    else
                    {
                        ScoresPast50.Add(new HighScores(battingEntry.Name, 0, 1));
                    }
                }
            }

            bool battedFirst = match.BattedFirst(teamName);
            if (battedFirst || (!battedFirst && match.Result != ResultType.Win))
            {
                var innings = match.GetInnings(teamName, batting: true);
                BattingEntry bat = innings.Batting[0];
                if (!bat.Out())
                {
                    CarryingBat.Add(new CarryingOfBat() { Name = bat.Name, Runs = bat.RunsScored, Date = match.MatchData.Date, Opposition = match.MatchData.OppositionName(teamName), Location = match.MatchData.Location, TeamTotalScore = innings.BattingScore() });
                }

                bat = innings.Batting[1];
                if (!bat.Out())
                {
                    CarryingBat.Add(new CarryingOfBat() { Name = bat.Name, Runs = bat.RunsScored, Date = match.MatchData.Date, Opposition = match.MatchData.OppositionName(teamName), Location = match.MatchData.Location, TeamTotalScore = innings.BattingScore() });
                }
            }
        }

        public void ExportStats(StreamWriter writer, ExportType exportType)
        {
            if (CenturyScores.Any())
            {
                FileWritingSupport.WriteTitle(writer, exportType, "Centuries", HtmlTag.h3);
                FileWritingSupport.WriteTable(writer, exportType, CenturyScores, headerFirstColumn: false);
            }

            if (ScoresPast50.Any())
            {
                FileWritingSupport.WriteTitle(writer, exportType, "Number Scores Past Fifty", HtmlTag.h3);
                FileWritingSupport.WriteTable(writer, exportType, ScoresPast50, headerFirstColumn: false);
            }

            if (CarryingBat.Any())
            {
                FileWritingSupport.WriteTitle(writer, exportType, "Carrying of Bat", HtmlTag.h3);
                FileWritingSupport.WriteTable(writer, exportType, CarryingBat, headerFirstColumn: false);
            }

            if (SeasonRunsOver500.Any())
            {
                FileWritingSupport.WriteTitle(writer, exportType, "Over 500 runs in a season", HtmlTag.h3);
                FileWritingSupport.WriteTable(writer, exportType, SeasonRunsOver500, headerFirstColumn: false);
            }

            if (SeasonAverageOver30.Any())
            {
                FileWritingSupport.WriteTitle(writer, exportType, "Average over 30 in a season", HtmlTag.h3);
                FileWritingSupport.WriteTable(writer, exportType, SeasonAverageOver30, headerFirstColumn: false);
            }
        }
    }
}
