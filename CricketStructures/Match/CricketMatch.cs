using System;
using System.Collections.Generic;
using System.Linq;
using CricketStructures.Interfaces;
using CricketStructures.Player;
using StructureCommon.Extensions;
using StructureCommon.Validation;
using CricketStructures.Match.Innings;

namespace CricketStructures.Match
{
    public sealed class CricketMatch : ICricketMatch, IValidity
    {
        /// <inheritdoc/>
        public MatchInfo MatchData
        {
            get;
            set;
        }

        /// <inheritdoc/>
        public ResultType Result
        {
            get;
            set;
        }

        /// <inheritdoc/>
        public PlayerName[] MenOfMatch
        {
            get;
            set;
        }

        /// <inheritdoc/>
        public CricketInnings FirstInnings
        {
            get;
            set;
        }

        /// <inheritdoc/>
        public CricketInnings SecondInnings
        {
            get;
            set;
        }

        /// <summary>
        /// default generator of match
        /// </summary>
        public CricketMatch(string teamName, string homeTeam, string awayTeam, DateTime date, bool battingFirst)
        {
            MatchData = new MatchInfo()
            {
                HomeTeam = homeTeam,
                AwayTeam = awayTeam,
                Date = date
            };

            string team = teamName.Equals(homeTeam) ? homeTeam : awayTeam;
            string opposition = !teamName.Equals(homeTeam) ? homeTeam : awayTeam;
            FirstInnings = new CricketInnings(battingFirst ? team : opposition, battingFirst ? opposition : team);
            SecondInnings = new CricketInnings(battingFirst ? opposition : team, battingFirst ? team : opposition);
        }

        public CricketMatch(MatchInfo info)
        {
            MatchData = info;
            FirstInnings = new CricketInnings();
            SecondInnings = new CricketInnings();
        }

        public CricketMatch()
        {
        }

        public event EventHandler PlayerAdded;

        private void OnPlayerAdded(PlayerName newPlayerName)
        {
            PlayerAdded?.Invoke(newPlayerName, new EventArgs());
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return MatchData.ToString();
        }

        /// <inheritdoc/>
        public void EditPlayerName(PlayerName oldName, PlayerName newName)
        {
            FirstInnings.EditPlayerName(oldName, newName);
            SecondInnings.EditPlayerName(oldName, newName);
        }

        /// <inheritdoc/>
        public bool PlayNotPlay(string team, PlayerName person)
        {
            return Players(team).Contains(person);
        }

        /// <inheritdoc/>
        public bool SameMatch(DateTime date, string homeTeam, string awayTeam)
        {
            return MatchData.Equals(date, homeTeam, awayTeam);
        }

        /// <inheritdoc/>
        public List<PlayerName> Players(string team)
        {
            string teamName = team;
            var players = new HashSet<PlayerName>();
            players.UnionWith(FirstInnings.Players(teamName));
            players.UnionWith(SecondInnings.Players(teamName));
            return players.ToList();
        }

        /// <inheritdoc/>
        public void EditInfo(string homeTeam = null, string awayTeam = null, DateTime? date = null, string location = null, MatchType? typeOfMatch = null, ResultType? result = null)
        {
            if (!string.IsNullOrEmpty(homeTeam))
            {
                MatchData.HomeTeam = homeTeam;
            }
            if (!string.IsNullOrEmpty(awayTeam))
            {
                MatchData.AwayTeam = awayTeam;
            }
            if (date.HasValue)
            {
                MatchData.Date = date.Value;
            }
            if (!string.IsNullOrEmpty(location))
            {
                MatchData.Location = location;
            }
            if (typeOfMatch.HasValue)
            {
                MatchData.Type = typeOfMatch.Value;
            }
            if (result.HasValue)
            {
                Result = result.Value;
            }
        }

        /// <inheritdoc/>
        public bool EditManOfMatch(PlayerName[] player)
        {
            MenOfMatch = player;
            return true;
        }

        /// <inheritdoc/>
        public void SetBatting(string team, PlayerName player, Wicket howOut, int runs, int order, int wicketFellAt, int teamScoreAtWicket, PlayerName fielder = null, bool wasKeeper = false, PlayerName bowler = null)
        {
            var innings = InningsHelpers.SelectBattingInnings(FirstInnings, SecondInnings, team);
            if (innings != null)
            {
                innings.SetBatting(player, howOut, runs, order, wicketFellAt, teamScoreAtWicket, fielder, wasKeeper, bowler);
            }
        }

        internal void SetInnings(CricketInnings innings, bool first)
        {
            // TODO add checks to ensure the innings has the correct teams.
            if (first)
            {
                FirstInnings = innings;
            }
            else
            {
                SecondInnings = innings;
            }
        }

