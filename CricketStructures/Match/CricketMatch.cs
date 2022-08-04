using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using CricketStructures.Match.Innings;
using CricketStructures.Player;
using Common.Structure.Extensions;
using Common.Structure.Validation;
using Common.Structure.ReportWriting;
using CricketStructures.Match.Result;

namespace CricketStructures.Match
{
    public sealed class CricketMatch : ICricketMatch, IValidity
    {
        [XmlAttribute(AttributeName = "H")]
        public string HomeTeam
        {
            get => MatchData.HomeTeam;
            set => MatchData.HomeTeam = value;
        }

        [XmlAttribute(AttributeName = "A")]
        public string AwayTeam
        {
            get => MatchData.AwayTeam;
            set => MatchData.AwayTeam = value;
        }

        [XmlAttribute(AttributeName = "L")]
        public string Location
        {
            get => MatchData.Location;
            set => MatchData.Location = value;
        }

        [XmlAttribute(AttributeName = "D")]
        public DateTime Date
        {
            get => MatchData.Date;
            set => MatchData.Date = value;
        }

        [XmlAttribute(AttributeName = "T")]
        public MatchType Type
        {
            get => MatchData.Type;
            set => MatchData.Type = value;
        }

        /// <inheritdoc/>
        [XmlIgnore]
        public MatchInfo MatchData
        {
            get;
            set;
        }

        /// <inheritdoc/>
        [XmlElement]
        public ResultType Result
        {
            get;
            set;
        }

        /// <inheritdoc/>
        [XmlArray]
        public PlayerName[] MenOfMatch
        {
            get;
            set;
        }

        /// <inheritdoc/>
        [XmlElement]
        public CricketInnings FirstInnings
        {
            get;
            set;
        }

        /// <inheritdoc/>
        [XmlElement]
        public CricketInnings SecondInnings
        {
            get;
            set;
        }

        /// <summary>
        /// default generator of match
        /// </summary>
        internal CricketMatch(string homeTeam, string awayTeam, DateTime date, MatchType matchType, bool homeTeamBattingFirst, string location = null)
        {
            MatchData = new MatchInfo(homeTeam, awayTeam, location, date, matchType);

            FirstInnings = new CricketInnings(homeTeamBattingFirst ? homeTeam : awayTeam, homeTeamBattingFirst ? awayTeam : homeTeam);
            SecondInnings = new CricketInnings(homeTeamBattingFirst ? awayTeam : homeTeam, homeTeamBattingFirst ? homeTeam : awayTeam);
        }

        internal CricketMatch(MatchInfo info)
        {
            MatchData = info;
            FirstInnings = new CricketInnings();
            SecondInnings = new CricketInnings();
        }

        public CricketMatch()
        {
            MatchData = new MatchInfo();
            FirstInnings = new CricketInnings();
            SecondInnings = new CricketInnings();
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
            if (FirstInnings != null)
            {
            }
            players.UnionWith(FirstInnings.Players(teamName));
            players.UnionWith(SecondInnings.Players(teamName));
            return players.ToList();
        }

        /// <inheritdoc/>
        public void EditInfo(string homeTeam = null, string awayTeam = null, DateTime? date = null, string location = null, MatchType? typeOfMatch = null, ResultType? result = null)
        {
            if (!string.IsNullOrEmpty(homeTeam))
            {
                string oldHomeTeam = MatchData.HomeTeam;
                MatchData.HomeTeam = homeTeam;
                FirstInnings.UpdateTeamName(oldHomeTeam, homeTeam);
            }
            if (!string.IsNullOrEmpty(awayTeam))
            {
                string oldAwayTeam = MatchData.AwayTeam;
                MatchData.AwayTeam = awayTeam;
                FirstInnings.UpdateTeamName(oldAwayTeam, awayTeam);
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
        public void SetBattingFirst(bool isHomeTeam)
        {
            string battingFirst = isHomeTeam ? MatchData.HomeTeam : MatchData.AwayTeam;
            string bowlingFirst = isHomeTeam ? MatchData.AwayTeam : MatchData.HomeTeam;

            FirstInnings.BattingTeam = battingFirst;
            FirstInnings.FieldingTeam = bowlingFirst;
            SecondInnings.BattingTeam = bowlingFirst;
            SecondInnings.FieldingTeam = battingFirst;

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

        public void SetInnings(CricketInnings innings, bool first)
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
            return InningsHelpers.SelectInnings(FirstInnings, SecondInnings, innings => batting ? string.Equals(innings.BattingTeam, team) : string.Equals(innings.FieldingTeam, team));
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
                if (innings.IsBowlingPlayer(player))
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

        public MatchResult MatchResult()
        {
            var firstInningsScore = FirstInnings.Score();
            var secondInningsScore = SecondInnings.Score();

            if (firstInningsScore.Runs == 0 && secondInningsScore.Runs == 0)
            {
                return Match.Result.MatchResult.IsNoResultMatch(FirstInnings.BattingTeam, FirstInnings.FieldingTeam);
            }
            else if (firstInningsScore.Runs > secondInningsScore.Runs)
            {
                return new MatchResult(FirstInnings.BattingTeam, FirstInnings.FieldingTeam, winningRunMargin: firstInningsScore.Runs - secondInningsScore.Runs);
            }
            else if (firstInningsScore.Runs < secondInningsScore.Runs)
            {
                return new MatchResult(SecondInnings.BattingTeam, SecondInnings.FieldingTeam, winningWicketMargin: 10 - secondInningsScore.Wickets);
            }
            else
            {
                return new MatchResult("", "");
            }

            throw new NotImplementedException();
        }

        public ReportBuilder SerializeToString(DocumentType exportType)
        {
            ReportBuilder sb = new ReportBuilder(exportType, new ReportSettings(useColours: true, useDefaultStyle: false, useScripts: true));
            _ = sb.WriteHeader("")
                .WriteTitle($"{MatchData.HomeTeam} vs {MatchData.AwayTeam}. Venue: {MatchData.Location}. Date: {MatchData.Date}. Type of Match: {MatchData.Type}", DocumentElement.h1);

            var firstInningsString = FirstInnings.SerializeToString(exportType);
            _ = sb.Append(firstInningsString)
                .AppendLine();

            var secondInningsString = SecondInnings.SerializeToString(exportType);
            _ = sb.Append(secondInningsString)
                .AppendLine();


            var result = MatchResult();
            _ = sb.WriteTitle($"Result", DocumentElement.h2)
                .WriteParagraph(new[] { $"Match Result: ", result.ToString() })
                .WriteFooter();
            return sb;
        }
    }
}
