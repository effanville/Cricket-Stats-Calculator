using System;
using System.Linq;
using System.Text;

using Common.Structure.ReportWriting;

using CricketStructures.Match;
using CricketStructures.Match.Innings;
using CricketStructures.Player;
using CricketStructures.Season;
using CricketStructures.Statistics.Implementation.Player.Model;

namespace CricketStructures.Statistics.Implementation.Player.Bowling
{
    public class CareerBowlingRecord : ICricketStat
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

        public double Overs
        {
            get;
            set;
        }

        public int Maidens
        {
            get;
            set;
        }

        public int RunsConceded
        {
            get;
            set;
        }

        public int Wickets
        {
            get;
            set;
        }

        public double Average
        {
            get;
            set;
        }

        public BestBowling BestFigures
        {
            get;
            set;
        }

        public int Catches
        {
            get;
            set;
        }

        public int KeeperDismissals
        {
            get;
            set;
        }

        public CareerBowlingRecord()
        {
        }

        public CareerBowlingRecord(PlayerName name)
        {
            Name = name;
        }

        public CareerBowlingRecord(PlayerName name, ICricketTeam team)
            : this(name)
        {
            CalculateStats(team, MatchHelpers.AllMatchTypes);
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
            Average = Wickets != 0 ? RunsConceded / (double)Wickets : double.NaN;
        }

        public void CalculateStats(string teamName, ICricketSeason season, MatchType[] matchTypes)
        {
            CricketStatsHelpers.MatchIterator(
                season,
                matchTypes,
                match => UpdateStats(teamName, match));
        }

        public void UpdateStats(string teamName, ICricketMatch match)
        {
            if (match.PlayNotPlay(teamName, Name))
            {
                if (match.MatchData.Date.Year < StartYear)
                {
                    StartYear = match.MatchData.Date.Year;
                }
                if (match.MatchData.Date.Year > EndYear)
                {
                    EndYear = match.MatchData.Date.Year;
                }

                BowlingEntry bowling = match.GetBowling(teamName, Name);
                if (bowling != null)
                {
                    Overs += bowling.OversBowled;
                    Maidens += bowling.Maidens;
                    RunsConceded += bowling.RunsConceded;
                    Wickets += bowling.Wickets;

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

                FieldingEntry fielding = match.GetFielding(teamName, Name);
                if (fielding != null)
                {
                    Catches += fielding.Catches;
                    KeeperDismissals += fielding.KeeperStumpings + fielding.KeeperCatches;
                }
            }
        }

        public void ResetStats()
        {
            Overs = 0;
            Maidens = 0;
            RunsConceded = 0;
            Wickets = 0;
            BestFigures = new BestBowling();
            StartYear = DateTime.Today.Year;
            EndYear = new DateTime().Year;
        }

        public StringBuilder ExportStats(DocumentType exportType, DocumentElement headerElement)
        {
            var writer = new StringBuilder();
            TextWriting.WriteTitle(writer, exportType, "Overall Bowling Performance", headerElement);
            var fields = new string[]
            {
                Name.ToString(),
                StartYear.ToString(),
                EndYear.ToString(),
                Overs.ToString(),
                Maidens.ToString(),
                RunsConceded.ToString(),
                Wickets.ToString(),
                Average.ToString(),
                BestFigures.ToString(),
                Catches.ToString(),
                KeeperDismissals.ToString()
            };
            TableWriting.WriteTable(writer, exportType, fields, headerFirstColumn: false);
            return writer;
        }
    }
}
