using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using CricketStructures.Match;
using CricketStructures.Player;
using Common.Structure.Extensions;
using Common.Structure.Validation;

namespace CricketStructures.Season
{
    /// <inheritdoc/>
    public sealed class CricketSeason : ICricketSeason, IValidity
    {
        /// <inheritdoc/>
        [XmlAttribute]
        public DateTime Year
        {
            get;
            set;
        }

        /// <inheritdoc/>
        [XmlAttribute]
        public string Name
        {
            get;
            set;
        }

        [XmlArray]
        public List<CricketMatch> SeasonsMatches
        {
            get;
            set;
        } = new List<CricketMatch>();

        /// <inheritdoc/>
        [XmlIgnore]
        public IReadOnlyList<ICricketMatch> Matches => SeasonsMatches.Select(match => (ICricketMatch)match).ToList();

        /// <inheritdoc/>
        public SeasonRecord CalculateGamesPlayed(MatchType[] matchTypes)
        {
            int gamesPlayed = 0;
            var results = new Dictionary<ResultType, int>();
            foreach (var match in Matches)
            {
                if (matchTypes.Contains(match.MatchData.Type))
                {
                    gamesPlayed++;
                    results[match.Result]++;
                }
            }

            return new SeasonRecord(Year, gamesPlayed, results);
        }

        public event EventHandler PlayerAdded;

        private void OnPlayerAdded(object obj, EventArgs args)
        {
            PlayerAdded?.Invoke(obj, args);
        }

        public void SetupEventListening()
        {
            foreach (var match in SeasonsMatches)
            {
                match.PlayerAdded += OnPlayerAdded;
            }
        }

        /// <inheritdoc/>
        public void EditPlayerName(PlayerName oldName, PlayerName newName)
        {
            SeasonsMatches.ForEach(match => match.EditPlayerName(oldName, newName));
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return Year.Year.ToString() + " " + Name;
        }

        /// <inheritdoc/>
        public IReadOnlyList<PlayerName> Players(string teamName, MatchType[] matchTypes)
        {
            return SeasonsMatches.SelectMany(match => MatchPlayers(match)).Distinct().ToList();

            List<PlayerName> MatchPlayers(ICricketMatch match)
            {
                if (matchTypes.Contains(match.MatchData.Type))
                {
                    return match.Players(teamName).ToList();
                }

                return new List<PlayerName>();
            }
        }

        /// <inheritdoc/>
        public bool Played(string teamName, MatchType[] matchTypes, PlayerName player)
        {
            return Players(teamName, matchTypes).Contains(player);
        }

        /// <inheritdoc/>
        public void EditSeasonName(DateTime year, string name)
        {
            Year = year;
            Name = name;
        }

        /// <inheritdoc/>
        /// This is currently not implemented.
        public ICricketMatch GetMatch(DateTime date, string homeTeam, string awayTeam)
        {
            if (ContainsMatch(date, homeTeam, awayTeam))
            {
                return SeasonsMatches.First(match => match.SameMatch(date, homeTeam, awayTeam));
            }

            return null;
        }

        /// <inheritdoc/>
        public bool AddMatch(MatchInfo info)
        {
            if (!ContainsMatch(info.Date, info.HomeTeam, info.AwayTeam))
            {
                var match = new CricketMatch(info);
                match.PlayerAdded += OnPlayerAdded;
                SeasonsMatches.Add(match);
                SeasonsMatches.Sort((a, b) => a.MatchData.Date.CompareTo(b.MatchData.Date));
                return true;
            }

            SeasonsMatches.Sort((a, b) => a.MatchData.Date.CompareTo(b.MatchData.Date));
            return false;
        }

                /// <inheritdoc/>
        public bool AddMatch(CricketMatch match)
        {
            var info = match.MatchData;
            if (!ContainsMatch(info.Date, info.HomeTeam, info.AwayTeam))
            {
                match.PlayerAdded += OnPlayerAdded;
                SeasonsMatches.Add(match);
                SeasonsMatches.Sort((a, b) => a.MatchData.Date.CompareTo(b.MatchData.Date));
                return true;
            }

            SeasonsMatches.Sort((a, b) => a.MatchData.Date.CompareTo(b.MatchData.Date));
            return false;
        }

        /// <inheritdoc/>
        public bool ContainsMatch(DateTime date, string homeTeam, string awayTeam)
        {
            SeasonsMatches.Sort((a, b) => a.MatchData.Date.CompareTo(b.MatchData.Date));
            return SeasonsMatches.Any(match => match.SameMatch(date, homeTeam, awayTeam));
        }

        /// <inheritdoc/>
        public bool RemoveMatch(DateTime date, string homeTeam, string awayTeam)
        {
            int removed = SeasonsMatches.RemoveAll(match => match.SameMatch(date, homeTeam, awayTeam));
            if (removed == 1)
            {
                return true;
            }
            if (removed == 0)
            {
                return false;
            }

            throw new Exception($"Had {removed} matches with info {date} and {homeTeam}-{awayTeam}, but should have at most 1.");
        }

        /// <inheritdoc/>
        public bool Validate()
        {
            return !Validation().Any(validation => !validation.IsValid);
        }

        /// <inheritdoc/>
        public List<ValidationResult> Validation()
        {
            var results = new List<ValidationResult>();
            foreach (var match in SeasonsMatches)
            {
                results.AddValidations(match.Validation(), ToString());
            }
            if (Year.Equals(new DateTime()))
            {
                var yearNotSet = new ValidationResult
                {
                    IsValid = false,
                    PropertyName = nameof(Year)
                };
                yearNotSet.AddMessage($"{nameof(Year)} must be set.");
            }
            return results;
        }

        public CricketSeason()
        {
        }

        internal CricketSeason(DateTime year, string name)
        {
            Name = name;
            Year = year;
        }

        /// <inheritdoc/>
        public bool SameSeason(DateTime year, string name)
        {
            if (string.IsNullOrEmpty(Name))
            {
                if (string.IsNullOrEmpty(name))
                {
                    return Year.Equals(year);
                }

                return false;
            }

            return Year.Equals(year) && Name.Equals(name);
        }

        public override bool Equals(object obj)
        {
            if (obj is CricketSeason season)
            {
                return SameSeason(season.Year, season.Name);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Year, Name);
        }
    }
}
