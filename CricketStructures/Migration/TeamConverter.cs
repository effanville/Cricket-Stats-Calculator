using System;
using System.Collections.Generic;
using System.Linq;

using CricketStructures.Match;
using CricketStructures.Match.Innings;
using CricketStructures.Player;
using CricketStructures.Season;

namespace CricketStructures.Migration
{
    public static class TeamConverter
    {
        internal static CricketTeam Conversion(Cricket.Interfaces.ICricketTeam oldStyleTeam)
        {
            var output = new CricketTeam();
            output.TeamName = oldStyleTeam.TeamName;
            output.HomeLocation = oldStyleTeam.HomeLocation;
            output.TeamPlayers = oldStyleTeam.Players.Select(player => ConvertPlayer(player)).ToList();
            output.TeamSeasons = oldStyleTeam.Seasons.Select(season => ConvertSeason(oldStyleTeam.TeamName, season)).ToList();
            return output;
        }

        private static Wicket ConvertWicket(Cricket.Match.Wicket wicket)
        {
            return (Wicket)((int)wicket);
        }

        private static PlayerName ConvertPlayerName(Cricket.Player.PlayerName player)
        {
            return new PlayerName(player?.Surname, player?.Forename);
        }

        private static CricketPlayer ConvertPlayer(Cricket.Interfaces.ICricketPlayer player)
        {
            return new CricketPlayer(ConvertPlayerName(player.Name));
        }

        private static CricketSeason ConvertSeason(string teamName, Cricket.Interfaces.ICricketSeason season)
        {
            var output = new CricketSeason();
            output.Year = season.Year;
            output.Name = season.Name;
            output.SeasonsMatches = season.Matches.Select(match => ConvertMatch(teamName, match)).ToList();
            return output;
        }

        private static CricketMatch ConvertMatch(string teamName, Cricket.Interfaces.ICricketMatch match)
        {
            var output = new CricketMatch();
            output.MatchData.Date = match.MatchData.Date;
            output.MatchData.HomeTeam = match.MatchData.HomeOrAway == Cricket.Match.Location.Home ? teamName : match.MatchData.Opposition;
            output.MatchData.AwayTeam = match.MatchData.HomeOrAway == Cricket.Match.Location.Home ? match.MatchData.Opposition : teamName;
            output.MatchData.Location = match.MatchData.Place;
            output.MatchData.Type = (MatchType)Enum.Parse(typeof(MatchType), match.MatchData.Type.ToString());
            output.Result = (ResultType)Enum.Parse(typeof(ResultType), match.Result.ToString());
            output.MenOfMatch = new List<PlayerName> { ConvertPlayerName(match.ManOfMatch) };

            output.FirstInnings = match.BattingFirstOrSecond == Cricket.Match.TeamInnings.First
                ? ConvertBattingInnings(match.Batting, teamName, match.MatchData.Opposition)
                : ConvertBowlingInnings(match.Bowling, match.FieldingStats, match.MatchData.Opposition, teamName);
            output.SecondInnings = match.BattingFirstOrSecond == Cricket.Match.TeamInnings.Second
                ? ConvertBattingInnings(match.Batting, teamName, match.MatchData.Opposition)
                : ConvertBowlingInnings(match.Bowling, match.FieldingStats, match.MatchData.Opposition, teamName);
            return output;
        }

        private static CricketInnings ConvertBattingInnings(Cricket.Match.BattingInnings innings, string battingTeam, string fieldingTeam)
        {
            var output = new CricketInnings
            {
                BattingTeam = battingTeam,
                FieldingTeam = fieldingTeam,
                Batting = innings.BattingInfo.Select(info => ConvertBatting(info)).ToList()
            };
            output.InningsExtras.Byes = innings.Extras;
            return output;
            BattingEntry ConvertBatting(Cricket.Match.BattingEntry battingEntry)
            {
                var entry = new BattingEntry(ConvertPlayerName(battingEntry.Name));
                entry.SetScores(
                    ConvertWicket(battingEntry.MethodOut),
                    battingEntry.RunsScored,
                    battingEntry.Order,
                    battingEntry.WicketFellAt,
                    battingEntry.TeamScoreAtWicket,
                    ConvertPlayerName(battingEntry.Fielder),
                    wasKeeper: false,
                    ConvertPlayerName(battingEntry.Bowler));
                return entry;
            }
        }

        private static CricketInnings ConvertBowlingInnings(Cricket.Match.BowlingInnings innings, Cricket.Match.Fielding fielding, string battingTeam, string bowlingTeam)
        {
            var output = new CricketInnings
            {
                BattingTeam = battingTeam,
                FieldingTeam = bowlingTeam,
                Bowling = innings.BowlingInfo.Select(info => ConvertBowling(info)).ToList(),
                Batting = ConvertFielding(fielding)
            };
            output.InningsExtras.Byes = innings.ByesLegByes;
            return output;
            BowlingEntry ConvertBowling(Cricket.Match.BowlingEntry bowlingEntry)
            {
                var entry = new BowlingEntry(ConvertPlayerName(bowlingEntry.Name));
                entry.SetBowling(
                    (Over)bowlingEntry.OversBowled,
                    bowlingEntry.Maidens,
                    bowlingEntry.RunsConceded,
                    bowlingEntry.Wickets,
                    wides: 0,
                    noBalls: 0);
                return entry;
            }

            List<BattingEntry> ConvertFielding(Cricket.Match.Fielding fielding)
            {
                var output = new List<BattingEntry>();
                foreach (var value in fielding.FieldingInfo)
                {
                    int index = 0;
                    int dismissalNumber = 0;
                    while (index < value.Catches)
                    {
                        var battEntry = new BattingEntry(new PlayerName($"opposition{dismissalNumber}", "forename"))
                        {
                            Fielder = ConvertPlayerName(value.Name),
                            MethodOut = Wicket.Caught
                        };
                        output.Add(battEntry);
                        dismissalNumber++;
                        index++;
                    }

                    index = 0;
                    while (index < value.RunOuts)
                    {
                        var battEntry = new BattingEntry(new PlayerName($"opposition{dismissalNumber}", "forename"))
                        {
                            Fielder = ConvertPlayerName(value.Name),
                            MethodOut = Wicket.RunOut
                        };
                        output.Add(battEntry);
                        dismissalNumber++;
                        index++;
                    }

                    index = 0;
                    while (index < value.KeeperCatches)
                    {
                        var battEntry = new BattingEntry(new PlayerName($"opposition{dismissalNumber}", "forename"))
                        {
                            Fielder = ConvertPlayerName(value.Name),
                            MethodOut = Wicket.Caught,
                            WasKeeper = true
                        };
                        output.Add(battEntry);
                        dismissalNumber++;
                        index++;
                    }

                    index = 0;
                    while (index < value.KeeperStumpings)
                    {
                        var battEntry = new BattingEntry(new PlayerName($"opposition{dismissalNumber}", "forename"))
                        {
                            Fielder = ConvertPlayerName(value.Name),
                            MethodOut = Wicket.Stumped,
                            WasKeeper = true
                        };
                        output.Add(battEntry);
                        dismissalNumber++;
                        index++;
                    }
                }
                return output;
            }
        }
    }
}
