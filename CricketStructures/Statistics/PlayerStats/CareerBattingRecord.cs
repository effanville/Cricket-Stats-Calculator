using System;
using CricketStructures.Match;
using CricketStructures.Match.Innings;
using CricketStructures.Player;
using CricketStructures.Season;

namespace CricketStructures.Statistics.PlayerStats
{
    public class CareerBattingRecord
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

        public CareerBattingRecord(PlayerName name, ICricketTeam team)
        {
            Name = name;
            MatchesPlayed = 0;
            Innings = 0;
            NotOut = 0;
            Runs = 0;
            High = new BestBatting();
            StartYear = DateTime.Today.Year;
            EndYear = new DateTime().Year;
            foreach (ICricketSeason season in team.Seasons)
            {
                foreach (ICricketMatch match in season.Matches)
                {
                    if (match.PlayNotPlay(team.TeamName, Name))
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

                        BattingEntry batting = match.GetBatting(team.TeamName, Name);
                        if (batting != null)
                        {
                            if (batting.MethodOut != Match.Wicket.DidNotBat)
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
                                    Opposition = match.MatchData.OppositionName(team.TeamName),
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
            }

            if (Innings != NotOut)
            {
                Average = Runs / (Innings - (double)NotOut);
            }
        }
    }
}
