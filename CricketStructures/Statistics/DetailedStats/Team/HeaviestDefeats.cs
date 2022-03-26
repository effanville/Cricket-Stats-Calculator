using System.Collections.Generic;
using System.Linq;
using CricketStructures.Match;
using CricketStructures.Season;
using Common.Structure.FileAccess;
using Common.Structure.ReportWriting;
using System.Text;

namespace CricketStructures.Statistics.DetailedStats
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
                if (match.SecondInnings.BattingScore().Wickets.Equals(0))
                {
                    BattingWinningMargin margin = new BattingWinningMargin(match.MatchData.OppositionName(teamName), match);
                    HeaviestLossByWickets.Add(margin);
                    HeaviestLossByWickets.Sort((a, b) => b.Score.CompareTo(a.Score));
                }
            }
            if (!match.BattedFirst(teamName))
            {
                if (match.FirstInnings.BowlingScore().Runs > match.SecondInnings.BattingScore().Runs + 100)
                {
                    BowlingWinningMargin margin = new BowlingWinningMargin(match.MatchData.OppositionName(teamName), match);
                    HeaviestLossByRuns.Add(margin);
                    HeaviestLossByRuns.Sort((a, b) => b.WinningRuns.CompareTo(a.WinningRuns));
                }
            }
        }

        public void ExportStats(StringBuilder writer, ExportType exportType)
        {
            if (HeaviestLossByRuns.Any())
            {
                TextWriting.WriteTitle(writer, exportType, "Loss by 100 runs", HtmlTag.h2);
                TableWriting.WriteTable(writer, exportType, HeaviestLossByRuns, headerFirstColumn: false);
            }


            if (HeaviestLossByWickets.Any())
            {
                TextWriting.WriteTitle(writer, exportType, "Loss by 10 wickets", HtmlTag.h2);
                TableWriting.WriteTable(writer, exportType, HeaviestLossByWickets, headerFirstColumn: false);
            }
        }
    }
}