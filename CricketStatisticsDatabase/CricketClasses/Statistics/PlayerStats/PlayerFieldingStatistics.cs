using Cricket.Interfaces;
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

        public PlayerFieldingStatistics(PlayerName name)
        {
            Name = name;
        }

        public PlayerFieldingStatistics(PlayerName name, ICricketSeason season)
        {
            Name = name;
            SetSeasonStats(season);
        }

        public PlayerFieldingStatistics(PlayerName name, ICricketTeam team)
        {
            Name = name;
            SetTeamStats(team);
        }

        public void SetSeasonStats(ICricketSeason season, bool reset = false)
        {
            if (reset)
            {
                Catches = 0;
                RunOuts = 0;
                KeeperStumpings = 0;
                KeeperCatches = 0;
            }

            foreach (var match in season.Matches)
            {
                var fielding = match.GetFielding(Name);
                if (fielding != null)
                {
                    Catches += fielding.Catches;
                    RunOuts += fielding.RunOuts;
                    KeeperCatches += fielding.KeeperCatches;
                    KeeperStumpings += fielding.KeeperStumpings;
                }
            }
        }

        public void SetTeamStats(ICricketTeam team)
        {
            Catches = 0;
            RunOuts = 0;
            KeeperStumpings = 0;
            KeeperCatches = 0;
            foreach (var season in team.Seasons)
            {
                SetSeasonStats(season);
            }
        }
    }
}
