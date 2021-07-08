using System.Linq;
using CricketStructures.Interfaces;
using CricketStructures.Match;
using CricketStructures.Player;

namespace CricketStructures.Statistics
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

        public PlayerAttendanceStatistics(PlayerName name)
        {
            Name = name;
        }

        public PlayerAttendanceStatistics(string teamName, PlayerName name, ICricketSeason season, MatchType[] matchTypes)
        {
            Name = name;
            SetSeasonStats(teamName, season, matchTypes);
        }

        public PlayerAttendanceStatistics(PlayerName name, ICricketTeam team, MatchType[] matchTypes)
        {
            Name = name;
            SetTeamStats(team, matchTypes);
        }

        public void SetSeasonStats(string teamName, ICricketSeason season, MatchType[] matchTypes, bool reset = false)
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
                    if (match.PlayNotPlay(teamName, Name))
                    {
                        TotalGamesPlayed += 1;
                        if (match.MenOfMatch.Contains(Name))
                        {
                            TotalMom += 1;
                        }
                        if (match.Result == ResultType.Win)
                        {
                            TotalGamesWon += 1;
                        }
                        if (match.Result == ResultType.Loss)
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
                SetSeasonStats(team.TeamName, season, matchTypes);
            }
        }
    }
}
