using Cricket.Interfaces;
using Cricket.Match;
using Cricket.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Validation;

namespace Cricket
{
    public class CricketSeason : ICricketSeason, IValidity
    {
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
            return Year.Year.ToString() + " " +  Name;
        }

        /// <inheritdoc/>
        public string Name
        { 
            get; 
            set; 
        }

        /// <inheritdoc/>
        public DateTime Year
        { 
            get; 
            set; 
        }

        /// <inheritdoc/>
        public List<PlayerName> Players
        {
            get { return SeasonsMatches.SelectMany(match => match.PlayerNames).Distinct().ToList(); }
        }

        List<CricketMatch> fSeasonsMatches = new List<CricketMatch>();
        public List<CricketMatch> SeasonsMatches
        {
            get { return fSeasonsMatches; }
            set { fSeasonsMatches = value; } 
        }

        /// <inheritdoc/>
        [XmlIgnoreAttribute]
        public List<ICricketMatch> Matches
        {
            get 
            { 
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
                SeasonsMatches.Add(new CricketMatch(info));
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
                results.AddRange(match.Validation());
            }
            if (Year == null)
            {
                var yearNotSet = new ValidationResult();
                yearNotSet.IsValid = false;
                yearNotSet.PropertyName = nameof(Year);
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
