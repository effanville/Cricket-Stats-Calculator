using System.Linq;
using Cricket.Interfaces;
using Cricket.Match;
using Cricket.Player;

namespace Cricket.Statistics
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

        public PlayerFieldingStatistics(PlayerName name, MatchType[] matchTypes)
        {
            Name = name;
        }

        public PlayerFieldingStatistics(PlayerName name, ICricketSeason season, MatchType[] matchTypes)
        {
            Name = name;
            SetSeasonStats(season, matchTypes);
        }

        public PlayerFieldingStatistics(PlayerName name, ICricketTeam team, MatchType[] matchTypes)
        {
            Name = name;
            SetTeamStats(team, matchTypes);
        }

        public void SetSeasonStats(ICricketSeason season, MatchType[] matchTypes, bool reset = false)
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
                    Match.FieldingEntry fielding = match.GetFielding(Name);
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
                SetSeasonStats(season, matchTypes);
            }
        }
    }
}
