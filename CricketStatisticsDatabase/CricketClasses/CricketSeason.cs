using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Cricket.Interfaces;
using Cricket.Match;
using Cricket.Player;
using Common.Structure.Extensions;
using Common.Structure.Validation;

namespace Cricket
{
    public class CricketSeason : ICricketSeason, IValidity
    {
        /// <inheritdoc/>
        public DateTime Year
        {
            get;
            set;
        }

        /// <inheritdoc/>
        public string Name
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

        public void CalculateGamesPlayed(MatchType[] matchTypes)
        {
            GamesPlayed = 0;
            NumberWins = 0;
            NumberLosses = 0;
            NumberDraws = 0;
            NumberTies = 0;
            foreach (var match in Matches)
            {
                if (matchTypes.Contains(match.MatchData.Type))
                {
                    GamesPlayed++;

                    if (match.Result == Cricket.Match.ResultType.Win)
                    {
                        NumberWins++;
                    }
                    if (match.Result == Cricket.Match.ResultType.Loss)
                    {
                        NumberLosses++;
                    }
                    if (match.Result == Cricket.Match.ResultType.Draw)
                    {
                        NumberDraws++;
                    }
                    if (match.Result == Cricket.Match.ResultType.Tie)
                    {
                        NumberTies++;
                    }
                }
            }
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

        public override bool Equals(object obj)
        {
            if (obj is CricketSeason season)
            {
                if (string.IsNullOrEmpty(Name))
                {
                    if (string.IsNullOrEmpty(season.Name))
                    {
                        return Year.Equals(season.Year);
                    }

                    return false;
                }

                return Name.Equals(season.Name) && Year.Equals(season.Year);
            }

            return false;
        }

        public void EditPlayerName(PlayerName oldName, PlayerName newName)
        {
            Matches.ForEach(match => match.EditPlayerName(oldName, newName));
        }

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

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return Year.Year.ToString() + " " + Name;
        }

        /// <inheritdoc/>
        [XmlIgnore]
        public List<PlayerName> Players
        {
            get
            {
                return SeasonsMatches.SelectMany(match => match.PlayerNames).Distinct().ToList();
            }
        }

        public List<CricketMatch> SeasonsMatches
        {
            get;
            set;
        } = new List<CricketMatch>();

        /// <inheritdoc/>
        [XmlIgnoreAttribute]
        public List<ICricketMatch> Matches
        {
            get
            {
                SeasonsMatches.Sort((match1, match2) => match1.MatchData.Date.CompareTo(match2.MatchData.Date));
                return SeasonsMatches.Select(match => (ICricketMatch)match).ToList();
            }
        }

        /// <inheritdoc/>
        public void EditSeasonName(DateTime year, string name)
        {
            Year = year;
            Name = name;
        }

        /// <inheritdoc/>
        /// This is currently not implemented.
        public ICricketMatch GetMatch(DateTime date, string opposition)
        {
            if (ContainsMatch(date, opposition))
            {
                return SeasonsMatches.First(match => match.SameMatch(date, opposition));
            }

            return null;
        }

        public bool AddMatch(MatchInfo info)
        {
            if (!ContainsMatch(info.Date, info.Opposition))
            {
                var match = new CricketMatch(info);
                match.PlayerAdded += OnPlayerAdded;
                SeasonsMatches.Add(match);
                SeasonsMatches.Sort((match1, match2) => match1.MatchData.Date.CompareTo(match2.MatchData.Date));
                return true;
            }

            return false;
        }

        public bool ContainsMatch(DateTime date, string opposition)
        {
            return SeasonsMatches.Any(match => match.SameMatch(date, opposition));
        }

        public bool RemoveMatch(DateTime date, string opposition)
        {
            int removed = SeasonsMatches.RemoveAll(match => match.SameMatch(date, opposition));
            if (removed == 1)
            {
                return true;
            }
            if (removed == 0)
            {
                return false;
            }

            throw new Exception($"Had {removed} matches with info {date} and {opposition}, but should have at most 1.");
        }

        public bool Validate()
        {
            return !Validation().Any(validation => !validation.IsValid);
        }

        public List<ValidationResult> Validation()
        {
            var results = new List<ValidationResult>();
            foreach (var match in SeasonsMatches)
            {
                results.AddValidations(match.Validation(), ToString());
            }
            if (Year == default)
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

        public CricketSeason(DateTime year, string name)
        {
            Name = name;
            Year = year;
        }
    }
}
