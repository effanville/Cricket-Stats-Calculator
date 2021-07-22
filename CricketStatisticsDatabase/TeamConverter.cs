using System;
using System.Collections.Generic;
using System.Linq;
using Cricket;
using Cricket.Match;
using Cricket.Player;
using Cricket.Team;

namespace CricketStatisticsDatabase
{
    public static class TeamConverter
    {
        public static CricketStructures.CricketTeam Conversion(CricketTeam oldStyleTeam)
        {
            var output = new CricketStructures.CricketTeam();
            output.TeamName = oldStyleTeam.TeamName;
            output.HomeLocation = oldStyleTeam.HomeLocation;
            output.TeamPlayers = oldStyleTeam.TeamPlayers.Select(player => ConvertPlayer(player)).ToList();
            output.TeamSeasons = oldStyleTeam.TeamSeasons.Select(season => ConvertSeason(oldStyleTeam.TeamName, season)).ToList();
            return output;
        }

        private static CricketStructures.Match.Wicket ConvertWicket(Wicket wicket)
        {
            return (CricketStructures.Match.Wicket)((int)wicket);
        }

        private static CricketStructures.Player.PlayerName ConvertPlayerName(PlayerName player)
        {
            return new CricketStructures.Player.PlayerName(player?.Surname, player?.Forename);
        }
        private static CricketStructures.Player.CricketPlayer ConvertPlayer(CricketPlayer player)
        {
            return new CricketStructures.Player.CricketPlayer(ConvertPlayerName(player.Name));
        }

        private static CricketStructures.Season.CricketSeason ConvertSeason(string teamName, CricketSeason season)
        {
            var output = new CricketStructures.Season.CricketSeason();
            output.Year = season.Year;
            output.Name = season.Name;
            output.SeasonsMatches = season.SeasonsMatches.Select(match => ConvertMatch(teamName, match)).ToList();
            return output;
        }

        private static CricketStructures.Match.CricketMatch ConvertMatch(string teamName, CricketMatch match)
        {
            var output = new CricketStructures.Match.CricketMatch();
            output.MatchData.Date = match.MatchData.Date;
            output.MatchData.HomeTeam = match.MatchData.HomeOrAway == Location.Home ? teamName : match.MatchData.Opposition;
            output.MatchData.AwayTeam = match.MatchData.HomeOrAway == Location.Home ? match.MatchData.Opposition : teamName;
            output.MatchData.Location = match.MatchData.Place;
            output.MatchData.Type = (CricketStructures.Match.MatchType)Enum.Parse(typeof(CricketStructures.Match.MatchType), match.MatchData.Type.ToString());
            output.Result = (CricketStructures.Match.ResultType)Enum.Parse(typeof(CricketStructures.Match.ResultType), match.Result.ToString());
            output.MenOfMatch = new CricketStructures.Player.PlayerName[] { ConvertPlayerName(match.ManOfMatch) };

            output.FirstInnings = match.BattingFirstOrSecond == TeamInnings.First ? ConvertBattingInnings(match.Batting) : ConvertBowlingInnings(match.Bowling, match.FieldingStats);
            output.SecondInnings = match.BattingFirstOrSecond == TeamInnings.Second ? ConvertBattingInnings(match.Batting) : ConvertBowlingInnings(match.Bowling, match.FieldingStats);
            return output;
        }

        private static CricketStructures.Match.Innings.CricketInnings ConvertBattingInnings(BattingInnings innings)
        {
            var output = new CricketStructures.Match.Innings.CricketInnings();
            output.Batting = innings.BattingInfo.Select(info => ConvertBatting(info)).ToList();
            output.InningsExtras.Byes = innings.Extras;
            return output;
            CricketStructures.Match.Innings.BattingEntry ConvertBatting(BattingEntry battingEntry)
            {
                var entry = new CricketStructures.Match.Innings.BattingEntry(ConvertPlayerName(battingEntry.Name));
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

        private static CricketStructures.Match.Innings.CricketInnings ConvertBowlingInnings(BowlingInnings innings, Fielding fielding)
        {
            var output = new CricketStructures.Match.Innings.CricketInnings();
            output.Bowling = innings.BowlingInfo.Select(info => ConvertBowling(info)).ToList();
            output.Batting = ConvertFielding(fielding);
            output.InningsExtras.Byes = innings.ByesLegByes;
            return output;
            CricketStructures.Match.Innings.BowlingEntry ConvertBowling(BowlingEntry bowlingEntry)
            {
                var entry = new CricketStructures.Match.Innings.BowlingEntry(ConvertPlayerName(bowlingEntry.Name));
                entry.SetBowling(
                    bowlingEntry.OversBowled,
                    bowlingEntry.Maidens,
                    bowlingEntry.RunsConceded,
                    bowlingEntry.Wickets,
                    wides: 0,
                    noBalls: 0);
                return entry;
            }

            List<CricketStructures.Match.Innings.BattingEntry> ConvertFielding(Fielding fielding)
            {
                var output = new List<CricketStructures.Match.Innings.BattingEntry>();
                foreach (var value in fielding.FieldingInfo)
                {
                    int index = 0;
                    int dismissalNumber = 0;
                    while (index < value.Catches)
                    {
                        var battEntry = new CricketStructures.Match.Innings.BattingEntry(new CricketStructures.Player.PlayerName($"opposition{dismissalNumber}", "forename"));
                        battEntry.Fielder = ConvertPlayerName(value.Name);
                        battEntry.MethodOut = CricketStructures.Match.Wicket.Caught;
                        output.Add(battEntry);
                        dismissalNumber++;
                        index++;
                    }

                    index = 0;
                    while (index < value.RunOuts)
                    {
                        var battEntry = new CricketStructures.Match.Innings.BattingEntry(new CricketStructures.Player.PlayerName($"opposition{dismissalNumber}", "forename"));
                        battEntry.Fielder = ConvertPlayerName(value.Name);
                        battEntry.MethodOut = CricketStructures.Match.Wicket.RunOut;
                        output.Add(battEntry);
                        dismissalNumber++;
                        index++;
                    }

                    index = 0;
                    while (index < value.KeeperCatches)
                    {
                        var battEntry = new CricketStructures.Match.Innings.BattingEntry(new CricketStructures.Player.PlayerName($"opposition{dismissalNumber}", "forename"));
                        battEntry.Fielder = ConvertPlayerName(value.Name);
                        battEntry.MethodOut = CricketStructures.Match.Wicket.Caught;
                        battEntry.WasKeeper = true;
                        output.Add(battEntry);
                        dismissalNumber++;
                        index++;
                    }

                    index = 0;
                    while (index < value.KeeperStumpings)
                    {
                        var battEntry = new CricketStructures.Match.Innings.BattingEntry(new CricketStructures.Player.PlayerName($"opposition{dismissalNumber}", "forename"));
                        battEntry.Fielder = ConvertPlayerName(value.Name);
                        battEntry.MethodOut = CricketStructures.Match.Wicket.Stumped;
                        battEntry.WasKeeper = true;
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
