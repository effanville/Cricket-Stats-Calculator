namespace CricketStructures.Season
{
    public sealed class SeasonGames
    {
        public int GamesPlayed
        {
            get;
            set;
        }

        public int NumberWins
        {
            get;
            set;
        }

        public int NumberLosses
        {
            get;
            set;
        }

        public int NumberDraws
        {
            get;
            set;
        }

        public int NumberTies
        {
            get;
            set;
        }

        public int NumberAbandoned
        {
            get;
            set;
        }

        public SeasonGames(int gamesPlayed, int numberWins, int numberLosses, int numberDraws, int numberTies, int numberAbandoned)
        {
            GamesPlayed = gamesPlayed;
            NumberWins = numberWins;
            NumberLosses = numberLosses;
            NumberDraws = numberDraws;
            NumberTies = numberTies;
            NumberAbandoned = numberAbandoned;
        }
    }
}
