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

        private static string vs = "vs";
        private static string venue = "Venue";
        private static string DateString = "Date";
        private static string TypeOfMatch = "Type of Match";

        public static MatchInfo FromString(string stringForm)
        {
            int indexOfvs = stringForm.IndexOf(vs);
            int indexOfvenueName = stringForm.IndexOf(venue);
            int indexOfDate = stringForm.IndexOf(DateString);
            int indexOfType = stringForm.IndexOf(TypeOfMatch);


            string homeTeam = stringForm.Substring(0, indexOfvs).Trim();
            string awayTeam = stringForm.Substring(indexOfvs + vs.Length, indexOfvenueName - indexOfvs - vs.Length).Trim().Trim('.');
            string location = stringForm.Substring(indexOfvenueName + venue.Length + 1, indexOfDate - indexOfvenueName - venue.Length - 2).Trim().Trim('.');
            string dateString = stringForm.Substring(indexOfDate + DateString.Length + 1, indexOfType - indexOfDate - DateString.Length - 3).Trim();
            string typeString = stringForm.Substring(indexOfType + TypeOfMatch.Length + 2, stringForm.Length - indexOfType - TypeOfMatch.Length - 2).Trim();
            DateTime date = DateTime.Parse(dateString);
            MatchType matchType = Enum.Parse<MatchType>(typeString);

            return new MatchInfo(homeTeam, awayTeam, location, date, matchType);
        }

        public override string ToString()
        {
            return $"{HomeTeam} vs {AwayTeam}. Venue: {Location}. Date: {Date}. Type of Match: {Type}";
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
            return string.Equals(HomeTeam ?? "", homeTeam ?? "")
                && string.Equals(AwayTeam ?? "", awayTeam ?? "")
                && DateTime.Equals(Date, date);
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
