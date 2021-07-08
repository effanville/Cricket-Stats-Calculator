using CricketStructures.Interfaces;
using CricketStructures.Match;

namespace CricketStructures.Statistics.DetailedStats
{
    public class TeamRecord
    {
        public int Played
        {
            get;
            set;
        }

        public int Won
        {
            get;
            set;
        }

        public int Drew
        {
            get;
            set;
        }

        public int Lost
        {
            get;
            set;
        }

        public int Abandoned
        {
            get;
            set;
        }

        public int Tie
        {
            get;
            set;
        }

        public double WinRatio
        {
            get;
            set;
        }
        public TeamRecord()
        {
        }

        public TeamRecord(ICricketSeason season)
        {
            season.CalculateGamesPlayed(MatchHelpers.AllMatchTypes);
            Played = season.GamesPlayed;
            Won = season.NumberWins;
            Lost = season.NumberLosses;
            Drew = season.NumberDraws;
            Tie = season.NumberTies;
            WinRatio = Won / (double)Played;
        }

        public string ToCSVLine()
        {
            return Played + "," + Won + "," + Drew + "," + Lost + "," + Abandoned + "," + Tie + "," + WinRatio;
        }
    }
}
