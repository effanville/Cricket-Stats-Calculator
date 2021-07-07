using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cricket.Interfaces;
using StructureCommon.FileAccess;

namespace Cricket.Statistics.DetailedStats
{
    public class HeaviestDefeats
    {
        public List<BowlingWinningMargin> HeaviestLossByRuns
        {
            get;
            set;
        } = new List<BowlingWinningMargin>();

        public List<BattingWinningMargin> HeaviestLossByWickets
        {
            get;
            set;
        } = new List<BattingWinningMargin>();

        public HeaviestDefeats()
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
                if (match.Bowling.Score().Wickets.Equals(0))
                {
                    BattingWinningMargin margin = new BattingWinningMargin(match, isTeam: false);
                    HeaviestLossByWickets.Add(margin);
                    HeaviestLossByWickets.Sort((a, b) => b.Score.CompareTo(a.Score));
                }
            }
            if (match.BattingFirstOrSecond == Match.TeamInnings.Second)
            {
                if (match.Bowling.Score().Runs > match.Batting.Score().Runs + 100)
                {
                    BowlingWinningMargin margin = new BowlingWinningMargin(match, isTeam: false);
                    HeaviestLossByRuns.Add(margin);
                    HeaviestLossByRuns.Sort((a, b) => b.WinningRuns.CompareTo(a.WinningRuns));
                }
            }
        }

        public void ExportStats(StreamWriter writer, ExportType exportType)
        {
            if (HeaviestLossByRuns.Any())
            {
                writer.WriteTitle(exportType, "Loss by 100 runs", HtmlTag.h2);
                writer.WriteTable(exportType, HeaviestLossByRuns, headerFirstColumn: false);
            }


            if (HeaviestLossByWickets.Any())
            {
                FileWritingSupport.WriteTitle(writer, exportType, "Loss by 10 wickets", HtmlTag.h2);
                FileWritingSupport.WriteTable(writer, exportType, HeaviestLossByWickets, headerFirstColumn: false);
            }
        }
    }
}