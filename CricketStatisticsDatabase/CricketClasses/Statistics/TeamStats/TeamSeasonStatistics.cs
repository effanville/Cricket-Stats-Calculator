using Cricket.Interfaces;
using Cricket.Statistics;
using System;
using System.Collections.Generic;

namespace CricketStatistics
{
    public sealed class TeamSeasonStatistics
    {
        public List<PlayerSeasonStatistics> SeasonPlayerStats
        {
            get;
            set;
        } = new List<PlayerSeasonStatistics>();

        public List<Partnership> PartnershipsByWicket
        {
            get;
            set;
        } = new List<Partnership>(new Partnership[10]);

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

        public int GamesPlayed
        {
            get;
            set;
        }

        public int NumberWins
        {
            get;
            set;
        }

        public int NumberLosses
        {
            get;
            set;
        }

        public int NumberDraws
        {
            get;
            set;
        }

        public int NumberTies
        {
            get;
            set;
        }

        public TeamSeasonStatistics()
        { }

        public TeamSeasonStatistics(ICricketSeason season)
        {
            SeasonName = season.Name;
            SeasonYear = season.Year;
            CalculateTeamStats(season);
            CalculatePlayerStats(season);
            CalculatePartnerships(season);
        }

        public void CalculateTeamStats(ICricketSeason season)
        {
            if (!season.Year.Equals(SeasonYear))
            {
                return;
            }

            GamesPlayed = 0;
            NumberWins = 0;
            NumberLosses = 0;
            NumberDraws = 0;
            NumberTies = 0;
            foreach (var match in season.Matches)
            {
                GamesPlayed++;

                if (match.Result == Cricket.Match.ResultType.Win)
                {
                    NumberWins++;
                }
                if (match.Result == Cricket.Match.ResultType.Loss)
                {
                    NumberLosses++;
                }
                if (match.Result == Cricket.Match.ResultType.Draw)
                {
                    NumberDraws++;
                }
                if (match.Result == Cricket.Match.ResultType.Tie)
                {
                    NumberTies++;
                }
            }
        }

        public void CalculatePlayerStats(ICricketSeason season)
        {
            if (!season.Year.Equals(SeasonYear))
            {
                return;
            }

            foreach (var player in season.Players)
            {
                var playerStats = new PlayerSeasonStatistics(player, season);
                SeasonPlayerStats.Add(playerStats);
            }
        }

        public void CalculatePartnerships(ICricketSeason season)
        {
            foreach (var match in season.Matches)
            {
                var partnerships = match.Partnerships();
                for (int i = 0; i < partnerships.Count; i++)
                {
                    if (PartnershipsByWicket[i] == null)
                    {
                        PartnershipsByWicket[i] = partnerships[i];

                    }
                    else
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
