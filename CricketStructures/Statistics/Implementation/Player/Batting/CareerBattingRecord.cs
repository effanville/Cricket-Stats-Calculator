using System;
using System.Text;

using Common.Structure.ReportWriting;

using CricketStructures.Match;
using CricketStructures.Match.Innings;
using CricketStructures.Player;
using CricketStructures.Season;
using CricketStructures.Statistics.Implementation.Player.Model;

namespace CricketStructures.Statistics.Implementation.Player.Batting
{
    public sealed class CareerBattingRecord : ICricketStat
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

        public int MatchesPlayed
        {
            get;
            set;
        }

        public int Innings
        {
            get;
            set;
        }

        public int Runs
        {
            get;
            set;
        }

        public int NotOut
        {
            get;
            set;
        }

        public BestBatting High
        {
            get;
            set;
        }

        public double Average
        {
            get;
            set;
        }

        public int Centuries
        {
            get;
            set;
        }

        public int Fifties
        {
            get;
            set;
        }

        public CareerBattingRecord()
        {
        }

        public CareerBattingRecord(PlayerName name)
        {
            Name = name;
        }

        public CareerBattingRecord(PlayerName name, ICricketTeam team)
            : this(name)
        {
            CalculateStats(team, MatchHelpers.AllMatchTypes);
        }

        public void CalculateStats(ICricketTeam team, MatchType[] matchTypes)
        {
            CricketStatsHelpers.SeasonIterator(
                team.Seasons,
                season => CalculateStats(team.TeamName, season, matchTypes),
                postCycleAction: Finalise);

        }

        private void Finalise()
        {
            if (Innings != NotOut)
            {
                Average = Runs / (Innings - (double)NotOut);
            }
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
                MatchesPlayed++;

                if (match.MatchData.Date.Year < StartYear)
                {
                    StartYear = match.MatchData.Date.Year;
                }
                if (match.MatchData.Date.Year > EndYear)
                {
                    EndYear = match.MatchData.Date.Year;
                }

                BattingEntry batting = match.GetBatting(teamName, Name);
                if (batting != null)
                {
                    if (batting.MethodOut != Wicket.DidNotBat)
                    {
                        Innings++;
                        if (!batting.Out())
                        {
                            NotOut++;
                        }

                        Runs += batting.RunsScored;

                        if (batting.RunsScored >= 50 && batting.RunsScored < 100)
                        {
                            Fifties++;
                        }
                        if (batting.RunsScored >= 100)
                        {
                            Centuries++;
                        }

                        BestBatting possibleBest = new BestBatting()
                        {
                            Runs = batting.RunsScored,
                            HowOut = batting.MethodOut,
                            Opposition = match.MatchData.OppositionName(teamName),
                            Date = match.MatchData.Date
                        };

                        if (possibleBest.CompareTo(High) > 0)
                        {
                            High = possibleBest;
                        }
                    }
                }
            }
        }

        public void ResetStats()
        {
            MatchesPlayed = 0;
            Innings = 0;
            NotOut = 0;
            Runs = 0;
            High = new BestBatting();
            StartYear = DateTime.Today.Year;
            EndYear = new DateTime().Year;
        }

        public StringBuilder ExportStats(DocumentType exportType, DocumentElement headerElement)
        {
            var writer = new StringBuilder();
            TextWriting.WriteTitle(writer, exportType, "Overall Batting Performance", headerElement);
            var fields = new string[]
            {
                Name.ToString(),
                StartYear.ToString(),
                EndYear.ToString(),
                MatchesPlayed.ToString(),
                Innings.ToString(),
                Runs.ToString(),
                NotOut.ToString(),
                High.ToString(),
                Average.ToString(),
                Centuries.ToString(),
                Fifties.ToString()
            };
            TableWriting.WriteTable(writer, exportType, fields, headerFirstColumn: false);
            return writer;
        }
    }
}
