using Cricket.Interfaces;
using ExportHelpers;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
                if (match.Bowling.Score().Wickets.Equals(0))
                {
                    var margin = new BattingWinningMargin(match, isTeam: false);
                    HeaviestLossByWickets.Add(margin);
                    HeaviestLossByWickets.Sort((a, b) => b.Score.CompareTo(a.Score));
                }
            }
            if (match.BattingFirstOrSecond == Match.TeamInnings.Second)
            {
                if (match.Bowling.Score().Runs > match.Batting.Score().Runs + 100)
                {
                    var margin = new BowlingWinningMargin(match, isTeam: false);
                    HeaviestLossByRuns.Add(margin);
                    HeaviestLossByRuns.Sort((a, b) => b.WinningRuns.CompareTo(a.WinningRuns));
                }
            }
        }

        public void ExportStats(StreamWriter writer)
        {
            if (HeaviestLossByRuns.Any())
            {
                writer.WriteLine("");
                writer.WriteLine("Loss by 100 runs");
                writer.WriteLine("");

                writer.WriteLine(GenericHeaderWriter.TableHeader(new BowlingWinningMargin(), ","));
                foreach (var record in HeaviestLossByRuns)
                {
                    writer.WriteLine(record.ToCSVLine());
                }
            }


            if (HeaviestLossByWickets.Any())
            {
                writer.WriteLine("");
                writer.WriteLine("Loss by 10 wickets");
                writer.WriteLine("");

                writer.WriteLine(GenericHeaderWriter.TableHeader(new BattingWinningMargin(), ","));
                foreach (var record in HeaviestLossByWickets)
                {
                    writer.WriteLine(record.ToCSVLine());
                }
            }
        }
    }
}