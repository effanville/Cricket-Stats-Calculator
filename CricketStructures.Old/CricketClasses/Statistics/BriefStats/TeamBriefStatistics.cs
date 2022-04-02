using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cricket.Interfaces;
using Cricket.Match;
using Common.Structure.FileAccess;
using Common.Structure.ReportWriting;

namespace Cricket.Statistics
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

        public TeamBriefStatistics(ICricketTeam team, Cricket.Match.MatchType[] matcheTypes)
        {
            SeasonOrAllYear = StatisticsType.AllTimeBrief;
            CalculateTeamStats(team, matcheTypes);
            CalculatePlayerStats(team, matcheTypes);
            CalculatePartnerships(team, matcheTypes);
        }

        public TeamBriefStatistics(ICricketSeason season, Cricket.Match.MatchType[] matcheTypes)
        {
            SeasonOrAllYear = StatisticsType.SeasonBrief;
            SeasonName = season.Name;
            SeasonYear = season.Year;
            CalculateTeamStats(season, matcheTypes);
            CalculatePlayerStats(season, matcheTypes);
            CalculatePartnerships(season, matcheTypes);
        }

        public TeamBriefStatistics(ICricketSeason season)
            : this(season, MatchHelpers.AllMatchTypes)
        {
        }

        public void CalculateTeamStats(ICricketTeam team, Cricket.Match.MatchType[] matchTypes)
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

        public void CalculateTeamStats(ICricketSeason season, Cricket.Match.MatchType[] matchTypes)
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

        public void CalculatePlayerStats(ICricketTeam team, Cricket.Match.MatchType[] matchTypes)
        {
            foreach (ICricketPlayer player in team.Players)
            {
                PlayerBriefStatistics playerStats = new PlayerBriefStatistics(player.Name, team, matchTypes);
                SeasonPlayerStats.Add(playerStats);
            }
        }

        public void CalculatePlayerStats(ICricketSeason season, Cricket.Match.MatchType[] matchTypes)
        {
            if (!season.Year.Equals(SeasonYear))
            {
                return;
            }

            foreach (Player.PlayerName player in season.Players)
            {
                PlayerBriefStatistics playerStats = new PlayerBriefStatistics(player, season, matchTypes);
                SeasonPlayerStats.Add(playerStats);
            }
        }

        public void CalculatePartnerships(ICricketTeam team, Cricket.Match.MatchType[] matchTypes)
        {
            foreach (ICricketSeason season in team.Seasons)
            {
                CalculatePartnerships(season, matchTypes);
            }
        }

        public void CalculatePartnerships(ICricketSeason season, Cricket.Match.MatchType[] matchTypes)
        {
            foreach (ICricketMatch match in season.Matches)
            {
                if (matchTypes.Contains(match.MatchData.Type))
                {
                    List<Partnership> partnerships = match.Partnerships();
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

                (BestBatting Best, Player.PlayerName Name) bestBatting = SeasonPlayerStats.Select(player => (player.BattingStats.Best, player.Name)).Max();
                streamWriter.WriteParagraph(exportType, new string[] { "Best Batting:", bestBatting.Name.ToString(), bestBatting.Best.ToString() });

                (BestBowling BestFigures, Player.PlayerName Name) bestBowling = SeasonPlayerStats.Select(player => (player.BowlingStats.BestFigures, player.Name)).Max();
                streamWriter.WriteParagraph(exportType, new string[] { "Best Bowling:", bestBowling.Name.ToString(), bestBowling.BestFigures.ToString() });

                List<PlayerFieldingStatistics> fielding = SeasonPlayerStats.Select(player => player.FieldingStats).ToList();
                int mostKeeper = fielding.Max(player => player.TotalKeeperDismissals);
                List<Player.PlayerName> keepers = fielding.Where(player => player.TotalKeeperDismissals.Equals(mostKeeper)).Select(player => player.Name).ToList();
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
