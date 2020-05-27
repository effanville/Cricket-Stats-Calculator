using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cricket.Interfaces;

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
        {
            SeasonOrAllYear = StatisticsType.AllTimeBrief;
            CalculateTeamStats(team);
            CalculatePlayerStats(team);
            CalculatePartnerships(team);
        }
        public TeamBriefStatistics(ICricketSeason season)
        {
            SeasonOrAllYear = StatisticsType.SeasonBrief;
            SeasonName = season.Name;
            SeasonYear = season.Year;
            CalculateTeamStats(season);
            CalculatePlayerStats(season);
            CalculatePartnerships(season);
        }

        public void CalculateTeamStats(ICricketTeam team)
        {
            foreach (ICricketSeason season in team.Seasons)
            {
                CalculateTeamStats(season);
            }
        }

        public void CalculateTeamStats(ICricketSeason season)
        {
            if (SeasonOrAllYear == StatisticsType.SeasonBrief && !season.Year.Equals(SeasonYear))
            {
                return;
            }
            season.CalculateGamesPlayed();

            GamesPlayed += season.GamesPlayed;
            NumberWins += season.NumberWins;
            NumberLosses += season.NumberLosses;
            NumberDraws += season.NumberDraws;
            NumberTies += season.NumberTies;
        }

        public void CalculatePlayerStats(ICricketTeam team)
        {
            foreach (ICricketPlayer player in team.Players)
            {
                PlayerBriefStatistics playerStats = new PlayerBriefStatistics(player.Name, team);
                SeasonPlayerStats.Add(playerStats);
            }
        }

        public void CalculatePlayerStats(ICricketSeason season)
        {
            if (!season.Year.Equals(SeasonYear))
            {
                return;
            }

            foreach (Player.PlayerName player in season.Players)
            {
                PlayerBriefStatistics playerStats = new PlayerBriefStatistics(player, season);
                SeasonPlayerStats.Add(playerStats);
            }
        }

        public void CalculatePartnerships(ICricketTeam team)
        {
            foreach (ICricketSeason season in team.Seasons)
            {
                CalculatePartnerships(season);
            }
        }

        public void CalculatePartnerships(ICricketSeason season)
        {
            foreach (ICricketMatch match in season.Matches)
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

        public void ExportStats(string filePath, ExportType exportType)
        {
            try
            {
                StreamWriter streamWriter = new StreamWriter(filePath);
                if (exportType.Equals(ExportType.Html))
                {
                    streamWriter.CreateHTMLHeader("Statistics for team");
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
                FileWritingSupport.WriteTable(streamWriter, exportType, new PlayerAttendanceStatistics().GetType().GetProperties().Select(type => type.Name), played);

                streamWriter.WriteTitle(exportType, "Batting Stats", HtmlTag.h2);
                List<PlayerBattingStatistics> batting = SeasonPlayerStats.Select(player => player.BattingStats).ToList();
                batting.RemoveAll(bat => bat.TotalInnings.Equals(0));
                batting.Sort((x, y) => y.TotalRuns.CompareTo(x.TotalRuns));
                FileWritingSupport.WriteTable(streamWriter, exportType, new PlayerBattingStatistics().GetType().GetProperties().Select(type => type.Name), batting);

                streamWriter.WriteTitle(exportType, "Highest Partnerships", HtmlTag.h2);
                FileWritingSupport.WriteTable(streamWriter, exportType, new Partnership().GetType().GetProperties().Select(type => type.Name), PartnershipsByWicket);

                FileWritingSupport.WriteTitle(streamWriter, exportType, "Bowling Stats", HtmlTag.h2);
                List<PlayerBowlingStatistics> bowling = SeasonPlayerStats.Select(player => player.BowlingStats).ToList();
                bowling.RemoveAll(bowl => bowl.TotalOvers.Equals(0));
                bowling.Sort((x, y) => y.TotalWickets.CompareTo(x.TotalWickets));
                FileWritingSupport.WriteTable(streamWriter, exportType, new PlayerBowlingStatistics().GetType().GetProperties().Select(type => type.Name), bowling);

                streamWriter.WriteTitle(exportType, "Fielding Stats", HtmlTag.h2);
                fielding.RemoveAll(field => field.TotalDismissals.Equals(0));
                fielding.Sort((x, y) => y.TotalDismissals.CompareTo(x.TotalDismissals));
                FileWritingSupport.WriteTable(streamWriter, exportType, new PlayerFieldingStatistics().GetType().GetProperties().Select(type => type.Name), fielding);

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
