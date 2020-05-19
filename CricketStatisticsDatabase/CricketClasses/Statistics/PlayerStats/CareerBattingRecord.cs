using Cricket.Interfaces;
using Cricket.Player;
using System;

namespace Cricket.Statistics.PlayerStats
{
    public class CareerBattingRecord
    {
        public PlayerName Name
        {
            get;
            set;
        }

        public DateTime StartYear
        {
            get;
            set;
        }

        public DateTime EndYear
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
            StartYear = DateTime.Today;
            EndYear = new DateTime();
            foreach (var season in team.Seasons)
            {
                foreach (var match in season.Matches)
                {
                    if (match.PlayNotPlay(Name))
                    {
                        MatchesPlayed++;

                        if (match.MatchData.Date.Year < StartYear.Year)
                        {
                            StartYear = match.MatchData.Date;
                        }
                        if (match.MatchData.Date.Year > EndYear.Year)
                        {
                            EndYear = match.MatchData.Date;
                        }

                        var batting = match.GetBatting(Name);
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

                                var possibleBest = new BestBatting()
                                {
                                    Runs = batting.RunsScored,
                                    HowOut = batting.MethodOut,
                                    Opposition = match.MatchData.Opposition,
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
                Average = (double)Runs / ((double)Innings - (double)NotOut);
            }
        }

        public string ToCSVLine()
        {
            return Name.ToString() + "," + StartYear.Year + "," + EndYear.Year + "," + MatchesPlayed + "," + Innings + "," + Runs + "," + NotOut + "," + High.ToString() + "," + Average + "," + Centuries + "," + Fifties;
        }
    }
}
