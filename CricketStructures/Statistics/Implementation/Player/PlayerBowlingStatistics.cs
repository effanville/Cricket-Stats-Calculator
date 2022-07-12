using CricketStructures.Match;
using CricketStructures.Match.Innings;
using CricketStructures.Season;
using CricketStructures.Player;
using System;
using Common.Structure.ReportWriting;
using CricketStructures.Statistics.Implementation.Player.Model;

namespace CricketStructures.Statistics.Implementation.Player
{
    public class PlayerBowlingStatistics : ICricketStat
    {
        public PlayerName Name
        {
            get;
            set;
        }

        public double TotalOvers
        {
            get;
            set;
        }

        public int TotalMaidens
        {
            get;
            set;
        }

        public int TotalRunsConceded
        {
            get;
            set;
        }

        public int TotalWickets
        {
            get;
            set;
        }

        public double Average
        {
            get;
            set;
        }

        public double Economy
        {
            get;
            set;
        }

        public double StrikeRate
        {
            get;
            set;
        }

        public BestBowling BestFigures
        {
            get;
            set;
        }

        public PlayerBowlingStatistics()
        {
        }

        public PlayerBowlingStatistics(PlayerName name)
        {
            Name = name;
        }

        /// <inheritdoc/>
        public void CalculateStats(ICricketTeam team, MatchType[] matchTypes)
        {
            CricketStatsHelpers.SeasonIterator(
                team.Seasons,
                season => CalculateStats(team.TeamName, season, matchTypes),
                ResetStats,
                FinalCalculate);
        }

        /// <inheritdoc/>
        public void CalculateStats(string teamName, ICricketSeason season, MatchType[] matchTypes)
        {
            CricketStatsHelpers.MatchIterator(
                season,
                matchTypes,
                match => UpdateStats(teamName, match),
                postCycleAction: FinalCalculate);
        }

        /// <inheritdoc/>
        public void UpdateStats(string teamName, ICricketMatch match)
        {
            BowlingEntry bowling = match.GetBowling(teamName, Name);
            if (bowling != null)
            {
                TotalOvers += bowling.OversBowled;
                TotalMaidens += bowling.Maidens;
                TotalRunsConceded += bowling.RunsConceded;
                TotalWickets += bowling.Wickets;

                BestBowling possibleBest = new BestBowling()
                {
                    Wickets = bowling.Wickets,
                    Runs = bowling.RunsConceded,
                    Opposition = match.MatchData.OppositionName(teamName),
                    Date = match.MatchData.Date
                };

                if (possibleBest.CompareTo(BestFigures) > 0)
                {
                    BestFigures = possibleBest;
                }
            }
        }

        private void FinalCalculate()
        {
            if (TotalWickets != 0)
            {
                Average = Math.Round(TotalRunsConceded / (double)TotalWickets, 2);
                StrikeRate = Math.Round(6 * (double)TotalOvers / TotalWickets, 2);
            }
            else
            {
                Average = double.NaN;
                StrikeRate = double.NaN;
            }

            if (TotalOvers != 0)
            {
                Economy = Math.Round(TotalRunsConceded / (double)TotalOvers, 2);
            }
            else
            {
                Economy = double.NaN;
            }
        }

        /// <inheritdoc/>
        public void ResetStats()
        {
            TotalOvers = 0;
            TotalMaidens = 0;
            TotalRunsConceded = 0;
            TotalWickets = 0;
            BestFigures = new BestBowling();
            Average = double.NaN;
            StrikeRate = double.NaN;
            Economy = double.NaN;
        }

        /// <inheritdoc/>
        public void ExportStats(ReportBuilder rb, DocumentElement headerElement)
        {
            _ = rb.WriteTitle("Bowling Stats", headerElement)
                .WriteTable(new PlayerBowlingStatistics[] { this }, headerFirstColumn: false);
        }
    }
}
