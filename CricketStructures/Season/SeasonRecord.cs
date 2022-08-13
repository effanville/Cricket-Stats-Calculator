using System;
using System.Collections.Generic;

using CricketStructures.Match;

namespace CricketStructures.Season
{
    public sealed class SeasonRecord
    {
        private readonly IDictionary<ResultType, int> fResults;

        public DateTime Year
        {
            get;
        }

        public int GamesPlayed
        {
            get;
        }

        public int NumberWins => fResults[ResultType.Win];

        public int NumberLosses => fResults[ResultType.Loss];

        public int NumberDraws => fResults[ResultType.Draw];

        public int NumberTies => fResults[ResultType.Tie];

        public int NumberAbandoned => fResults[ResultType.Abandoned];

        public SeasonRecord(
            DateTime year,
            int gamesPlayed,
            IDictionary<ResultType, int> results)
        {
            Year = year;
            GamesPlayed = gamesPlayed;
            fResults = results;
        }
    }
}
