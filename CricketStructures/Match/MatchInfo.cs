using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

using Common.Structure.Extensions;
using Common.Structure.Validation;

namespace CricketStructures.Match
{
    public sealed class MatchInfo : IEquatable<MatchInfo>, IValidity
    {
        [XmlAttribute(AttributeName = "H")]
        public string HomeTeam
        {
            get;
            set;
        }

        [XmlAttribute(AttributeName = "A")]
        public string AwayTeam
        {
            get;
            set;
        }

        [XmlAttribute(AttributeName = "L")]
        public string Location
        {
            get;
            set;
        }

        [XmlAttribute(AttributeName = "D")]
        public DateTime Date
        {
            get;
            set;
        }

        [XmlAttribute(AttributeName = "T")]
        public MatchType Type
        {
            get;
            set;
        }

        public MatchInfo()
        {
        }

        public MatchInfo(string homeTeam, string awayTeam, string location, DateTime date, MatchType type)
        {
            HomeTeam = homeTeam;
            AwayTeam = awayTeam;
            Location = location;
            Date = date;
            Type = type;
        }

        public override string ToString()
        {
            return $"{Date.ToUkDateString()} - {HomeTeam} v {AwayTeam} at {Location}";
        }

        public bool Validate()
        {
            return !Validation().Any(validation => !validation.IsValid);
        }

        public List<ValidationResult> Validation()
        {
            List<ValidationResult> results = new List<ValidationResult>();
            results.AddIfNotNull(Validating.IsNotNullOrEmpty(HomeTeam, nameof(HomeTeam), ToString()));
            results.AddIfNotNull(Validating.IsNotNullOrEmpty(AwayTeam, nameof(AwayTeam), ToString()));
            return results;
        }

        public override bool Equals(object obj)
        {
            if (obj is MatchInfo matchInfo)
            {
                return Equals(matchInfo);
            }

            return false;
        }

        public bool Equals(MatchInfo other)
        {
            return Equals(other.Date, other.HomeTeam, other.AwayTeam);
        }

        public bool Equals(DateTime date, string homeTeam, string awayTeam)
        {
            return string.Equals(HomeTeam, homeTeam) && string.Equals(AwayTeam, awayTeam) & DateTime.Equals(Date, date);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(HomeTeam, AwayTeam, Date);
        }

        public string OppositionName(string teamName)
        {
            if (teamName.Equals(HomeTeam))
            {
                return AwayTeam;
            }
            if (teamName.Equals(AwayTeam))
            {
                return HomeTeam;
            }
            return null;
        }
    }
}
