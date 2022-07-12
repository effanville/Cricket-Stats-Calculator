using CricketStructures.Match;
using CricketStructures.Match.Innings;
using CricketStructures.Season;
using CricketStructures.Player;
using System;
using Common.Structure.ReportWriting;
using System.Collections.Generic;
using CricketStructures.Statistics.Implementation.Player.Model;

namespace CricketStructures.Statistics.Implementation.Player.Batting
{
    public class PlayerBattingStatistics : ICricketStat
    {
        public PlayerName Name
        {
            get;
            set;
        }

        public int TotalInnings
        {
            get;
            set;
        }

        public int TotalNotOut
        {
            get;
            set;
        }

        public int TotalRuns
        {
            get;
            set;
        }

        public double Average
        {
            get;
            set;
        }

        public double RunsPerInnings
        {
            get;
            set;
        }

        private List<int> WicketLossNumbers
        {
            get;
            set;
        } = new List<int>(new int[Enum.GetValues(typeof(Wicket)).Length]);

        public BestBatting Best
        {
            get;
            set;
        }

        public PlayerBattingStatistics()
        {
        }

        public PlayerBattingStatistics(PlayerName name)
        {
            Name = name;
        }

        public void CalculateStats(ICricketTeam team, MatchType[] matchTypes)
        {
            CricketStatsHelpers.SeasonIterator(
                team.Seasons,
                season => CalculateStats(team.TeamName, season, matchTypes),
                ResetStats,
                Finalise);
        }

        private void Finalise()
        {
            if (TotalInnings != TotalNotOut)
            {
                Average = Math.Round(TotalRuns / (TotalInnings - (double)TotalNotOut), 2);
            }

            RunsPerInnings = Math.Round((double)TotalRuns / TotalInnings, 2);
        }

        public void CalculateStats(string teamName, ICricketSeason season, MatchType[] matchTypes)
        {
            CricketStatsHelpers.MatchIterator(
                season,
                matchTypes,
                match => UpdateStats(teamName, match),
                postCycleAction: Finalise);
        }

        public void UpdateStats(string teamName, ICricketMatch match)
        {
            BattingEntry batting = match.GetBatting(teamName, Name);
            if (batting != null)
            {
                if (batting.MethodOut != Wicket.DidNotBat)
                {
                    TotalInnings++;
                    if (!batting.Out())
                    {
                        TotalNotOut++;
                    }
                    int index = (int)batting.MethodOut;
                    WicketLossNumbers[index] += 1;
                    TotalRuns += batting.RunsScored;

                    BestBatting possibleBest = new BestBatting()
                    {
                        Runs = batting.RunsScored,
                        HowOut = batting.MethodOut,
                        Opposition = match.MatchData.OppositionName(teamName),
                        Date = match.MatchData.Date
                    };

                    if (possibleBest.CompareTo(Best) > 0)
                    {
                        Best = possibleBest;
                    }
                }
            }
        }

        public void ResetStats()
        {
            TotalInnings = 0;
            TotalNotOut = 0;
            TotalRuns = 0;
            Best = new BestBatting();
        }

        public void ExportStats(ReportBuilder rb, DocumentElement headerElement)
        {
            _ = rb.WriteTitle("Batting Stats", headerElement)
                .WriteTable(new PlayerBattingStatistics[] { this }, headerFirstColumn: false);
        }
    }
}
