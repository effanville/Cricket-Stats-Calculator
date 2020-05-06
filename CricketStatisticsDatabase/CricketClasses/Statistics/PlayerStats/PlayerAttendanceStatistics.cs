using Cricket.Interfaces;
using Cricket.Player;

namespace Cricket.Statistics
{
    public class PlayerAttendanceStatistics
    {
        public static string CsvHeader()
        {
            return nameof(Name) + "," + nameof(TotalGamesPlayed) + "," + nameof(TotalGamesWon) + "," + nameof(TotalGamesLost) + "," + nameof(TotalMom) + "," + nameof(WinRatio);
        }

        public override string ToString()
        {
            return Name.ToString() + "," + TotalGamesPlayed + "," + TotalGamesWon + "," + TotalGamesLost + "," + TotalMom + "," + WinRatio;
        }

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

        public void SetSeasonStats(ICricketSeason season)
        {
            TotalGamesWon = 0;
            TotalGamesPlayed = 0;
            TotalGamesLost = 0;
            TotalMom = 0;
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
    }
}
