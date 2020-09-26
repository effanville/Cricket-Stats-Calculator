using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cricket.Interfaces;
using StructureCommon.FileAccess;

namespace Cricket.Statistics.DetailedStats
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
                CalculateStats(season);
            }
        }

        public void CalculateStats(ICricketSeason season)
        {
            foreach (ICricketMatch match in season.Matches)
            {
                UpdateStats(match);
            }
        }

        public void UpdateStats(ICricketMatch match)
        {
            if (match.BattingFirstOrSecond == Match.TeamInnings.First)
            {
                if (match.Batting.Score().Runs > match.Bowling.Score().Runs + 100)
                {
                    BowlingWinningMargin margin = new BowlingWinningMargin(match);
                    WinBy100Runs.Add(margin);
                    WinBy100Runs.Sort((a, b) => b.WinningRuns.CompareTo(a.WinningRuns));
                }
            }
            if (match.BattingFirstOrSecond == Match.TeamInnings.Second)
            {
                if (match.Batting.Score().Wickets.Equals(0))
                {
                    BattingWinningMargin margin = new BattingWinningMargin(match);
                    WinBy10Wickets.Add(margin);
                    WinBy10Wickets.Sort((a, b) => b.Score.CompareTo(a.Score));
                }
            }
        }

        public void ExportStats(StreamWriter writer, ExportType exportType)
        {
            if (WinBy100Runs.Any())
            {
                FileWritingSupport.WriteTitle(writer, exportType, "Wins by 100 Runs", HtmlTag.h2);
                FileWritingSupport.WriteTable(writer, exportType, WinBy100Runs, headerFirstColumn: false);
            }

            if (WinBy10Wickets.Any())
            {
                FileWritingSupport.WriteTitle(writer, exportType, "Wins by 10 wickets", HtmlTag.h2);
                FileWritingSupport.WriteTable(writer, exportType, WinBy10Wickets, headerFirstColumn: false);
            }
        }
    }
}
