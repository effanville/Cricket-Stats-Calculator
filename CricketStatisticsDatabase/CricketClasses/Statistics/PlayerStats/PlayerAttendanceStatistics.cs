using System.Linq;
using Cricket.Interfaces;
using Cricket.Match;
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
                return TotalGamesWon / (double)TotalGamesPlayed;
            }
        }

        public PlayerAttendanceStatistics()
        {
        }

        public PlayerAttendanceStatistics(PlayerName name, MatchType[] matchTypes)
        {
            Name = name;
        }

        public PlayerAttendanceStatistics(PlayerName name, ICricketSeason season, MatchType[] matchTypes)
        {
            Name = name;
            SetSeasonStats(season, matchTypes);
        }

        public PlayerAttendanceStatistics(PlayerName name, ICricketTeam team, MatchType[] matchTypes)
        {
            Name = name;
            SetTeamStats(team, matchTypes);
        }

        public void SetSeasonStats(ICricketSeason season, MatchType[] matchTypes, bool reset = false)
        {
            if (reset)
            {
                TotalGamesWon = 0;
                TotalGamesPlayed = 0;
                TotalGamesLost = 0;
                TotalMom = 0;
            }

            foreach (ICricketMatch match in season.Matches)
            {
                if (matchTypes.Contains(match.MatchData.Type))
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
        }

        public void SetTeamStats(ICricketTeam team, MatchType[] matchTypes)
        {
            TotalGamesWon = 0;
            TotalGamesPlayed = 0;
            TotalGamesLost = 0;
            TotalMom = 0;
            foreach (ICricketSeason season in team.Seasons)
            {
                SetSeasonStats(season, matchTypes);
            }
        }
    }
}
