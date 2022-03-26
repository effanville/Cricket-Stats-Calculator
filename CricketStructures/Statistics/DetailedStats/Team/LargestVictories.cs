using System.Collections.Generic;
using System.Linq;
using CricketStructures.Match;
using CricketStructures.Season;
using Common.Structure.FileAccess;
using Common.Structure.ReportWriting;
using System.Text;

namespace CricketStructures.Statistics.DetailedStats
{
    public class LargestVictories
    {
        public List<BowlingWinningMargin> WinBy100Runs
        {
            get;
            set;
        } = new List<BowlingWinningMargin>();

        public List<BattingWinningMargin> WinBy10Wickets
        {
            get;
            set;
        } = new List<BattingWinningMargin>();

        public LargestVictories()
        {
        }

        public void CalculateStats(ICricketTeam team)
        {
            foreach (ICricketSeason season in team.Seasons)
            {
                CalculateStats(team.TeamName, season);
            }
        }

        public void CalculateStats(string teamName, ICricketSeason season)
        {
            foreach (ICricketMatch match in season.Matches)
            {
                UpdateStats(teamName, match);
            }
        }

        public void UpdateStats(string teamName, ICricketMatch match)
        {
            if (match.BattedFirst(teamName))
            {
                if (match.FirstInnings.BattingScore().Runs > match.SecondInnings.BattingScore().Runs + 100)
                {
                    BowlingWinningMargin margin = new BowlingWinningMargin(teamName, match);
                    WinBy100Runs.Add(margin);
                    WinBy100Runs.Sort((a, b) => b.WinningRuns.CompareTo(a.WinningRuns));
                }
            }

            if (!match.BattedFirst(teamName))
            {
                if (match.SecondInnings.BattingScore().Wickets.Equals(0))
                {
                    BattingWinningMargin margin = new BattingWinningMargin(teamName, match);
                    WinBy10Wickets.Add(margin);
                    WinBy10Wickets.Sort((a, b) => b.Score.CompareTo(a.Score));
                }
            }
        }

        public void ExportStats(StringBuilder writer, ExportType exportType)
        {
            if (WinBy100Runs.Any())
            {
                TextWriting.WriteTitle(writer, exportType, "Wins by 100 Runs", HtmlTag.h2);
                TableWriting.WriteTable(writer, exportType, WinBy100Runs, headerFirstColumn: false);
            }

            if (WinBy10Wickets.Any())
            {
                TextWriting.WriteTitle(writer, exportType, "Wins by 10 wickets", HtmlTag.h2);
                TableWriting.WriteTable(writer, exportType, WinBy10Wickets, headerFirstColumn: false);
            }
        }
    }
}
