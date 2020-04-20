using Cricket.Interfaces;
using Cricket.Match;
using Cricket.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Cricket
{
    public class CricketSeason : ICricketSeason
    {
        public override bool Equals(object obj)
        {
            if (obj is CricketSeason season)
            {
                return Name.Equals(season.Name) && Year.Equals(season.Year);
            }

            return false;
        }

        public bool SameSeason(DateTime year, string name)
        {
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
            return false;
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