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
            return Year.Year.ToString() + Name;
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

        public List<CricketPlayer> SeasonsPlayers
        {
            get;
            set;
        }

        /// <inheritdoc/>
        [XmlIgnoreAttribute]
        public List<ICricketPlayer> Players
        {
            get 
            {
                return new List<ICricketPlayer>();
            }
        }

        public List<CricketMatch> SeasonsMatches
        { 
            get; 
            set; 
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
        /// This is currently not implemented.
        public ICricketMatch GetMatch()
        {
            return new CricketMatch();
        }

        public bool AddMatch()
        {
            throw new NotImplementedException();
        }

        public bool ContainsMatch()
        {
            throw new NotImplementedException();
        }

        public bool RemoveMatch()
        {
            throw new NotImplementedException();
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