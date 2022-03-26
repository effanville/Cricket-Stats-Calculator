using System.Collections.Generic;
using System.Linq;
using CricketStructures.Player;
using Common.Structure.FileAccess;
using CricketStructures.Player.Interfaces;
using CricketStructures.Statistics.PlayerStats;
using Common.Structure.ReportWriting;
using System.Text;

namespace CricketStructures.Statistics.DetailedStats
{
    public class AppearanceList
    {
        public PlayerName Name
        {
            get;
            set;
        }
        public int StartYear
        {
            get;
            set;
        }
        public int EndYear
        {
            get;
            set;
        }
        public int Appearances
        {
            get;
            set;
        }
    }

    public class RunsList
    {
        public PlayerName Name
        {
            get;
            set;
        }
        public int StartYear
        {
            get;
            set;
        }
        public int EndYear
        {
            get;
            set;
        }
        public int Runs
        {
            get;
            set;
        }
    }

    public class BattingAverageList
    {
        public PlayerName Name
        {
            get;
            set;
        }
        public int StartYear
        {
            get;
            set;
        }
        public int EndYear
        {
            get;
            set;
        }
        public double Average
        {
            get;
            set;
        }
    }

    public class WicketsList
    {
        public PlayerName Name
        {
            get;
            set;
        }
        public int StartYear
        {
            get;
            set;
        }
        public int EndYear
        {
            get;
            set;
        }
        public int Wickets
        {
            get;
            set;
        }
    }

    public class DetailedAllTimeCareerStatistics
    {
        public List<CareerBattingRecord> PlayerBatting
        {
            get;
            set;
        } = new List<CareerBattingRecord>();

        public List<AppearanceList> MostClubAppearances
        {
            get;
            set;
        } = new List<AppearanceList>();

        public List<RunsList> MostClubRuns
        {
            get;
            set;
        } = new List<RunsList>();

        public List<BattingAverageList> HighestClubBattingAverage
        {
            get;
            set;
        } = new List<BattingAverageList>();

        public List<WicketsList> MostClubWickets
        {
            get;
            set;
        } = new List<WicketsList>();

        public List<CareerBowlingRecord> PlayerBowling
        {
            get;
            set;
        } = new List<CareerBowlingRecord>();

        public void CalculateStats(ICricketTeam team)
        {
            foreach (ICricketPlayer player in team.Players)
            {
                PlayerBatting.Add(new CareerBattingRecord(player.Name, team));
                PlayerBowling.Add(new CareerBowlingRecord(player.Name, team));
            }

            PlayerBatting.Sort((a, b) => b.MatchesPlayed.CompareTo(a.MatchesPlayed));
            MostClubAppearances = PlayerBatting.Take(5).Select(batting => new AppearanceList() { Name = batting.Name, StartYear = batting.StartYear, EndYear = batting.EndYear, Appearances = batting.MatchesPlayed }).ToList();

            PlayerBatting.Sort((a, b) => b.Runs.CompareTo(a.Runs));
            MostClubRuns = PlayerBatting.Take(5).Select(batting => new RunsList() { Name = batting.Name, StartYear = batting.StartYear, EndYear = batting.EndYear, Runs = batting.Runs }).ToList();

            PlayerBatting.Sort((a, b) => b.Average.CompareTo(a.Average));
            HighestClubBattingAverage = PlayerBatting.Take(5).Select(batting => new BattingAverageList() { Name = batting.Name, StartYear = batting.StartYear, EndYear = batting.EndYear, Average = batting.Average }).ToList();

            PlayerBowling.Sort((a, b) => b.Wickets.CompareTo(a.Wickets));
            MostClubWickets = PlayerBowling.Take(5).Select(bowling => new WicketsList() { Name = bowling.Name, StartYear = bowling.StartYear, EndYear = bowling.EndYear, Wickets = bowling.Wickets }).ToList();

            PlayerBatting.Sort((a, b) => a.Name.CompareTo(b.Name));
            PlayerBowling.Sort((a, b) => a.Name.CompareTo(b.Name));
        }

        public void ExportStats(StringBuilder writer, ExportType exportType)
        {
            TextWriting.WriteTitle(writer, exportType, "Leading Career Records", HtmlTag.h2);

            TextWriting.WriteTitle(writer, exportType, "Appearances", HtmlTag.h3);
            TableWriting.WriteTable(writer, exportType, MostClubAppearances, headerFirstColumn: false);

            TextWriting.WriteTitle(writer, exportType, "Most Club Runs", HtmlTag.h3);
            TableWriting.WriteTable(writer, exportType, MostClubRuns, headerFirstColumn: false);

            TextWriting.WriteTitle(writer, exportType, "Batting Average", HtmlTag.h3);
            TableWriting.WriteTable(writer, exportType, HighestClubBattingAverage, headerFirstColumn: false);

            TextWriting.WriteTitle(writer, exportType, "Wickets Taken", HtmlTag.h3);
            TableWriting.WriteTable(writer, exportType, MostClubWickets, headerFirstColumn: false);

            TextWriting.WriteTitle(writer, exportType, "Other Career Records", HtmlTag.h2);
            if (PlayerBatting.Any())
            {
                TextWriting.WriteTitle(writer, exportType, "Overall Batting Performance", HtmlTag.h3);
                TableWriting.WriteTable(writer, exportType, PlayerBatting, headerFirstColumn: false);
            }

            if (PlayerBowling.Any())
            {
                TextWriting.WriteTitle(writer, exportType, "Overall Bowling Performance", HtmlTag.h3);
                TableWriting.WriteTable(writer, exportType, PlayerBowling, headerFirstColumn: false);
            }
        }
    }
}
