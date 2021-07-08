using System;
using System.Collections.Generic;
using System.Linq;
using StructureCommon.Extensions;
using StructureCommon.Validation;

namespace CricketStructures.Match
{
    public class MatchInfo : IEquatable<MatchInfo>, IValidity
    {
        public string HomeTeam
        {
            get;
            set;
        }

        public string AwayTeam
        {
            get;
            set;
        }

        public bool AtHome
        {
            get;
            set;
        }

        public string Location
        {
            get;
            set;
        }

        public DateTime Date
        {
            get;
            set;
        }

        public MatchType Type
        {
            get;
            set;
        }

        public MatchInfo()
        {
        }

        public MatchInfo(string homeTeam, string awayTeam, bool atHome, string location, DateTime date, MatchType type)
        {
            HomeTeam = homeTeam;
            AwayTeam = awayTeam;
            Location = location;
            Date = date;
            Type = type;
            AtHome = atHome;
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

        public bool Equals(MatchInfo other)
        {
            return Equals(other.Date, other.HomeTeam, other.AwayTeam);
        }

        public bool Equals(DateTime date, string homeTeam, string awayTeam)
        {
            return HomeTeam.Equals(homeTeam) && AwayTeam.Equals(awayTeam) & Date.Equals(date);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as MatchInfo);
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public string OppositionName()
        {
            return AtHome ? AwayTeam : HomeTeam;
        }
    }
}
