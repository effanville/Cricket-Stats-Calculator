using Cricket.Interfaces;
using Cricket.Player;

namespace Cricket.Statistics
{
    public class PlayerAttendanceStatistics
    {
        public PlayerName Name
        {
            get;
            set;
        }

        public int TotalMom
        {
            get;
            set;
        }

        public int TotalGamesPlayed
        {
            get;
            set;
        }

        public int TotalGamesWon
        {
            get;
            set;
        }

        public int TotalGamesLost
        {
            get;
            set;
        }

        public double WinRatio
        {
            get
            {
                return (double)TotalGamesWon / (double)TotalGamesPlayed;
            }
        }

        public PlayerAttendanceStatistics()
        {
        }

        public PlayerAttendanceStatistics(PlayerName name)
        {
            Name = name;
        }

        public PlayerAttendanceStatistics(PlayerName name, ICricketSeason season)
        {
            Name = name;
            SetSeasonStats(season);
        }

        public PlayerAttendanceStatistics(PlayerName name, ICricketTeam team)
        {
            Name = name;
            SetTeamStats(team);
        }

        public void SetSeasonStats(ICricketSeason season, bool reset = false)
        {
            if (reset)
            {
                TotalGamesWon = 0;
                TotalGamesPlayed = 0;
                TotalGamesLost = 0;
                TotalMom = 0;
            }

            foreach (var match in season.Matches)
            {
                if (match.PlayNotPlay(Name))
                {
                    TotalGamesPlayed += 1;
                    if (Name.Equals(match.ManOfMatch))
                    {
                        TotalMom += 1;
                    }
                    if (match.Result == Match.ResultType.Win)
                    {
                        TotalGamesWon += 1;
                    }
                    if (match.Result == Match.ResultType.Loss)
                    {
                        TotalGamesLost += 1;
                    }
                }
            }
        }

        public void SetTeamStats(ICricketTeam team)
        {
            TotalGamesWon = 0;
            TotalGamesPlayed = 0;
            TotalGamesLost = 0;
            TotalMom = 0;
            foreach (var season in team.Seasons)
            {
                SetSeasonStats(season);
            }
        }
    }
}
