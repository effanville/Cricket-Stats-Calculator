using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CricketStructures.Season;
using CricketStructures.Match;
using CricketStructures.Match.Innings;
using CricketStructures.Player;
using Common.Structure.ReportWriting;
using System.IO.Abstractions;
using System.Text;

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
                    if(partnerships != null)
                    {
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
                    }}
                    
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

        public void ExportStats(IFileSystem fileSystem, string filePath, DocumentType exportType)
        {
            try
            {
                StringBuilder sb = ExportString(exportType);

                using (Stream stream = fileSystem.FileStream.Create(filePath, FileMode.Create))
                using (StreamWriter fileWriter = new StreamWriter(stream))
                {
                    fileWriter.WriteLine(sb.ToString());
                }
            }
            catch (IOException)
            {
                return;
            }

            StringBuilder ExportString(DocumentType exportType)
            {
                StringBuilder sb = new StringBuilder();
                TextWriting.WriteHeader(sb, exportType, $"Statistics for Player {Name}", useColours: true);


                TextWriting.WriteTitle(sb, exportType, $"Brief Statistics for player {Name}", DocumentElement.h1);

                TextWriting.WriteTitle(sb, exportType, "Player Overall", DocumentElement.h2);
                TextWriting.WriteParagraph(sb, exportType, new string[] { "Games Played:", $"{Played.TotalGamesPlayed}" });
                TextWriting.WriteParagraph(sb, exportType, new string[] { "Wins:", $"{Played.TotalGamesWon}" });
                TextWriting.WriteParagraph(sb, exportType, new string[] { "Losses:", $"{Played.TotalGamesLost}" });
                if (BattingStats.Best != null)
                {
                    TextWriting.WriteParagraph(sb, exportType, new string[] { "Best Batting:", BattingStats.Best.ToString() });
                }
                if (BowlingStats.BestFigures != null)
                {
                    TextWriting.WriteParagraph(sb, exportType, new string[] { "Best Bowling:", BowlingStats.BestFigures.ToString() });
                }

                TextWriting.WriteTitle(sb, exportType, "Appearances", DocumentElement.h2);

                TableWriting.WriteTable(sb, exportType, new PlayerAttendanceStatistics[] { Played }, headerFirstColumn: false);

                TextWriting.WriteTitle(sb, exportType, "Batting Stats", DocumentElement.h2);
                TableWriting.WriteTable(sb, exportType, new PlayerBattingStatistics[] { BattingStats }, headerFirstColumn: false);

                TextWriting.WriteTitle(sb, exportType, "Highest Partnerships", DocumentElement.h2);
                TableWriting.WriteTable(sb, exportType, PartnershipsByWicket, headerFirstColumn: false);

                TextWriting.WriteTitle(sb, exportType, "Bowling Stats", DocumentElement.h2);
                TableWriting.WriteTable(sb, exportType, new PlayerBowlingStatistics[] { BowlingStats }, headerFirstColumn: false);

                TextWriting.WriteTitle(sb, exportType, "Fielding Stats", DocumentElement.h2);
                TableWriting.WriteTable(sb, exportType, new PlayerFieldingStatistics[] { FieldingStats }, headerFirstColumn: false);

                TextWriting.WriteFooter(sb, exportType);

                return sb;
            }
        }
    }
}
