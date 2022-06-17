using System;

using CricketStructures.Player;

namespace CricketStructures.Season
{
    public class SeasonRuns
    {
        public DateTime Year
        {
            get;
        }

        public PlayerName Name
        {
            get;
        }
        public int Innings
        {
            get;
        }

        public int NotOut
        {
            get;
        }

        public int Runs
        {
            get;
        }

        public double Average
        {
            get;
        }

        public SeasonRuns()
        {
        }

        public SeasonRuns(DateTime year, PlayerName name, int innings, int notOut, int runs, double average)
        {
            Year = year;
            Name = name;
            Innings = innings;
            NotOut = notOut;
            Runs = runs;
            Average = average;
        }
    }
}
