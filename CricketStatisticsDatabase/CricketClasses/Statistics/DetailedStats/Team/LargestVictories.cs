using Cricket.Interfaces;
using ExportHelpers;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
            foreach (var season in team.Seasons)
            {
                CalculateStats(season);
            }
        }

        public void CalculateStats(ICricketSeason season)
        {
            foreach (var match in season.Matches)
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
                    var margin = new BowlingWinningMargin(match);
                    WinBy100Runs.Add(margin);
                    WinBy100Runs.Sort((a, b) => b.WinningRuns.CompareTo(a.WinningRuns));
                }
            }
            if (match.BattingFirstOrSecond == Match.TeamInnings.Second)
            {
                if (match.Batting.Score().Wickets.Equals(0))
                {
                    var margin = new BattingWinningMargin(match);
                    WinBy10Wickets.Add(margin);
                    WinBy10Wickets.Sort((a, b) => b.Score.CompareTo(a.Score));
                }
            }
        }

        public void ExportStats(StreamWriter writer)
        {
            if (WinBy100Runs.Any())
            {
                writer.WriteLine("");
                writer.WriteLine("Wins by 100 runs");
                writer.WriteLine("");

                writer.WriteLine(GenericHeaderWriter.TableHeader(new BowlingWinningMargin(), ","));
                foreach (var record in WinBy100Runs)
                {
                    writer.WriteLine(record.ToCSVLine());
                }
            }


            if (WinBy10Wickets.Any())
            {
                writer.WriteLine("");
                writer.WriteLine("Wins by 10 wickets");
                writer.WriteLine("");

                writer.WriteLine(GenericHeaderWriter.TableHeader(new BattingWinningMargin(), ","));
                foreach (var record in WinBy10Wickets)
                {
                    writer.WriteLine(record.ToCSVLine());
                }
            }
        }
    }
}
