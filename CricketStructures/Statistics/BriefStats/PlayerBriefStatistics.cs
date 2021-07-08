using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CricketStructures.Interfaces;
using CricketStructures.Match;
using CricketStructures.Match.Innings;
using CricketStructures.Player;
using StructureCommon.FileAccess;

namespace CricketStructures.Statistics
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
        public PlayerBriefStatistics(string teamName, PlayerName name, ICricketSeason season)
            : this(teamName, name, season, MatchHelpers.AllMatchTypes)
        {
        }

        public PlayerBriefStatistics(string teamName, PlayerName name, ICricketSeason season, Match.MatchType[] matchTypes)
        {
            Name = name;
            SeasonName = season.Name;
            SeasonYear = season.Year;
            BattingStats = new PlayerBattingStatistics(teamName, name, season, matchTypes);
            BowlingStats = new PlayerBowlingStatistics(teamName, name, season, matchTypes);
            FieldingStats = new PlayerFieldingStatistics(teamName, name, season, matchTypes);
            Played = new PlayerAttendanceStatistics(teamName, name, season, matchTypes);
            CalculatePartnerships(teamName, season, matchTypes);
        }

        public PlayerBriefStatistics(PlayerName name, ICricketTeam team)
            : this(name, team, MatchHelpers.AllMatchTypes)
        {
        }
        public PlayerBriefStatistics(PlayerName name, ICricketTeam team, Match.MatchType[] matchTypes)
        {
            Name = name;
            BattingStats = new PlayerBattingStatistics(name, team, matchTypes);
            BowlingStats = new PlayerBowlingStatistics(name, team, matchTypes);
            FieldingStats = new PlayerFieldingStatistics(name, team, matchTypes);
            Played = new PlayerAttendanceStatistics(name, team, matchTypes);

            CalculatePartnerships(team, matchTypes);
        }

        public void CalculatePartnerships(string teamName, ICricketSeason season, Match.MatchType[] matchtypes)
        {
            foreach (var match in season.Matches)
            {
                if (matchtypes.Contains(match.MatchData.Type))
                {
                    var partnerships = match.Partnerships(teamName);
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

        public void CalculatePartnerships(ICricketTeam team, Match.MatchType[] matchTypes)
        {
            foreach (var season in team.Seasons)
            {
                CalculatePartnerships(team.TeamName, season, matchTypes);
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
                    streamWriter.CreateHTMLHeader($"Statistics for Player {Name}", useColours: true);
                }
                streamWriter.WriteTitle(exportType, $"Brief Statistics for player {Name}", HtmlTag.h1);

                streamWriter.WriteTitle(exportType, "Player Overall", HtmlTag.h2);
                streamWriter.WriteParagraph(exportType, new string[] { "Games Played:", $"{Played.TotalGamesPlayed}" });
                streamWriter.WriteParagraph(exportType, new string[] { "Wins:", $"{Played.TotalGamesWon}" });
                streamWriter.WriteParagraph(exportType, new string[] { "Losses:", $"{Played.TotalGamesLost}" });
                if (BattingStats.Best != null)
                {
                    streamWriter.WriteParagraph(exportType, new string[] { "Best Batting:", BattingStats.Best.ToString() });
                }
                if (BowlingStats.BestFigures != null)
                {
                    streamWriter.WriteParagraph(exportType, new string[] { "Best Bowling:", BowlingStats.BestFigures.ToString() });
                }
                streamWriter.WriteTitle(exportType, "Appearances", HtmlTag.h2);

                streamWriter.WriteTable(exportType, new PlayerAttendanceStatistics[] { Played }, headerFirstColumn: false);

                streamWriter.WriteTitle(exportType, "Batting Stats", HtmlTag.h2);
                streamWriter.WriteTable(exportType, new PlayerBattingStatistics[] { BattingStats }, headerFirstColumn: false);

                streamWriter.WriteTitle(exportType, "Highest Partnerships", HtmlTag.h2);
                streamWriter.WriteTable(exportType, PartnershipsByWicket, headerFirstColumn: false);

                streamWriter.WriteTitle(exportType, "Bowling Stats", HtmlTag.h2);
                streamWriter.WriteTable(exportType, new PlayerBowlingStatistics[] { BowlingStats }, headerFirstColumn: false);

                streamWriter.WriteTitle(exportType, "Fielding Stats", HtmlTag.h2);
                streamWriter.WriteTable(exportType, new PlayerFieldingStatistics[] { FieldingStats }, headerFirstColumn: false);

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
