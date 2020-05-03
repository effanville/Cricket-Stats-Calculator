using Cricket.Interfaces;
using Cricket.Player;

namespace Cricket.Statistics
{
    public class PlayerFieldingStatistics
    {
        public static string CsvHeader()
        {
            return nameof(Name) + "," + nameof(Catches) + "," + nameof(RunOuts) + "," + "Stumpings" + "," + nameof(KeeperCatches) + "," + "Total" + "," + nameof(TotalKeeperDismissals);
        }

        public override string ToString()
        {
            return Name.ToString() + "," + Catches + "," + RunOuts + "," + KeeperStumpings + "," + KeeperCatches + "," + TotalDismissals + "," + TotalKeeperDismissals;
        }

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

        public void SetSeasonStats(ICricketSeason season)
        {
            Catches = 0;
            RunOuts = 0;
            KeeperStumpings = 0;
            KeeperCatches = 0;
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
    }
}
