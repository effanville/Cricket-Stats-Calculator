using Cricket.Interfaces;
using Cricket.Player;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;

namespace Cricket.Statistics
{
    public class PlayerBriefStatistics
    {
        public StatisticsType SeasonOrAllYear
        {
            get;
            set;
        }

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

        public PlayerBriefStatistics()
        {
            BattingStats = new PlayerBattingStatistics();
            BowlingStats = new PlayerBowlingStatistics();
            FieldingStats = new PlayerFieldingStatistics();
            Played = new PlayerAttendanceStatistics();
        }

        public PlayerBriefStatistics(PlayerName name)
        {
            Name = name;
            BattingStats = new PlayerBattingStatistics(name);
            BowlingStats = new PlayerBowlingStatistics(name);
            FieldingStats = new PlayerFieldingStatistics(name);
            Played = new PlayerAttendanceStatistics(name);
        }

        public PlayerBriefStatistics(PlayerName name, ICricketSeason season)
        {
            Name = name;
            SeasonName = season.Name;
            SeasonYear = season.Year;
            BattingStats = new PlayerBattingStatistics(name, season);
            BowlingStats = new PlayerBowlingStatistics(name, season);
            FieldingStats = new PlayerFieldingStatistics(name, season);
            Played = new PlayerAttendanceStatistics(name, season);
            CalculatePartnerships(season);
        }

        public PlayerBriefStatistics(PlayerName name, ICricketTeam team)
        {
            Name = name;
            BattingStats = new PlayerBattingStatistics(name, team);
            BowlingStats = new PlayerBowlingStatistics(name, team);
            FieldingStats = new PlayerFieldingStatistics(name, team);
            Played = new PlayerAttendanceStatistics(name, team);

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
                CalculatePartnerships(season);
            }
        }

        public override string ToString()
        {
            return Name.ToString();
        }

        public void ExportStats(string filePath, ExportType exportType)
        {
            try
            {
                StreamWriter streamWriter = new StreamWriter(filePath);
                if (exportType.Equals(ExportType.Html))
                {
                    streamWriter.CreateHTMLHeader($"Statistics for Player {Name}");
                }
                streamWriter.WriteTitle(exportType, $"Brief Statistics for player {Name}", HtmlTag.h1);

                streamWriter.WriteTitle(exportType, "Player Overall", HtmlTag.h2);
                streamWriter.WriteParagraph(exportType, new string[] { "Games Played:", $"{Played.TotalGamesPlayed}" });
                streamWriter.WriteParagraph(exportType, new string[] { "Wins:", $"{Played.TotalGamesWon}" });
                streamWriter.WriteParagraph(exportType, new string[] { "Losses:", $"{Played.TotalGamesLost}" });
                streamWriter.WriteParagraph(exportType, new string[] { "Best Batting:", BattingStats.Best.ToString() });
                streamWriter.WriteParagraph(exportType, new string[] { "Best Bowling:", BowlingStats.BestFigures.ToString() });

                streamWriter.WriteTitle(exportType, "Appearances", HtmlTag.h2);

                streamWriter.WriteTable(exportType, new PlayerAttendanceStatistics[] { Played });

                streamWriter.WriteTitle(exportType, "Batting Stats", HtmlTag.h2);
                streamWriter.WriteTable(exportType, new PlayerBattingStatistics[] { BattingStats });

                streamWriter.WriteTitle(exportType, "Highest Partnerships", HtmlTag.h2);
                streamWriter.WriteTable(exportType, PartnershipsByWicket);

                streamWriter.WriteTitle(exportType, "Bowling Stats", HtmlTag.h2);
                streamWriter.WriteTable(exportType, new PlayerBowlingStatistics[] { BowlingStats });

                streamWriter.WriteTitle(exportType, "Fielding Stats", HtmlTag.h2);
                streamWriter.WriteTable(exportType, new PlayerFieldingStatistics[] { FieldingStats });

                if (exportType.Equals(ExportType.Html))
                {
                    streamWriter.CreateHTMLFooter();
                }
                streamWriter.Close();
            }
            catch (Exception)
            {
            }
        }
    }
}
