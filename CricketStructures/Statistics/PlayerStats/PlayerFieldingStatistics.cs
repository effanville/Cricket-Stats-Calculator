using System.Linq;
using CricketStructures.Interfaces;
using CricketStructures.Match;
using CricketStructures.Match.Innings;
using CricketStructures.Player;

namespace CricketStructures.Statistics
{
    public class PlayerFieldingStatistics
    {
        public PlayerName Name
        {
            get;
            set;
        }

        public int Catches
        {
            get;
            set;
        }

        public int RunOuts
        {
            get;
            set;
        }

        public int KeeperStumpings
        {
            get;
            set;
        }

        public int KeeperCatches
        {
            get;
            set;
        }

        public int TotalDismissals
        {
            get
            {
                return Catches + RunOuts + KeeperCatches + KeeperStumpings;
            }
        }

        public int TotalKeeperDismissals
        {
            get
            {
                return KeeperCatches + KeeperStumpings;
            }
        }

        public int TotalNonKeeperDismissals
        {
            get
            {
                return Catches + RunOuts;
            }
        }

        public PlayerFieldingStatistics()
        {
        }

        public PlayerFieldingStatistics(PlayerName name)
        {
            Name = name;
        }

        public PlayerFieldingStatistics(string teamName, PlayerName name, ICricketSeason season, MatchType[] matchTypes)
        {
            Name = name;
            SetSeasonStats(teamName, season, matchTypes);
        }

        public PlayerFieldingStatistics(PlayerName name, ICricketTeam team, MatchType[] matchTypes)
        {
            Name = name;
            SetTeamStats(team, matchTypes);
        }

        public void SetSeasonStats(string teamName, ICricketSeason season, MatchType[] matchTypes, bool reset = false)
        {
            if (reset)
            {
                Catches = 0;
                RunOuts = 0;
                KeeperStumpings = 0;
                KeeperCatches = 0;
            }

            foreach (ICricketMatch match in season.Matches)
            {
                if (matchTypes.Contains(match.MatchData.Type))
                {
                    FieldingEntry fielding = match.GetFielding(teamName, Name);
                    if (fielding != null)
                    {
                        Catches += fielding.Catches;
                        RunOuts += fielding.RunOuts;
                        KeeperCatches += fielding.KeeperCatches;
                        KeeperStumpings += fielding.KeeperStumpings;
                    }
                }
            }
        }

        public void SetTeamStats(ICricketTeam team, MatchType[] matchTypes)
        {
            Catches = 0;
            RunOuts = 0;
            KeeperStumpings = 0;
            KeeperCatches = 0;
            foreach (ICricketSeason season in team.Seasons)
            {
                SetSeasonStats(team.TeamName, season, matchTypes);
            }
        }
    }
}
