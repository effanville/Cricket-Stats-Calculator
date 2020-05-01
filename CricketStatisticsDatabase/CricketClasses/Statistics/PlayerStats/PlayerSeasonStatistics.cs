using Cricket.Interfaces;
using Cricket.Player;
using System;
using System.Collections.Generic;

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

        public List<Partnership> PartnershipsByWicket
        {
            get;
            set;
        } = new List<Partnership>(new Partnership[10]);

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
            TotalMom = season.Matches.FindAll(match => Name.Equals(match.ManOfMatch)).Count;

            CalculatePartnerships(season);
        }

        public void CalculatePartnerships(ICricketSeason season)
        {
            foreach (var match in season.Matches)
            {
                var partnerships = match.Partnerships();
                for (int i = 0; i < partnerships.Count; i++)
                {
                    if (partnerships[i] == null)
                    {
                        PartnershipsByWicket[i] = partnerships[i];
                    }
                    else
                    {
                        if (partnerships[i].ContainsPlayer(Name))
                        {
                            if (PartnershipsByWicket[i].CompareTo(partnerships[i]) > 0)
                            {
                                PartnershipsByWicket[i] = partnerships[i];
                            }
                        }
                    }
                }
            }
        }
    }
}
