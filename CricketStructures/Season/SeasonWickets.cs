using CricketStructures.Player;
using CricketStructures.Statistics.Implementation.Player;
using CricketStructures.Statistics.Implementation.Player.Model;

namespace CricketStructures.Season
{
    public class SeasonWickets
    {
        public int Year
        {
            get;
            set;
        }

        public PlayerName Name
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

        public double Economy
        {
            get;
            set;
        }
        public double StrikeRate
        {
            get;
            set;
        }

        public BestBowling BestFigures
        {
            get;
            set;
        }

        public SeasonWickets()
        {
        }

        public SeasonWickets(int year, PlayerName name, PlayerBowlingStatistics stats)
        {
            Year = year;
            Name = name;
            Overs = stats.TotalOvers;
            Maidens = stats.TotalMaidens;
            RunsConceded = stats.TotalRunsConceded;
            Wickets = stats.TotalWickets;
            Average = stats.Average;
            Economy = stats.Economy;
            StrikeRate = stats.StrikeRate;
            BestFigures = stats.BestFigures;
        }
    }
}
