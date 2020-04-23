using Cricket.Interfaces;
using Cricket.Player;
using System;

namespace Cricket.Statistics
{
    public class PlayerSeasonStatistics
    {
        public PlayerName Name
        {
            get;
            set;
        }

        public string SeasonName
        {
            get;
            set;
        }

        public DateTime SeasonYear
        {
            get;
            set;
        }

        public PlayerBattingStatistics BattingStats
        {
            get;
            set;
        }
        public PlayerBowlingStatistics BowlingStats
        {
            get;
            set;
        }
        public PlayerFieldingStatistics FieldingStats
        {
            get;
            set;
        }

        private int fTotalMom;
        public int TotalMom
        {
            get { return fTotalMom; }
            set { fTotalMom = value; }
        }

        private int fTotalGames;
        public int TotalGamesPlayed
        {
            get { return fTotalGames; }
            set { fTotalGames = value; }
        }

        public PlayerSeasonStatistics()
        {
            BattingStats = new PlayerBattingStatistics();
            BowlingStats = new PlayerBowlingStatistics();
            FieldingStats = new PlayerFieldingStatistics();
        }

        public PlayerSeasonStatistics(PlayerName name)
        {
            Name = name;
            BattingStats = new PlayerBattingStatistics(name);
            BowlingStats = new PlayerBowlingStatistics(name);
            FieldingStats = new PlayerFieldingStatistics(name);
        }

        public PlayerSeasonStatistics(PlayerName name, ICricketSeason season)
        {
            Name = name;
            BattingStats = new PlayerBattingStatistics(name);
            BowlingStats = new PlayerBowlingStatistics(name);
            FieldingStats = new PlayerFieldingStatistics(name);
            SetSeasonStats(season);
        }

        public void SetSeasonStats(ICricketSeason season)
        {
            SeasonName = season.Name;
            SeasonYear = season.Year;
            BattingStats.SetSeasonStats(season);
            BowlingStats.SetSeasonStats(season);
            FieldingStats.SetSeasonStats(season);

            TotalGamesPlayed = season.Matches.FindAll(match => match.PlayNotPlay(Name)).Count;
            TotalMom = season.Matches.FindAll(match => match.ManOfMatch.Equals(Name)).Count;
        }
    }
}
