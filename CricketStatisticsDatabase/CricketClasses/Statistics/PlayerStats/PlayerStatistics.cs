using Cricket.Interfaces;
using Cricket.Player;
using System;
using System.Collections.Generic;

namespace Cricket.Statistics
{
    public class PlayerStatistics
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

        public PlayerAttendanceStatistics Played
        {
            get;
            set;
        }

        public PlayerStatistics()
        {
            BattingStats = new PlayerBattingStatistics();
            BowlingStats = new PlayerBowlingStatistics();
            FieldingStats = new PlayerFieldingStatistics();
            Played = new PlayerAttendanceStatistics();
        }

        public PlayerStatistics(PlayerName name)
        {
            Name = name;
            BattingStats = new PlayerBattingStatistics(name);
            BowlingStats = new PlayerBowlingStatistics(name);
            FieldingStats = new PlayerFieldingStatistics(name);
            Played = new PlayerAttendanceStatistics(name);
        }

        public PlayerStatistics(PlayerName name, ICricketSeason season)
        {
            Name = name;
            BattingStats = new PlayerBattingStatistics(name);
            BowlingStats = new PlayerBowlingStatistics(name);
            FieldingStats = new PlayerFieldingStatistics(name);
            Played = new PlayerAttendanceStatistics(name);
            SetSeasonStats(season);
        }

        public void SetSeasonStats(ICricketSeason season)
        {
            SeasonName = season.Name;
            SeasonYear = season.Year;
            BattingStats.SetSeasonStats(season);
            BowlingStats.SetSeasonStats(season);
            FieldingStats.SetSeasonStats(season);
            Played.SetSeasonStats(season);

            CalculatePartnerships(season);
        }

        public PlayerStatistics(PlayerName name, ICricketTeam match)
        {
            Name = name;
            BattingStats = new PlayerBattingStatistics(name);
            BowlingStats = new PlayerBowlingStatistics(name);
            FieldingStats = new PlayerFieldingStatistics(name);
            Played = new PlayerAttendanceStatistics(name);
            SetTeamStats(match);
        }

        public void SetTeamStats(ICricketTeam team)
        {
            BattingStats.SetTeamStats(team);
            BowlingStats.SetTeamStats(team);
            FieldingStats.SetTeamStats(team);
            Played.SetTeamStats(team);

            CalculatePartnerships(team);
        }

        public void CalculatePartnerships(ICricketSeason season)
        {
            foreach (var match in season.Matches)
            {
                var partnerships = match.Partnerships();
                for (int i = 0; i < partnerships.Count; i++)
                {
                    if (partnerships[i] != null)
                    {
                        if (PartnershipsByWicket[i] == null)
                        {
                            PartnershipsByWicket[i] = partnerships[i];
                        }
                        else
                        {
                            if (partnerships[i].ContainsPlayer(Name) && PartnershipsByWicket[i].CompareTo(partnerships[i]) > 0)
                            {
                                PartnershipsByWicket[i] = partnerships[i];
                            }
                        }
                    }
                }
            }
        }

        public void CalculatePartnerships(ICricketTeam team)
        {
            foreach (var season in team.Seasons)
            {
                foreach (var match in season.Matches)
                {
                    var partnerships = match.Partnerships();
                    for (int i = 0; i < partnerships.Count; i++)
                    {
                        if (partnerships[i] != null)
                        {
                            if (PartnershipsByWicket[i] == null)
                            {
                                PartnershipsByWicket[i] = partnerships[i];
                            }
                            else
                            {
                                if (partnerships[i].ContainsPlayer(Name) && PartnershipsByWicket[i].CompareTo(partnerships[i]) > 0)
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
}
