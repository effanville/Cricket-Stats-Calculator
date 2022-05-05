using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CricketStructures.Season;
using CricketStructures.Match;
using CricketStructures.Match.Innings;
using CricketStructures.Player;
using CricketStructures.Player.Interfaces;
using Common.Structure.ReportWriting;
using System.Text;
using System.IO.Abstractions;

namespace CricketStructures.Statistics
{
    public sealed class TeamBriefStatistics
    {
        public StatisticsType SeasonOrAllYear
        {
            get;
            set;
        }

        public List<PlayerBriefStatistics> SeasonPlayerStats
        {
            get;
            set;
        } = new List<PlayerBriefStatistics>();

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

        public TeamBriefStatistics()
        {
        }

        public TeamBriefStatistics(ICricketTeam team)
            : this(team, MatchHelpers.AllMatchTypes)
        {
        }

        public TeamBriefStatistics(ICricketTeam team, Match.MatchType[] matcheTypes)
        {
            SeasonOrAllYear = StatisticsType.AllTimeBrief;
            CalculateTeamStats(team, matcheTypes);
            CalculatePlayerStats(team, matcheTypes);
            CalculatePartnerships(team, matcheTypes);
        }

        public TeamBriefStatistics(string teamName, ICricketSeason season, Match.MatchType[] matcheTypes)
        {
            SeasonOrAllYear = StatisticsType.SeasonBrief;
            SeasonName = season.Name;
            SeasonYear = season.Year;
            CalculateTeamStats(season, matcheTypes);
            CalculatePlayerStats(teamName, season, matcheTypes);
            CalculatePartnerships(teamName, season, matcheTypes);
        }

        public TeamBriefStatistics(string teamName, ICricketSeason season)
            : this(teamName, season, MatchHelpers.AllMatchTypes)
        {
        }

        public void CalculateTeamStats(ICricketTeam team, Match.MatchType[] matchTypes)
        {
            foreach (ICricketSeason season in team.Seasons)
            {
                CalculateTeamStats(season, matchTypes);
            }
        }

        public void CalculateTeamStats(ICricketSeason season)
        {
            CalculateTeamStats(season, MatchHelpers.AllMatchTypes);
        }

        public void CalculateTeamStats(ICricketSeason season, Match.MatchType[] matchTypes)
        {
            if (SeasonOrAllYear == StatisticsType.SeasonBrief && !season.Year.Equals(SeasonYear))
            {
                return;
            }
            var seasonGames = season.CalculateGamesPlayed(matchTypes);

            GamesPlayed += seasonGames.GamesPlayed;
            NumberWins += seasonGames.NumberWins;
            NumberLosses += seasonGames.NumberLosses;
            NumberDraws += seasonGames.NumberDraws;
            NumberTies += seasonGames.NumberTies;
        }

        public void CalculatePlayerStats(ICricketTeam team, Match.MatchType[] matchTypes)
        {
            foreach (ICricketPlayer player in team.Players)
            {
                PlayerBriefStatistics playerStats = new PlayerBriefStatistics(player.Name, team, matchTypes);
                SeasonPlayerStats.Add(playerStats);
            }
        }

        public void CalculatePlayerStats(string teamName, ICricketSeason season, Match.MatchType[] matchTypes)
        {
            if (!season.Year.Equals(SeasonYear))
            {
                return;
            }

            foreach (PlayerName player in season.Players(teamName))
            {
                PlayerBriefStatistics playerStats = new PlayerBriefStatistics(teamName, player, season, matchTypes);
                SeasonPlayerStats.Add(playerStats);
            }
        }

        public void CalculatePartnerships(ICricketTeam team, Match.MatchType[] matchTypes)
        {
            foreach (ICricketSeason season in team.Seasons)
            {
                CalculatePartnerships(team.TeamName, season, matchTypes);
            }
        }

