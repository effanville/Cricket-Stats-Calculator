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
    public sealed class CricketMatch : ICricketMatch, IValidity, IEquatable<CricketMatch>
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
        public List<PlayerName> MenOfMatch
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

        [XmlIgnore]
        private Dictionary<string, HashSet<PlayerName>> PlayersByTeam
        {
            get;
            set;
        } = new Dictionary<string, HashSet<PlayerName>>();

        /// <summary>
        /// default generator of match
        /// </summary>
        internal CricketMatch(string homeTeam, string awayTeam, DateTime date, MatchType matchType, bool homeTeamBattingFirst, string location = null)
        : this(
              new MatchInfo(homeTeam, awayTeam, location, date, matchType),
              new CricketInnings(homeTeamBattingFirst ? homeTeam : awayTeam, homeTeamBattingFirst ? awayTeam : homeTeam),
              new CricketInnings(homeTeamBattingFirst ? awayTeam : homeTeam, homeTeamBattingFirst ? homeTeam : awayTeam))
        {
        }

        private CricketMatch(MatchInfo info, CricketInnings firstInnings, CricketInnings secondInnings)
        {
            MatchData = info;
            FirstInnings = firstInnings;
            SecondInnings = secondInnings;
            MenOfMatch = new List<PlayerName>();
        }

        internal CricketMatch(MatchInfo info)
            : this(info, new CricketInnings(), new CricketInnings())
        {
        }

        public CricketMatch()
            : this(new MatchInfo(), new CricketInnings(), new CricketInnings())
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
        public bool Played(string team, PlayerName person)
        {
            return Players(team).Contains(person);
        }

        /// <inheritdoc/>
        public bool SameMatch(DateTime date, string homeTeam, string awayTeam)
        {
            return MatchData.Equals(date, homeTeam, awayTeam);
        }

        /// <inheritdoc/>
        public HashSet<PlayerName> Players(string team)
        {
            string teamName = team;
            if (!PlayersByTeam.ContainsKey(team))
            {
                var players = new HashSet<PlayerName>();
                players.UnionWith(FirstInnings.Players(teamName));
                players.UnionWith(SecondInnings.Players(teamName));
                PlayersByTeam[team] = players;
            }

            return PlayersByTeam[team];
        }

        /// <inheritdoc/>
        public void EditInfo(string homeTeam = null, string awayTeam = null, DateTime? date = null, string location = null, MatchType? typeOfMatch = null, ResultType? result = null)
        {
            if (!string.IsNullOrEmpty(homeTeam))
            {
                string oldHomeTeam = MatchData.HomeTeam;
                MatchData.HomeTeam = homeTeam;
                FirstInnings.UpdateTeamName(oldHomeTeam, homeTeam);
                if (!string.IsNullOrEmpty(oldHomeTeam))
                {
                    _ = PlayersByTeam.Remove(oldHomeTeam);
                }

                _ = Players(homeTeam);
            }
            if (!string.IsNullOrEmpty(awayTeam))
            {
                string oldAwayTeam = MatchData.AwayTeam;
                MatchData.AwayTeam = awayTeam;
                FirstInnings.UpdateTeamName(oldAwayTeam, awayTeam);

                if (!string.IsNullOrEmpty(oldAwayTeam))
                {
                    _ = PlayersByTeam.Remove(oldAwayTeam);
                }

                _ = Players(awayTeam);
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
        public void SetBatting(string team, PlayerName player, Wicket howOut, int runs, int order, int wicketFellAt, int teamScoreAtWicket, PlayerName fielder = null, bool wasKeeper = false, PlayerName bowler = null)
        {
            var innings = InningsHelpers.SelectBattingInnings(FirstInnings, SecondInnings, team);
            innings?.SetBatting(player, howOut, runs, order, wicketFellAt, teamScoreAtWicket, fielder, wasKeeper, bowler);
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
        public void SetBowling(string team, PlayerName player, Over overs, int maidens, int runsConceded, int wickets, int wides = 0, int noBalls = 0)
        {
            var innings = InningsHelpers.SelectFieldingInnings(FirstInnings, SecondInnings, team);
            innings?.SetBowling(player, overs, maidens, runsConceded, wickets);
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
            var innings = InningsHelpers.SelectFieldingInnings(FirstInnings, SecondInnings, team);
            if (innings != null)
            {
                return innings.GetAllFielding(team);
            }

            return null;
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


        private static readonly string InningsTitle = "Innings of:";

        public static CricketMatch CreateFromScorecard(DocumentType exportType, string scorecard)
        {
            var scorecardDocument = ReportSplitter.SplitReportString(exportType, scorecard);
            int indexOfFirstInnings = scorecardDocument.FindIndex(0, part => part.ConstituentString.Contains(InningsTitle));

            var firstInningsDoc = scorecardDocument.GetSubDocument(indexOfFirstInnings);
            var secondInningsDoc = scorecardDocument.GetSubDocumentFrom(indexOfFirstInnings + 1, part => part.ConstituentString.Contains(InningsTitle));

            var info = MatchInfo.FromString(scorecardDocument.FirstTextPart(DocumentElement.h1).Text);
            var firstInnings = CricketInnings.CreateFromScorecard(firstInningsDoc);
            firstInnings.FieldingTeam = info.OppositionName(firstInnings.BattingTeam);
            var secondInnings = CricketInnings.CreateFromScorecard(secondInningsDoc);
            secondInnings.FieldingTeam = info.OppositionName(secondInnings.BattingTeam);

            return new CricketMatch(info, firstInnings, secondInnings);
        }

        public ReportBuilder SerializeToString(DocumentType exportType)
        {
            ReportBuilder sb = new ReportBuilder(exportType, new ReportSettings(useColours: true, useDefaultStyle: false, useScripts: true));
            _ = sb.WriteHeader("")
                .WriteTitle($"{MatchData}", DocumentElement.h1);

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

        public bool IsMatchDataEqual(CricketMatch other)
        {
            return MatchData.Equals(other.MatchData);
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return Equals(obj as CricketMatch);
        }

        /// <inheritdoc/>
        public bool Equals(CricketMatch other)
        {
            return IsMatchDataEqual(other)
                && FirstInnings.Equals(other.FirstInnings)
                && SecondInnings.Equals(other.SecondInnings)
                && Result.Equals(other.Result)
                && Enumerable.SequenceEqual(MenOfMatch, other.MenOfMatch);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(MatchData);
            hash.Add(Result);
            hash.Add(MenOfMatch);
            hash.Add(FirstInnings);
            hash.Add(SecondInnings);
            return hash.ToHashCode();
        }
    }
}
