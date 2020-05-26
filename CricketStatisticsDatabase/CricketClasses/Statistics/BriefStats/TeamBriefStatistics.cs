using Cricket.Interfaces;
using Cricket.Statistics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
            foreach (var season in team.Seasons)
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
            foreach (var player in team.Players)
            {
                var playerStats = new PlayerBriefStatistics(player.Name, team);
                SeasonPlayerStats.Add(playerStats);
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
                var playerStats = new PlayerBriefStatistics(player, season);
                SeasonPlayerStats.Add(playerStats);
            }
        }

        public void CalculatePartnerships(ICricketTeam team)
        {
            foreach (var season in team.Seasons)
            {
                CalculatePartnerships(season);
            }
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

                var bestBatting = SeasonPlayerStats.Select(player => (player.BattingStats.Best, player.Name)).Max();
                streamWriter.WriteParagraph(exportType, new string[] { "Best Batting:", bestBatting.Name.ToString(), bestBatting.Best.ToString() });

                var bestBowling = SeasonPlayerStats.Select(player => (player.BowlingStats.BestFigures, player.Name)).Max();
                streamWriter.WriteParagraph(exportType, new string[] { "Best Bowling:", bestBowling.Name.ToString(), bestBowling.BestFigures.ToString() });

                var fielding = SeasonPlayerStats.Select(player => player.FieldingStats).ToList();
                var mostKeeper = fielding.Max(player => player.TotalKeeperDismissals);
                var keepers = fielding.Where(player => player.TotalKeeperDismissals.Equals(mostKeeper)).Select(player => player.Name).ToList();
                streamWriter.WriteParagraph(exportType, new string[] { "Most Dismissals as keeper:", $"{mostKeeper}", string.Join(",", keepers) });

                streamWriter.WriteTitle(exportType, "Appearances", HtmlTag.h2);

                var played = SeasonPlayerStats.Select(player => player.Played).ToList();
                played.Sort((x, y) => y.TotalGamesPlayed.CompareTo(x.TotalGamesPlayed));
                FileWritingSupport.WriteTable(streamWriter, exportType, new PlayerAttendanceStatistics().GetType().GetProperties().Select(type => type.Name), played);

                streamWriter.WriteTitle(exportType, "Batting Stats", HtmlTag.h2);
                var batting = SeasonPlayerStats.Select(player => player.BattingStats).ToList();
                batting.RemoveAll(bat => bat.TotalInnings.Equals(0));
                batting.Sort((x, y) => y.TotalRuns.CompareTo(x.TotalRuns));
                FileWritingSupport.WriteTable(streamWriter, exportType, new PlayerBattingStatistics().GetType().GetProperties().Select(type => type.Name), batting);

                streamWriter.WriteTitle(exportType, "Highest Partnerships", HtmlTag.h2);
                FileWritingSupport.WriteTable(streamWriter, exportType, new Partnership().GetType().GetProperties().Select(type => type.Name), PartnershipsByWicket);

                FileWritingSupport.WriteTitle(streamWriter, exportType, "Bowling Stats", HtmlTag.h2);
                var bowling = SeasonPlayerStats.Select(player => player.BowlingStats).ToList();
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
