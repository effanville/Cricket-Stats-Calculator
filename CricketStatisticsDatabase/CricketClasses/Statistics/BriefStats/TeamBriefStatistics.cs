using Cricket.Interfaces;
using Cricket.Statistics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CricketStatistics
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
            SeasonOrAllYear = StatisticsType.Alltime;
            CalculateTeamStats(team);
            CalculatePlayerStats(team);
            CalculatePartnerships(team);
        }
        public TeamBriefStatistics(ICricketSeason season)
        {
            SeasonOrAllYear = StatisticsType.Season;
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
            if (!season.Year.Equals(SeasonYear))
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

        public void ExportStats(string filePath)
        {
            switch (SeasonOrAllYear)
            {
                case StatisticsType.Season:
                {
                    ExportSeasonStats(filePath);
                    break;
                }
                case StatisticsType.Alltime:
                {
                    ExportAllTimeStats(filePath);
                    break;
                }
            }
        }

        private void ExportSeasonStats(string filePath)
        {
            try
            {
                StreamWriter streamWriter = new StreamWriter(filePath);
                streamWriter.WriteLine($"Exporting Team {1 + 1}");
                streamWriter.WriteLine($"For season {SeasonYear.Year}-{SeasonName}");

                streamWriter.WriteLine("");

                streamWriter.WriteLine("Team Overall");
                streamWriter.WriteLine($"Games Played:, {GamesPlayed}");

                streamWriter.WriteLine($"Wins:, {NumberWins}");
                streamWriter.WriteLine($"Losses:, {NumberLosses}");
                streamWriter.WriteLine($"Draws:, {NumberDraws}");
                streamWriter.WriteLine($"Ties:, {NumberTies}");

                var bestBatting = SeasonPlayerStats.Select(player => (player.BattingStats.Best, player.Name)).Max();

                streamWriter.WriteLine("Best Batting," + bestBatting.Name.ToString() + "," + bestBatting.Best.ToString());

                var bestBowling = SeasonPlayerStats.Select(player => (player.BowlingStats.BestFigures, player.Name)).Max();

                streamWriter.WriteLine("Best Bowling," + bestBowling.Name.ToString() + "," + bestBowling.BestFigures.ToString());

                var fielding = SeasonPlayerStats.Select(player => player.FieldingStats).ToList();
                var mostKeeper = fielding.Max(player => player.TotalKeeperDismissals);
                var keepers = fielding.Where(player => player.TotalKeeperDismissals.Equals(mostKeeper)).Select(player => player.Name).ToList();
                streamWriter.WriteLine("Most Dismissals as keeper," + mostKeeper + "," + string.Join(",", keepers));
                streamWriter.WriteLine("");

                streamWriter.WriteLine("");
                streamWriter.WriteLine("Attendance");
                streamWriter.WriteLine("");
                var played = SeasonPlayerStats.Select(player => player.Played).ToList();
                played.Sort((x, y) => y.TotalGamesPlayed.CompareTo(x.TotalGamesPlayed));
                streamWriter.WriteLine(PlayerAttendanceStatistics.CsvHeader());
                foreach (var playing in played)
                {
                    streamWriter.WriteLine(playing.ToString());
                }

                streamWriter.WriteLine("");
                streamWriter.WriteLine("Batting Stats");

                streamWriter.WriteLine("");

                var batting = SeasonPlayerStats.Select(player => player.BattingStats).ToList();
                batting.RemoveAll(bat => bat.TotalInnings.Equals(0));

                batting.Sort((x, y) => y.TotalRuns.CompareTo(x.TotalRuns));
                streamWriter.WriteLine(PlayerBattingStatistics.CsvHeader());
                foreach (var bat in batting)
                {
                    streamWriter.WriteLine(bat.ToString());
                }

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

                var bowling = SeasonPlayerStats.Select(player => player.BowlingStats).ToList();
                bowling.RemoveAll(bowl => bowl.TotalOvers.Equals(0));
                bowling.Sort((x, y) => y.TotalWickets.CompareTo(x.TotalWickets));
                streamWriter.WriteLine(PlayerBowlingStatistics.CsvHeader());
                foreach (var bowl in bowling)
                {
                    streamWriter.WriteLine(bowl.ToString());
                }

                streamWriter.WriteLine("");
                streamWriter.WriteLine("Fielding Stats");
                streamWriter.WriteLine("");

                fielding.RemoveAll(field => field.TotalDismissals.Equals(0));
                fielding.Sort((x, y) => y.TotalDismissals.CompareTo(x.TotalDismissals));
                streamWriter.WriteLine(PlayerFieldingStatistics.CsvHeader());
                foreach (var field in fielding)
                {
                    streamWriter.WriteLine(field.ToString());
                }

                streamWriter.Close();
            }
            catch (Exception ex)
            {
            }
        }

        private void ExportAllTimeStats(string filePath)
        {
            try
            {
                StreamWriter streamWriter = new StreamWriter(filePath);
                streamWriter.WriteLine($"Exporting Team {1 + 1}");

                streamWriter.WriteLine("");

                streamWriter.WriteLine("Team Overall");
                streamWriter.WriteLine($"Games Played:, {GamesPlayed}");

                streamWriter.WriteLine($"Wins:, {NumberWins}");
                streamWriter.WriteLine($"Losses:, {NumberLosses}");
                streamWriter.WriteLine($"Draws:, {NumberDraws}");
                streamWriter.WriteLine($"Ties:, {NumberTies}");

                var bestBatting = SeasonPlayerStats.Select(player => (player.BattingStats.Best, player.Name)).Max();

                streamWriter.WriteLine("Best Batting," + bestBatting.Name.ToString() + "," + bestBatting.Best.ToString());

                var bestBowling = SeasonPlayerStats.Select(player => (player.BowlingStats.BestFigures, player.Name)).Max();

                streamWriter.WriteLine("Best Bowling," + bestBowling.Name.ToString() + "," + bestBowling.BestFigures.ToString());

                var fielding = SeasonPlayerStats.Select(player => player.FieldingStats).ToList();
                var mostKeeper = fielding.Max(player => player.TotalKeeperDismissals);
                var keepers = fielding.Where(player => player.TotalKeeperDismissals.Equals(mostKeeper)).Select(player => player.Name).ToList();
                streamWriter.WriteLine("Most Dismissals as keeper," + mostKeeper + "," + string.Join(",", keepers));
                streamWriter.WriteLine("");

                streamWriter.WriteLine("");
                streamWriter.WriteLine("Attendance");
                streamWriter.WriteLine("");
                var played = SeasonPlayerStats.Select(player => player.Played).ToList();
                played.Sort((x, y) => y.TotalGamesPlayed.CompareTo(x.TotalGamesPlayed));
                streamWriter.WriteLine(PlayerAttendanceStatistics.CsvHeader());
                foreach (var playing in played)
                {
                    streamWriter.WriteLine(playing.ToString());
                }

                streamWriter.WriteLine("");
                streamWriter.WriteLine("Batting Stats");

                streamWriter.WriteLine("");

                var batting = SeasonPlayerStats.Select(player => player.BattingStats).ToList();
                batting.RemoveAll(bat => bat.TotalInnings.Equals(0));

                batting.Sort((x, y) => y.TotalRuns.CompareTo(x.TotalRuns));
                streamWriter.WriteLine(PlayerBattingStatistics.CsvHeader());
                foreach (var bat in batting)
                {
                    streamWriter.WriteLine(bat.ToString());
                }

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

                var bowling = SeasonPlayerStats.Select(player => player.BowlingStats).ToList();
                bowling.RemoveAll(bowl => bowl.TotalOvers.Equals(0));
                bowling.Sort((x, y) => y.TotalWickets.CompareTo(x.TotalWickets));
                streamWriter.WriteLine(PlayerBowlingStatistics.CsvHeader());
                foreach (var bowl in bowling)
                {
                    streamWriter.WriteLine(bowl.ToString());
                }

                streamWriter.WriteLine("");
                streamWriter.WriteLine("Fielding Stats");
                streamWriter.WriteLine("");

                fielding.RemoveAll(field => field.TotalDismissals.Equals(0));
                fielding.Sort((x, y) => y.TotalDismissals.CompareTo(x.TotalDismissals));
                streamWriter.WriteLine(PlayerFieldingStatistics.CsvHeader());
                foreach (var field in fielding)
                {
                    streamWriter.WriteLine(field.ToString());
                }

                streamWriter.Close();
            }
            catch (Exception ex)
            {
            }
        }
    }
}
