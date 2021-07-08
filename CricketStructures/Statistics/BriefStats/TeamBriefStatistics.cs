using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CricketStructures.Interfaces;
using CricketStructures.Match;
using CricketStructures.Match.Innings;
using CricketStructures.Player;
using CricketStructures.Player.Interfaces;
using StructureCommon.FileAccess;

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
            season.CalculateGamesPlayed(matchTypes);

            GamesPlayed += season.GamesPlayed;
            NumberWins += season.NumberWins;
            NumberLosses += season.NumberLosses;
            NumberDraws += season.NumberDraws;
            NumberTies += season.NumberTies;
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

            foreach (PlayerName player in season.Players)
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

        public void ExportStats(string filePath, ExportType exportType)
        {
            try
            {
                StreamWriter streamWriter = new StreamWriter(filePath);
                if (exportType.Equals(ExportType.Html))
                {
                    streamWriter.CreateHTMLHeader("Statistics for team", useColours: true);
                }

                if (SeasonOrAllYear == StatisticsType.AllTimeBrief)
                {
                    streamWriter.WriteTitle(exportType, "All time Brief Statistics", HtmlTag.h1);
                }
                if (SeasonOrAllYear == StatisticsType.SeasonBrief)
                {
                    streamWriter.WriteTitle(exportType, $"For season {SeasonYear.Year}-{SeasonName}", HtmlTag.h1);
                }

                streamWriter.WriteTitle(exportType, "Team Overall", HtmlTag.h2);
                streamWriter.WriteParagraph(exportType, new string[] { "Games Played:", $"{GamesPlayed}" });
                streamWriter.WriteParagraph(exportType, new string[] { "Wins:", $"{NumberWins}" });
                streamWriter.WriteParagraph(exportType, new string[] { "Losses:", $"{NumberLosses}" });
                streamWriter.WriteParagraph(exportType, new string[] { "Draws:", $"{NumberDraws}" });
                streamWriter.WriteParagraph(exportType, new string[] { "Ties:", $"{NumberTies}" });

                (BestBatting Best, PlayerName Name) bestBatting = SeasonPlayerStats.Select(player => (player.BattingStats.Best, player.Name)).Max();
                streamWriter.WriteParagraph(exportType, new string[] { "Best Batting:", bestBatting.Name.ToString(), bestBatting.Best.ToString() });

                (BestBowling BestFigures, PlayerName Name) = SeasonPlayerStats.Select(player => (player.BowlingStats.BestFigures, player.Name)).Max();
                streamWriter.WriteParagraph(exportType, new string[] { "Best Bowling:", Name.ToString(), BestFigures.ToString() });

                List<PlayerFieldingStatistics> fielding = SeasonPlayerStats.Select(player => player.FieldingStats).ToList();
                int mostKeeper = fielding.Max(player => player.TotalKeeperDismissals);
                List<PlayerName> keepers = fielding.Where(player => player.TotalKeeperDismissals.Equals(mostKeeper)).Select(player => player.Name).ToList();
                streamWriter.WriteParagraph(exportType, new string[] { "Most Dismissals as keeper:", $"{mostKeeper}", string.Join(",", keepers) });

                streamWriter.WriteTitle(exportType, "Appearances", HtmlTag.h2);

                List<PlayerAttendanceStatistics> played = SeasonPlayerStats.Select(player => player.Played).ToList();
                played.Sort((x, y) => y.TotalGamesPlayed.CompareTo(x.TotalGamesPlayed));
                FileWritingSupport.WriteTable(streamWriter, exportType, played, headerFirstColumn: false);

                streamWriter.WriteTitle(exportType, "Batting Stats", HtmlTag.h2);
                List<PlayerBattingStatistics> batting = SeasonPlayerStats.Select(player => player.BattingStats).ToList();
                _ = batting.RemoveAll(bat => bat.TotalInnings.Equals(0));
                batting.Sort((x, y) => y.TotalRuns.CompareTo(x.TotalRuns));
                FileWritingSupport.WriteTable(streamWriter, exportType, batting, headerFirstColumn: false);

                streamWriter.WriteTitle(exportType, "Highest Partnerships", HtmlTag.h2);
                FileWritingSupport.WriteTable(streamWriter, exportType, PartnershipsByWicket, headerFirstColumn: false);

                FileWritingSupport.WriteTitle(streamWriter, exportType, "Bowling Stats", HtmlTag.h2);
                List<PlayerBowlingStatistics> bowling = SeasonPlayerStats.Select(player => player.BowlingStats).ToList();
                _ = bowling.RemoveAll(bowl => bowl.TotalOvers.Equals(0));
                bowling.Sort((x, y) => y.TotalWickets.CompareTo(x.TotalWickets));
                FileWritingSupport.WriteTable(streamWriter, exportType, bowling, headerFirstColumn: false);

                streamWriter.WriteTitle(exportType, "Fielding Stats", HtmlTag.h2);
                _ = fielding.RemoveAll(field => field.TotalDismissals.Equals(0));
                fielding.Sort((x, y) => y.TotalDismissals.CompareTo(x.TotalDismissals));
                FileWritingSupport.WriteTable(streamWriter, exportType, fielding, headerFirstColumn: false);

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