        public void CalculatePartnerships(string teamName, ICricketSeason season, Match.MatchType[] matchTypes)
        {
            foreach (ICricketMatch match in season.Matches)
            {
                if (matchTypes.Contains(match.MatchData.Type))
                {
                    List<Partnership> partnerships = match.Partnerships(teamName);
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
                                if (partnerships[i].CompareTo(PartnershipsByWicket[i]) > 0)
                                {
                                    PartnershipsByWicket[i] = partnerships[i];
                                }
                            }
                        }
                    }
                }
            }
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
                TextWriting.WriteHeader(sb, exportType, "Statistics for team", useColours: true);

                if (SeasonOrAllYear == StatisticsType.AllTimeBrief)
                {
                    TextWriting.WriteTitle(sb, exportType, "All time Brief Statistics", DocumentElement.h1);
                }
                if (SeasonOrAllYear == StatisticsType.SeasonBrief)
                {
                    TextWriting.WriteTitle(sb, exportType, $"For season {SeasonYear.Year}-{SeasonName}", DocumentElement.h1);
                }

                TextWriting.WriteTitle(sb, exportType, "Team Overall", DocumentElement.h2);
                TextWriting.WriteParagraph(sb, exportType, new string[] { "Games Played:", $"{GamesPlayed}" });
                TextWriting.WriteParagraph(sb, exportType, new string[] { "Wins:", $"{NumberWins}" });
                TextWriting.WriteParagraph(sb, exportType, new string[] { "Losses:", $"{NumberLosses}" });
                TextWriting.WriteParagraph(sb, exportType, new string[] { "Draws:", $"{NumberDraws}" });
                TextWriting.WriteParagraph(sb, exportType, new string[] { "Ties:", $"{NumberTies}" });

                (BestBatting Best, PlayerName Name) bestBatting = SeasonPlayerStats.Select(player => (player.BattingStats.Best, player.Name)).Max();
                TextWriting.WriteParagraph(sb, exportType, new string[] { "Best Batting:", bestBatting.Name.ToString(), bestBatting.Best.ToString() });

                (BestBowling BestFigures, PlayerName Name) = SeasonPlayerStats.Select(player => (player.BowlingStats.BestFigures, player.Name)).Max();
                TextWriting.WriteParagraph(sb, exportType, new string[] { "Best Bowling:", Name.ToString(), BestFigures.ToString() });

                List<PlayerFieldingStatistics> fielding = SeasonPlayerStats.Select(player => player.FieldingStats).ToList();
                int mostKeeper = fielding.Max(player => player.TotalKeeperDismissals);
                List<PlayerName> keepers = fielding.Where(player => player.TotalKeeperDismissals.Equals(mostKeeper)).Select(player => player.Name).ToList();
                TextWriting.WriteParagraph(sb, exportType, new string[] { "Most Dismissals as keeper:", $"{mostKeeper}", string.Join(",", keepers) });

                TextWriting.WriteTitle(sb, exportType, "Appearances", DocumentElement.h2);

                List<PlayerAttendanceStatistics> played = SeasonPlayerStats.Select(player => player.Played).ToList();
                played.Sort((x, y) => y.TotalGamesPlayed.CompareTo(x.TotalGamesPlayed));
                TableWriting.WriteTable(sb, exportType, played, headerFirstColumn: false);

                TextWriting.WriteTitle(sb, exportType, "Batting Stats", DocumentElement.h2);
                List<PlayerBattingStatistics> batting = SeasonPlayerStats.Select(player => player.BattingStats).ToList();
                _ = batting.RemoveAll(bat => bat.TotalInnings.Equals(0));
                batting.Sort((x, y) => y.TotalRuns.CompareTo(x.TotalRuns));
                TableWriting.WriteTable(sb, exportType, batting, headerFirstColumn: false);

                TextWriting.WriteTitle(sb, exportType, "Highest Partnerships", DocumentElement.h2);
                TableWriting.WriteTable(sb, exportType, PartnershipsByWicket, headerFirstColumn: false);

                TextWriting.WriteTitle(sb, exportType, "Bowling Stats", DocumentElement.h2);
                List<PlayerBowlingStatistics> bowling = SeasonPlayerStats.Select(player => player.BowlingStats).ToList();
                _ = bowling.RemoveAll(bowl => bowl.TotalOvers.Equals(0));
                bowling.Sort((x, y) => y.TotalWickets.CompareTo(x.TotalWickets));
                TableWriting.WriteTable(sb, exportType, bowling, headerFirstColumn: false);

                TextWriting.WriteTitle(sb, exportType, "Fielding Stats", DocumentElement.h2);
                _ = fielding.RemoveAll(field => field.TotalDismissals.Equals(0));
                fielding.Sort((x, y) => y.TotalDismissals.CompareTo(x.TotalDismissals));
                TableWriting.WriteTable(sb, exportType, fielding, headerFirstColumn: false);

                TextWriting.WriteFooter(sb, exportType);

                return sb;
            }
        }
    }
}
