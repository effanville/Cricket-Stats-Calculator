using Cricket.Interfaces;
using Cricket.Player;
using Cricket.Statistics.PlayerStats;
using ExportHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Cricket.Statistics.DetailedStats
{
    public class DetailedAllTimeCareerStatistics
    {
        public List<CareerBattingRecord> PlayerBatting
        {
            get;
            set;
        } = new List<CareerBattingRecord>();

        public List<(PlayerName Name, DateTime startYear, DateTime endYear, int Appearances)> MostClubAppearances
        {
            get;
            set;
        } = new List<(PlayerName, DateTime, DateTime, int)>();

        public List<(PlayerName Name, DateTime startYear, DateTime endYear, int Runs)> MostClubRuns
        {
            get;
            set;
        } = new List<(PlayerName, DateTime, DateTime, int)>();

        public List<(PlayerName Name, DateTime startYear, DateTime endYear, double Average)> HighestClubBattingAverage
        {
            get;
            set;
        } = new List<(PlayerName, DateTime, DateTime, double)>();

        public List<(PlayerName Name, DateTime startYear, DateTime endYear, int Wickets)> MostClubWickets
        {
            get;
            set;
        } = new List<(PlayerName, DateTime, DateTime, int)>();

        public List<CareerBowlingRecord> PlayerBowling
        {
            get;
            set;
        } = new List<CareerBowlingRecord>();

        public void CalculateStats(ICricketTeam team)
        {
            foreach (var player in team.Players)
            {
                PlayerBatting.Add(new CareerBattingRecord(player.Name, team));
                PlayerBowling.Add(new CareerBowlingRecord(player.Name, team));
            }

            PlayerBatting.Sort((a, b) => b.MatchesPlayed.CompareTo(a.MatchesPlayed));
            MostClubAppearances = PlayerBatting.Take(5).Select(batting => (batting.Name, batting.StartYear, batting.EndYear, batting.MatchesPlayed)).ToList();

            PlayerBatting.Sort((a, b) => b.Runs.CompareTo(a.Runs));
            MostClubRuns = PlayerBatting.Take(5).Select(batting => (batting.Name, batting.StartYear, batting.EndYear, batting.Runs)).ToList();

            PlayerBatting.Sort((a, b) => b.Average.CompareTo(a.Average));
            HighestClubBattingAverage = PlayerBatting.Take(5).Select(batting => (batting.Name, batting.StartYear, batting.EndYear, batting.Average)).ToList();

            PlayerBowling.Sort((a, b) => b.Wickets.CompareTo(a.Wickets));
            MostClubWickets = PlayerBowling.Take(5).Select(bowling => (bowling.Name, bowling.StartYear, bowling.EndYear, bowling.Wickets)).ToList();

            PlayerBatting.Sort((a, b) => a.Name.CompareTo(b.Name));
            PlayerBowling.Sort((a, b) => a.Name.CompareTo(b.Name));
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
        }

        public void ExportStats(StreamWriter writer)
        {
            writer.WriteLine("");
            writer.WriteLine("Leading Career Records");

            writer.WriteLine("");
            writer.WriteLine("Appearances");
            writer.WriteLine("Name,StartYear,EndYear,Appearances");
            foreach (var record in MostClubAppearances)
            {
                writer.WriteLine(record.Name + "," + record.startYear.Year + "," + record.endYear.Year + "," + record.Appearances);
            }

            writer.WriteLine("");
            writer.WriteLine("Runs");
            writer.WriteLine("Name,StartYear,EndYear,Runs");
            foreach (var record in MostClubRuns)
            {
                writer.WriteLine(record.Name + "," + record.startYear.Year + "," + record.endYear.Year + "," + record.Runs);
            }

            writer.WriteLine("");
            writer.WriteLine("Batting Average");
            writer.WriteLine("Name,StartYear,EndYear,Average");
            foreach (var record in HighestClubBattingAverage)
            {
                writer.WriteLine(record.Name + "," + record.startYear.Year + "," + record.endYear.Year + "," + record.Average);
            }

            writer.WriteLine("");
            writer.WriteLine("Wickets Taken");
            writer.WriteLine("Name,StartYear,EndYear,Wickets");
            foreach (var record in MostClubWickets)
            {
                writer.WriteLine(record.Name + "," + record.startYear.Year + "," + record.endYear.Year + "," + record.Wickets);
            }

            if (PlayerBatting.Any())
            {
                writer.WriteLine("");
                writer.WriteLine("Overall Batting Performance");
                writer.WriteLine(GenericHeaderWriter.TableHeader(new CareerBattingRecord(), ","));
                foreach (var record in PlayerBatting)
                {
                    writer.WriteLine(record.ToCSVLine());
                }
            }

            if (PlayerBowling.Any())
            {
                writer.WriteLine("");
                writer.WriteLine("Overall Bowling Performance");
                writer.WriteLine(GenericHeaderWriter.TableHeader(new CareerBowlingRecord(), ","));
                foreach (var record in PlayerBowling)
                {
                    writer.WriteLine(record.ToCSVLine());
                }
            }
        }
    }
}
