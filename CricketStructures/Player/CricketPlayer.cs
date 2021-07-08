using System.Collections.Generic;
using System.Linq;
using CricketStructures.Player.Interfaces;
using StructureCommon.Validation;

namespace CricketStructures.Player
{
    /// <summary>
    /// Class containing all information about a player of cricket
    /// </summary>
    public class CricketPlayer : ICricketPlayer, IValidity
    {
        public override string ToString()
        {
            return Name.ToString();
        }
        public CricketPlayer()
        {
        }

        public CricketPlayer(PlayerName name)
        {
            Name = name;
        }

        public CricketPlayer(string surname, string forename)
        {
            Name = new PlayerName(surname, forename);
        }


        public void EditName(string surname, string forename)
        {
            PlayerName newNames = new PlayerName(surname, forename);
            if (!Name.Equals(newNames))
            {
                Name = newNames;
            }
        }

        public bool Validate()
        {
            return !Validation().Any(validation => !validation.IsValid);
        }

        public List<ValidationResult> Validation()
        {
            return Name.Validation();
        }

        public PlayerName Name
        {
            get;
            set;
        }
    }
}