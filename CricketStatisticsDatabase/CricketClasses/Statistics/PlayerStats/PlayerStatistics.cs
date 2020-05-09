using Cricket.Interfaces;
using Cricket.Player;
using System;
using System.Collections.Generic;
using System.IO;

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
                            if (partnerships[i].ContainsPlayer(Name))
                            {
                                PartnershipsByWicket[i] = partnerships[i];
                            }
                        }
                        else
                        {
                            if (partnerships[i].ContainsPlayer(Name) && partnerships[i].CompareTo(PartnershipsByWicket[i]) > 0)
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
                                if (partnerships[i].ContainsPlayer(Name))
                                {
                                    PartnershipsByWicket[i] = partnerships[i];
                                }
                            }
                            else
                            {
                                if (partnerships[i].ContainsPlayer(Name) && partnerships[i].CompareTo(PartnershipsByWicket[i]) > 0)
                                {
                                    PartnershipsByWicket[i] = partnerships[i];
                                }
                            }
                        }
                    }
                }
            }
        }

        public void ExportStats(string filePath)
        {
            try
            {
                StreamWriter streamWriter = new StreamWriter(filePath);
                streamWriter.WriteLine("Exporting Stats");
                streamWriter.WriteLine($"For player {Name.ToString()}");

                streamWriter.WriteLine("");

                streamWriter.WriteLine("Player Overall");
                streamWriter.WriteLine($"Games Played:, {Played.TotalGamesPlayed}");

                streamWriter.WriteLine($"Wins:, {Played.TotalGamesWon}");
                streamWriter.WriteLine($"Losses:, {Played.TotalGamesLost}");

                streamWriter.WriteLine("Best Batting," + BattingStats.Best.ToString());

                streamWriter.WriteLine("Best Bowling," + BowlingStats.BestFigures.ToString());

                streamWriter.WriteLine("");
                streamWriter.WriteLine("Attendance");
                streamWriter.WriteLine("");
                streamWriter.WriteLine(PlayerAttendanceStatistics.CsvHeader());
                streamWriter.WriteLine(Played.ToString());

                streamWriter.WriteLine("");
                streamWriter.WriteLine("Batting Stats");
                streamWriter.WriteLine("");
                streamWriter.WriteLine(PlayerBattingStatistics.CsvHeader());
                streamWriter.WriteLine(BattingStats.ToString());

                streamWriter.WriteLine("");
                streamWriter.WriteLine("Highest Partnerships");
                streamWriter.WriteLine("");

                foreach (var partnership in PartnershipsByWicket)
                {
                    if (partnership != null)
                    {
                        streamWriter.WriteLine(partnership.ToString());
                    }
                }

                streamWriter.WriteLine("");
                streamWriter.WriteLine("Bowling Stats");
                streamWriter.WriteLine("");
                streamWriter.WriteLine(PlayerBowlingStatistics.CsvHeader());
                streamWriter.WriteLine(BowlingStats.ToString());

                streamWriter.WriteLine("");
                streamWriter.WriteLine("Fielding Stats");
                streamWriter.WriteLine("");
                streamWriter.WriteLine(PlayerFieldingStatistics.CsvHeader());
                streamWriter.WriteLine(FieldingStats.ToString());

                streamWriter.Close();
            }
            catch (Exception ex)
            {
            }
        }
    }
}
