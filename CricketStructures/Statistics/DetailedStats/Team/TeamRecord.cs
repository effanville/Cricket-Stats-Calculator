using CricketStructures.Match;
using CricketStructures.Season;

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
            var seasonGames = season.CalculateGamesPlayed(MatchHelpers.AllMatchTypes);
            Played = seasonGames.GamesPlayed;
            Won = seasonGames.NumberWins;
            Lost = seasonGames.NumberLosses;
            Drew = seasonGames.NumberDraws;
            Tie = seasonGames.NumberTies;
            WinRatio = Won / (double)Played;
        }

        public string ToCSVLine()
        {
            return Played + "," + Won + "," + Drew + "," + Lost + "," + Abandoned + "," + Tie + "," + WinRatio;
        }
    }
}