        /// <inheritdoc/>
        public bool DeleteBattingEntry(string team, PlayerName player)
        {

            var innings = InningsHelpers.SelectBattingInnings(FirstInnings, SecondInnings, team);
            if (innings != null)
            {
                return innings.DeleteBatting(player);
            }

            return false;
        }

        /// <inheritdoc/>
        public void SetBowling(string team, PlayerName player, double overs, int maidens, int runsConceded, int wickets, int wides = 0, int noBalls = 0)
        {
            var innings = InningsHelpers.SelectFieldingInnings(FirstInnings, SecondInnings, team);
            if (innings != null)
            {
                innings.SetBowling(player, overs, maidens, runsConceded, wickets);
            }
        }

        /// <inheritdoc/>
        public bool DeleteBowlingEntry(string team, PlayerName player)
        {
            var innings = InningsHelpers.SelectFieldingInnings(FirstInnings, SecondInnings, team);
            if (innings != null)
            {
                return innings.DeleteBowling(player);
            }

            return false;
        }

        /// <inheritdoc/>
        public CricketInnings GetInnings(string team, bool batting)
        {
            return InningsHelpers.SelectInnings(FirstInnings, SecondInnings, innings => batting ? innings.BattingTeam.Equals(team) : innings.FieldingTeam.Equals(team));
        }

        public InningsScore Score(string team)
        {
            var innings = InningsHelpers.SelectBattingInnings(FirstInnings, SecondInnings, team);
            return innings.Score();
        }

        /// <inheritdoc/>
        public bool BattedFirst(string team)
        {
            return FirstInnings == GetInnings(team, batting: true);
        }

        /// <inheritdoc/>
        public BattingEntry GetBatting(string team, PlayerName player)
        {
            var innings = InningsHelpers.SelectBattingInnings(FirstInnings, SecondInnings, team);
            if (innings != null)
            {
                if (innings.IsBattingPlayer(player))
                {
                    return innings.GetBatting(team, player);
                }
            }

            return null;
        }

        /// <inheritdoc/>
        public BowlingEntry GetBowling(string team, PlayerName player)
        {
            var innings = InningsHelpers.SelectFieldingInnings(FirstInnings, SecondInnings, team);
            if (innings != null)
            {
                if (innings.IsBattingPlayer(player))
                {
                    return innings.GetBowling(team, player);
                }
            }

            return null;
        }

        /// <inheritdoc/>
        public FieldingEntry GetFielding(string team, PlayerName player)
        {
            var innings = InningsHelpers.SelectFieldingInnings(FirstInnings, SecondInnings, team);
            if (innings != null)
            {
                if (innings.IsFieldingPlayer(player))
                {
                    return innings.GetFielding(team, player);
                }
            }

            return null;
        }

        /// <inheritdoc/>
        public IReadOnlyList<FieldingEntry> GetAllFielding(string team)
        {
            var fielding = new List<FieldingEntry>();
            var innings = InningsHelpers.SelectFieldingInnings(FirstInnings, SecondInnings, team);
            if (innings != null)
            {

            }

            return fielding;
        }

        /// <inheritdoc/>
        public bool Validate()
        {
            return !Validation().Any(validation => !validation.IsValid);
        }

        /// <inheritdoc/>
        public List<ValidationResult> Validation()
        {
            List<ValidationResult> results = new List<ValidationResult>();
            results.AddValidations(MatchData.Validation(), ToString());
            results.AddValidations(FirstInnings.Validation(), ToString());
            results.AddValidations(SecondInnings.Validation(), ToString());
            return results;
        }

        /// <inheritdoc/>
        public List<Partnership> Partnerships(string team)
        {
            var innings = InningsHelpers.SelectBattingInnings(FirstInnings, SecondInnings, team);
            if (innings != null)
            {
                List<Partnership> partnerships = innings.Partnerships();
                foreach (Partnership ship in partnerships)
                {
                    if (ship != null && ship.MatchData == null)
                    {
                        ship.MatchData = MatchData;
                    }
                }

                return partnerships;
            }

            return null;
        }
    }

    public static class InningsHelpers
    {
        public static CricketInnings SelectBattingInnings(CricketInnings firstInnings, CricketInnings secondInnings, string battingTeam)
        {
            return SelectInnings(firstInnings, secondInnings, innings => innings.BattingTeam.Equals(battingTeam));
        }

        public static CricketInnings SelectFieldingInnings(CricketInnings firstInnings, CricketInnings secondInnings, string fieldingTeam)
        {
            return SelectInnings(firstInnings, secondInnings, innings => innings.FieldingTeam.Equals(fieldingTeam));
        }

        public static CricketInnings SelectInnings(CricketInnings firstInnings, CricketInnings secondInnings, Func<CricketInnings, bool> teamSelector)
        {
            if (teamSelector(firstInnings))
            {
                return firstInnings;
            }
            if (teamSelector(secondInnings))
            {
                return secondInnings;
            }

            return null;
        }
    }
}
